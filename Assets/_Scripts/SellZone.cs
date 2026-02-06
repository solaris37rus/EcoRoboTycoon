using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SellZone : MonoBehaviour
{
    [Header("Настройки")]
    public Transform sellPoint; // Точка, куда летят предметы при продаже (центр зоны)
    public int pricePerItem = 5; // Цена за 1 мусор
    public float sellSpeed = 0.1f; // Как быстро продаются предметы (интервал)

    private Coroutine _sellCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что зашел Игрок и у него есть скрипт Collector
        if (other.TryGetComponent(out Collector collector))
        {
            if (_sellCoroutine != null) StopCoroutine(_sellCoroutine);
            _sellCoroutine = StartCoroutine(SellRoutine(collector));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collector>())
        {
            if (_sellCoroutine != null) StopCoroutine(_sellCoroutine);
            _sellCoroutine = null;
        }
    }

    IEnumerator SellRoutine(Collector collector)
    {
        // Пока у игрока есть предметы
        while (collector.HasItems)
        {
            // 1. Забираем предмет у игрока
            Transform item = collector.RemoveLastItem();

            if (item != null)
            {
                // 2. Отвязываем от игрока
                item.SetParent(null);

                // 3. Анимация полета в точку продажи
                item.DOMove(sellPoint.position, 0.3f).OnComplete(() =>
                {
                    // Когда долетел:
                    MoneyManager.Instance.AddMoney(pricePerItem); // Даем деньги
                    Destroy(item.gameObject); // Уничтожаем объект
                });
            }

            // Ждем перед следующей продажей
            yield return new WaitForSeconds(sellSpeed);
        }
    }
}