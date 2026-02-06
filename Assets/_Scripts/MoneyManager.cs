using UnityEngine;
using TMPro;
using DG.Tweening; // <--- ÝÒÎÉ ÑÒÐÎÊÈ ÍÅ ÕÂÀÒÀËÎ

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [Header("UI Reference")]
    public TextMeshProUGUI moneyText;

    private int _currentMoney = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddMoney(int amount)
    {
        _currentMoney += amount;
        UpdateUI();

        // Òåïåðü îøèáêà ïðîïàäåò
        if (moneyText != null)
            moneyText.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 10, 1);
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = _currentMoney.ToString() + "$";
    }
}