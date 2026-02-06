using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MoneyVisuals : MonoBehaviour
{
    public static MoneyVisuals Instance;

    [Header("Настройки")]
    public GameObject coinPrefab;
    public Transform targetUI;
    public Canvas parentCanvas;

    [Header("Параметры")]
    public float flyDuration = 0.8f; // (Попробуй уменьшить до 0.8, так динамичнее)
    public int coinsAmount = 5;

    private Camera _mainCamera;

    void Awake()
    {
        Instance = this;
        _mainCamera = Camera.main;
    }

    // ТЕПЕРЬ МЫ ПРИНИМАЕМ СЮДА ЕЩЕ И СУММУ (int amount)
    public void SpawnFlyingCoins(Vector3 worldPosition, int amount)
    {
        // 1. Запускаем визуальный фейерверк
        for (int i = 0; i < coinsAmount; i++)
        {
            float delay = i * 0.05f; // Делаем задержку поменьше, чтобы кучнее летели
            CreateCoin(worldPosition, delay);
        }

        // 2. Ждем, пока долетят, и ТОЛЬКО ТОГДА даем деньги
        // Используем DOVirtual.DelayedCall - это таймер от DOTween
        DOVirtual.DelayedCall(flyDuration, () => {
            MoneyManager.Instance.AddMoney(amount);
        });
    }

    void CreateCoin(Vector3 startPos3D, float delay)
    {
        GameObject coin = Instantiate(coinPrefab, parentCanvas.transform);
        Vector2 screenPos = _mainCamera.WorldToScreenPoint(startPos3D);
        coin.transform.position = screenPos;
        coin.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.PrependInterval(delay);

        Vector3 randomOffset = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), 0);

        seq.Append(coin.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        seq.Join(coin.transform.DOMove(coin.transform.position + randomOffset, 0.2f));

        // Полет
        seq.Append(coin.transform.DOMove(targetUI.position, flyDuration).SetEase(Ease.InCubic));

        // Исчезновение
        seq.Join(coin.transform.DOScale(0f, 0.1f).SetDelay(flyDuration - 0.1f));

        seq.OnComplete(() => Destroy(coin));
    }
}