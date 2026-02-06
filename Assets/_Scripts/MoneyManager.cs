using UnityEngine;
using TMPro;
using DG.Tweening;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI moneyText;

    private int _currentMoney = 0;
    public int CurrentMoney => _currentMoney;

    //  люч, по которому будем искать деньги в пам€ти
    private const string SAVE_KEY_MONEY = "Save_Money";

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadProgress(); // <--- «ј√–”∆ј≈ћ—я ѕ–» —“ј–“≈
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        _currentMoney += amount;
        SaveProgress(); // <--- —ќ’–јЌя≈ћ—я

        UpdateUI();
        if (moneyText != null)
            moneyText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 10, 1);
    }

    public void SpendMoney(int amount)
    {
        _currentMoney -= amount;
        if (_currentMoney < 0) _currentMoney = 0;

        SaveProgress(); // <--- —ќ’–јЌя≈ћ—я
        UpdateUI();
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = _currentMoney.ToString() + "$";
    }

    // --- Ћќ√» ј —ќ’–јЌ≈Ќ»я ---

    void SaveProgress()
    {
        PlayerPrefs.SetInt(SAVE_KEY_MONEY, _currentMoney);
        PlayerPrefs.Save(); // ѕринудительно записываем на диск
    }

    void LoadProgress()
    {
        // ≈сли сохранение есть - берем его, иначе 0
        if (PlayerPrefs.HasKey(SAVE_KEY_MONEY))
        {
            _currentMoney = PlayerPrefs.GetInt(SAVE_KEY_MONEY);
        }
    }

    // ѕолезно дл€ тестов: ”далить сохранение
    [ContextMenu("Delete Save File")]
    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("—ќ’–јЌ≈Ќ»я ”ƒјЋ≈Ќџ!");
    }
}