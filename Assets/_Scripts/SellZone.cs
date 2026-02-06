using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SellZone : MonoBehaviour
{
    [Header("Настройки")]
    public Transform sellPoint;      // Точка, куда летит мусор (касса)
    public int pricePerItem = 5;     // Цена за 1 предмет
    public float sellSpeed = 0.1f;   // Скорость продажи

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
            // Забираем предмет из рюкзака
            Transform item = collector.RemoveLastItem();

            if (item != null)
            {
                // 1. Сбрасываем анимации и родителя (чтобы он перестал лететь за игроком)
                item.DOKill();
                item.SetParent(null);

                // 2. Летим в точку продажи (кассу)
                item.DOMove(sellPoint.position, 0.3f).OnComplete(() =>
                {
                    // 3. Когда долетели:
                    item.DOKill();

                    // --- ГЛАВНОЕ ИЗМЕНЕНИЕ ---
                    // Мы не начисляем деньги здесь. Мы запускаем монетки.
                    // А монетки сами начислят деньги, когда долетят до UI.

                    if (MoneyVisuals.Instance != null)
                    {
                        // Передаем: (Откуда лететь, СКОЛЬКО денег начислить)
                        MoneyVisuals.Instance.SpawnFlyingCoins(sellPoint.position, pricePerItem);
                    }
                    else
                    {
                        // Страховка: Если вдруг скрипт визуала удален или выключен,
                        // начисляем деньги сразу, чтобы игрок их не потерял.
                        MoneyManager.Instance.AddMoney(pricePerItem);
                    }
                    // -------------------------

                    Destroy(item.gameObject);
                });
            }

            yield return new WaitForSeconds(sellSpeed);
        }
    }
}