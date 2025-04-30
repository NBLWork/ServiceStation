using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum GameResources
{
    Gear,
    Fuel,
    Diagnostics,
    Repairs
}

public class GameData : MonoBehaviour
{
    [SerializeField] private ScoreManager score;

    [SerializeField] private int money = 50;
    [SerializeField] private int reputation = 0;
    [SerializeField] private int gear = 20;
    [SerializeField] private int fuel = 20;
    [SerializeField] private int diagnostics = 20;
    [SerializeField] private int repairs = 20;

    public int Money => money;
    public int Reputation => reputation;
    public int Gear => gear;
    public int Fuel => fuel;
    public int Diagnostics => diagnostics;
    public int Repairs => repairs;

    public DateTime IncomeDate => incomeDate;
    private DateTime incomeDate;

    private void OnEnable()
    {
        money = (PlayerPrefs.HasKey("_Money")) ? PlayerPrefs.GetInt("_Money") : money;
        reputation = (PlayerPrefs.HasKey("_Reputation")) ? PlayerPrefs.GetInt("_Reputation") : reputation;

        gear = (PlayerPrefs.HasKey("_Gear")) ? PlayerPrefs.GetInt("_Gear") : gear;
        fuel = (PlayerPrefs.HasKey("_Fuel")) ? PlayerPrefs.GetInt("_Fuel") : fuel;
        diagnostics = (PlayerPrefs.HasKey("_Diagnostics")) ? PlayerPrefs.GetInt("_Diagnostics") : diagnostics;
        repairs = (PlayerPrefs.HasKey("_Repairs")) ? PlayerPrefs.GetInt("_Repairs") : repairs;

        incomeDate = PlayerPrefs.HasKey("_DailyIncome") ? new DateTime(
           Convert.ToInt64(PlayerPrefs.GetString("_DailyIncome")))
           .ToLocalTime() : DateTime.Now.AddDays(-1);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("_Money", money);
        PlayerPrefs.SetInt("_Reputation", reputation);

        PlayerPrefs.SetInt("_Gear", gear);
        PlayerPrefs.SetInt("_Fuel", fuel);
        PlayerPrefs.SetInt("_Diagnostics", diagnostics);
        PlayerPrefs.SetInt("_Repairs", repairs);

        PlayerPrefs.SetString("_DailyIncome", "" + incomeDate.ToUniversalTime().Ticks);
    }

    public void UpdateMoney(int value)
    {
        money += value;
        if (money < 0)
        {
            money = 0;
        }
        score.UpdateGlobal(false, money);
    }

    public void UpdateRep(int value)
    {
        reputation += value;
        reputation = Mathf.Clamp(reputation, -100, 100);
        score.UpdateGlobal(true, reputation);
    }

    public void UpdateRes(GameResources id, int value, bool gameplay)
    {
        switch (id)
        {
            case GameResources.Gear: gear += value; score.UpdateResources(id, gear, gameplay); break;
            case GameResources.Fuel: fuel += value; score.UpdateResources(id, fuel, gameplay); break;
            case GameResources.Diagnostics: diagnostics += value; score.UpdateResources(id, diagnostics, gameplay); break;
            case GameResources.Repairs: repairs += value; score.UpdateResources(id, repairs, gameplay); break;
            default: throw new NotSupportedException();
        }
    }

    public void UpdateAllRes(bool gameplay)
    {
        UpdateRes(GameResources.Gear, 0, gameplay);
        UpdateRes(GameResources.Fuel, 0, gameplay);
        UpdateRes(GameResources.Diagnostics, 0, gameplay);
        UpdateRes(GameResources.Repairs, 0, gameplay);
    }

    public void RefreshDaily(DateTime newDate)
    {
        incomeDate = newDate;
    }
}
