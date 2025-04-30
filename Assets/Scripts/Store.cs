using System;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    [SerializeField] private Button gearB;
    [SerializeField] private Button fuelB;
    [SerializeField] private Button diagnosticsB;
    [SerializeField] private Button repairsB;

    [SerializeField] private int gearPrice;
    [SerializeField] private int fuelPrice;
    [SerializeField] private int diagnosticsPrice;
    [SerializeField] private int repairsPrice;

    [SerializeField] private Text gearText;
    [SerializeField] private Text fuelText;
    [SerializeField] private Text diagnosticsText;
    [SerializeField] private Text repairsText;

    private int gearP;
    private int fuelP;
    private int diagnosticsP;
    private int repairsP;

    [Inject] private readonly GameData data;
    [Inject] private readonly GameEvents gameEvents;

    private void OnEnable()
    {
        data.UpdateAllRes(false);
        data.UpdateRep(0);
        data.UpdateMoney(0);

        SetPrices();
        CheckBuffs();
        gearB.onClick.AddListener(() => BuyRes(GameResources.Gear));
        fuelB.onClick.AddListener(() => BuyRes(GameResources.Fuel));
        diagnosticsB.onClick.AddListener(() => BuyRes(GameResources.Diagnostics));
        repairsB.onClick.AddListener(() => BuyRes(GameResources.Repairs));
    }

    private void OnDisable()
    {
        gearB.onClick.RemoveAllListeners();
        fuelB.onClick.RemoveAllListeners();
        diagnosticsB.onClick.RemoveAllListeners();
        repairsB.onClick.RemoveAllListeners();
    }

    private void SetPrices()
    {
        int rep = data.Reputation;
        float divider = rep > 0 ? 200f : 100f;
        float modifyer = 1f - (rep / divider);

        gearP = (int) (gearPrice * modifyer);
        fuelP = (int)(fuelPrice * modifyer);
        diagnosticsP = (int)(diagnosticsPrice * modifyer);
        repairsP = (int)(repairsPrice * modifyer);

        gearText.text = gearP.ToString();
        fuelText.text = fuelP.ToString();
        diagnosticsText.text = diagnosticsP.ToString();
        repairsText.text = repairsP.ToString();
    }

    private void CheckBuffs()
    {
        gearB.interactable = data.Money >= gearP;
        fuelB.interactable = data.Money >= fuelP;
        diagnosticsB.interactable = data.Money >= diagnosticsP;
        repairsB.interactable = data.Money >= repairsP;
    }

    private void BuyRes(GameResources resId)
    {
        data.UpdateRes(resId, 1, false);
        int price = resId switch
        {
            GameResources.Gear => gearP,
            GameResources.Fuel => fuelP,
            GameResources.Diagnostics => diagnosticsP,
            GameResources.Repairs => repairsP,
            _ => throw new NotSupportedException()
        };
        gameEvents.PlaySound(AudioEffect.Resource);
        data.UpdateMoney(-price);
        CheckBuffs();
    }
}
