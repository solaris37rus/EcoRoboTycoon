using UnityEngine;
using DG.Tweening; // Важно
using System.Collections;

public class SellZone : MonoBehaviour
{
    [Header("Настройки")]
    public Transform sellPoint;
    public int pricePerItem = 5;
    public float sellSpeed = 0.1f;

    private Coroutine _sellCoroutine;

    private void OnTriggerEnter(Collider other)
    {
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
        while (collector.HasItems)
        {
            Transform item = collector.RemoveLastItem();

            if (item != null)
            {
                // 1. ОТМЕНЯЕМ все прошлые анимации (полет в рюкзак)
                // Это уберет ошибку "Target is missing"
                item.DOKill();

                item.SetParent(null);

                // 2. Запускаем полет в кассу
                item.DOMove(sellPoint.position, 0.3f).OnComplete(() =>
                {
                    // 3. Снова DOKill перед уничтожением (на всякий случай)
                    item.DOKill();

                    MoneyManager.Instance.AddMoney(pricePerItem);
                    Destroy(item.gameObject);
                });
            }

            yield return new WaitForSeconds(sellSpeed);
        }
    }
}