using DG.Tweening;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class DailyIncome : MonoBehaviour
{
    [SerializeField] private Button getButton;
    [SerializeField] private Text incomeText;
    [SerializeField] private int cooldown;
    [SerializeField] private int incomeValue;

    [SerializeField] private string enableText;
    [SerializeField] private string disableText;

    [Inject] private readonly GameData data;

    private void OnEnable()
    {
        getButton.image.color = Color.gray;
        incomeText.text = disableText;
        if (CheckDaily())
        {
            getButton.onClick.AddListener(GetIncome);
            getButton.image.DOColor(Color.white, 0.4f);
            incomeText.text = enableText;
        }
    }

    private void OnDisable()
    {
        getButton.onClick.AddListener(GetIncome);
    }

    private bool CheckDaily()
    {
        System.TimeSpan span = System.DateTime.Now.Subtract(data.IncomeDate);
        int hours = (int)span.TotalHours;
        return (hours < cooldown) ? false : true;
    }

    private void GetIncome()
    {
        getButton.onClick.RemoveListener(GetIncome);
        getButton.image.DOColor(Color.gray, 0.4f);
        incomeText.text = disableText;

        data.UpdateMoney(incomeValue);
        data.RefreshDaily(System.DateTime.Now);
    }
}
