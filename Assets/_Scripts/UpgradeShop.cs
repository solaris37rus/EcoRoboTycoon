using UnityEngine;
using TMPro;

public class UpgradeShop : MonoBehaviour
{
    [Header("Настройки UI")]
    public GameObject shopPanel; // Ссылка на Canvas панель магазина

    [Header("UI Кнопки (Тексты)")]
    public TextMeshProUGUI speedPriceText;
    public TextMeshProUGUI speedLvlText;

    public TextMeshProUGUI capacityPriceText;
    public TextMeshProUGUI capacityLvlText;

    [Header("Баланс Скорости")]
    public float startSpeed = 5f;
    public float speedStep = 0.5f; // Сколько добавляем за уровень
    public int speedBasePrice = 50;

    [Header("Баланс Вместимости")]
    public int startCapacity = 10;
    public int capacityStep = 5;   // +5 мест за уровень
    public int capacityBasePrice = 100;

    // Внутренние переменные уровней
    private int _speedLvl = 1;
    private int _capacityLvl = 1;

    // Ссылки на игрока
    private PlayerMovement _playerMove;
    private Collector _playerCollector;

    void Start()
    {
        // Ищем игрока на сцене
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj)
        {
            _playerMove = playerObj.GetComponent<PlayerMovement>();
            _playerCollector = playerObj.GetComponent<Collector>();
        }

        LoadStats(); // Загружаем сохранения
        ApplyStats(); // Применяем их к игроку
        UpdateUI();

        // Скрываем магазин при старте
        if (shopPanel != null) shopPanel.SetActive(false);
    }

    // --- Вход в зону ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shopPanel.SetActive(true); // Показать меню
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shopPanel.SetActive(false); // Скрыть меню
        }
    }

    // --- Логика Покупки ---

    public void BuySpeedUpgrade()
    {
        int price = GetSpeedPrice();
        if (MoneyManager.Instance.CurrentMoney >= price)
        {
            MoneyManager.Instance.SpendMoney(price);
            _speedLvl++;
            SaveStats();
            ApplyStats();
            UpdateUI();
        }
    }

    public void BuyCapacityUpgrade()
    {
        int price = GetCapacityPrice();
        if (MoneyManager.Instance.CurrentMoney >= price)
        {
            MoneyManager.Instance.SpendMoney(price);
            _capacityLvl++;
            SaveStats();
            ApplyStats();
            UpdateUI();
        }
    }

    // --- Математика и UI ---

    int GetSpeedPrice() => speedBasePrice * _speedLvl; // Простая формула: цена * уровень
    int GetCapacityPrice() => capacityBasePrice * _capacityLvl;

    void ApplyStats()
    {
        // Считаем: База + (Уровень * Шаг)
        if (_playerMove) _playerMove.SetSpeed(startSpeed + (_speedLvl - 1) * speedStep);
        if (_playerCollector) _playerCollector.SetCapacity(startCapacity + (_capacityLvl - 1) * capacityStep);
    }

    void UpdateUI()
    {
        if (speedPriceText) speedPriceText.text = GetSpeedPrice() + "$";
        if (speedLvlText) speedLvlText.text = "LVL " + _speedLvl;

        if (capacityPriceText) capacityPriceText.text = GetCapacityPrice() + "$";
        if (capacityLvlText) capacityLvlText.text = "LVL " + _capacityLvl;
    }

    // --- Сохранения ---
    void SaveStats()
    {
        PlayerPrefs.SetInt("Upg_Speed", _speedLvl);
        PlayerPrefs.SetInt("Upg_Capacity", _capacityLvl);
        PlayerPrefs.Save();
    }

    void LoadStats()
    {
        if (PlayerPrefs.HasKey("Upg_Speed")) _speedLvl = PlayerPrefs.GetInt("Upg_Speed");
        if (PlayerPrefs.HasKey("Upg_Capacity")) _capacityLvl = PlayerPrefs.GetInt("Upg_Capacity");
    }
}