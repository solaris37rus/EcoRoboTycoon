using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class UnlockZone : MonoBehaviour
{
    [Header("Настройки Сохранения")]
    public string zoneID = "Zone_1"; // Уникальное имя зоны

    [Header("Экономика")]
    public int price = 50;
    public float spendSpeed = 0.1f;
    public int spendAmount = 1;

    [Header("Анимация")]
    public float floatSpeed = 2f;
    public float floatHeight = 0.3f;

    [Header("Визуал")]
    public TextMeshPro priceText;
    public GameObject visualModel;

    [Header("Событие при открытии")]
    public UnityEvent OnUnlocked;

    private bool _isUnlocking = false;
    private Vector3 _startPos;

    // Ключ для сохранения цены (например "Zone_1_Price")
    private string PriceSaveKey => zoneID + "_Price";

    void Start()
    {
        // 1. Сначала проверяем, может она уже совсем открыта?
        if (CheckIfAlreadyUnlocked())
        {
            InstantUnlock();
            return;
        }

        // 2. Если не открыта, проверяем: может мы уже часть оплатили?
        LoadPriceProgress();

        UpdateText();

        if (priceText != null)
        {
            _startPos = priceText.transform.localPosition;
        }
    }

    void Update()
    {
        if (priceText != null)
        {
            float newY = _startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            priceText.transform.localPosition = new Vector3(_startPos.x, newY, _startPos.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isUnlocking) return;

        if (other.CompareTag("Player") || other.GetComponent<PlayerMovement>())
        {
            StartCoroutine(UnlockRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.GetComponent<PlayerMovement>())
        {
            StopAllCoroutines();
            _isUnlocking = false;

            // На всякий случай сохраняем при выходе игрока из зоны
            SavePriceProgress();
        }
    }

    IEnumerator UnlockRoutine()
    {
        _isUnlocking = true;

        while (price > 0)
        {
            if (MoneyManager.Instance.CurrentMoney >= spendAmount)
            {
                MoneyManager.Instance.SpendMoney(spendAmount);
                price -= spendAmount;

                UpdateText();

                // СОХРАНЯЕМ ПРОГРЕСС ЦЕНЫ ТУТ
                // Чтобы если игра вылетит, вложения не пропали
                SavePriceProgress();
            }
            yield return new WaitForSeconds(spendSpeed);
        }

        Unlock();
    }

    void UpdateText()
    {
        if (priceText != null)
            priceText.text = price.ToString() + "$";
    }

    void Unlock()
    {
        // Помечаем, что зона полностью куплена
        PlayerPrefs.SetInt(zoneID, 1);
        PlayerPrefs.Save();

        _isUnlocking = false;
        OnUnlocked.Invoke();

        if (visualModel != null) visualModel.SetActive(false);
        gameObject.SetActive(false);
    }

    void InstantUnlock()
    {
        OnUnlocked.Invoke();
        if (visualModel != null) visualModel.SetActive(false);
        gameObject.SetActive(false);
    }

    // --- НОВАЯ ЛОГИКА СОХРАНЕНИЯ ЦЕНЫ ---

    void SavePriceProgress()
    {
        PlayerPrefs.SetInt(PriceSaveKey, price);
        // PlayerPrefs.Save(); // Можно не вызывать часто, Unity сама сохранит при выходе
    }

    void LoadPriceProgress()
    {
        // Если есть сохраненная цена для этой зоны - загружаем её
        if (PlayerPrefs.HasKey(PriceSaveKey))
        {
            price = PlayerPrefs.GetInt(PriceSaveKey);
        }
    }

    bool CheckIfAlreadyUnlocked()
    {
        return PlayerPrefs.GetInt(zoneID) == 1;
    }
}