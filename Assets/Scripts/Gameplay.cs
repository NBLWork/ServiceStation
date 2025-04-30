using System;
using DG.Tweening;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private Planes planes;

    [SerializeField] private Button gearB;
    [SerializeField] private Button fuelB;
    [SerializeField] private Button diagnosticsB;
    [SerializeField] private Button repairB;

    [SerializeField] private Button giveB;

    [SerializeField] private Text gearO;
    [SerializeField] private Text fuelO;
    [SerializeField] private Text diagnosticsO;
    [SerializeField] private Text repairsO;

    [SerializeField] private Text gearP;
    [SerializeField] private Text fuelP;
    [SerializeField] private Text diagnosticsP;
    [SerializeField] private Text repairsP;

    [SerializeField] private GameObject requestPanel;
    [SerializeField] private GameObject playerPanel;

    private Request sourceRequest;
    private Request playerRequest;

    private int currentRep;
    private int currentIncome;

    [Inject] private readonly Randomizer randomizer;
    [Inject] private readonly GameEvents gameEvents;
    [Inject] private readonly GameData data;

    private void OnEnable()
    {
        data.UpdateAllRes(true);
        data.UpdateRep(0);
        data.UpdateMoney(0);

        currentRep = 0;

        gameEvents.GameStateEvent += SwitchGame;
    }

    private void OnDisable()
    {
        data.UpdateRep(currentRep);
        requestPanel.SetActive(false);
        playerPanel.SetActive(false);

        giveB.onClick.RemoveListener(Give);

        gearB.onClick.RemoveAllListeners();
        fuelB.onClick.RemoveAllListeners();
        diagnosticsB.onClick.RemoveAllListeners();
        repairB.onClick.RemoveAllListeners();

        gameEvents.GameStateEvent -= SwitchGame;
        planes.ResetPlane();
    }

    private void SwitchGame(bool activate)
    {
        if (activate)
        {
            playerRequest.ResetOrder();
            UpdateOrder(true);

            planes.ActivatePlane(ShipCallback);
            gameEvents.TimeOutEvent += TriggerTimeout;

            gearB.onClick.AddListener(() => PickRes(GameResources.Gear));
            fuelB.onClick.AddListener(() => PickRes(GameResources.Fuel));
            diagnosticsB.onClick.AddListener(() => PickRes(GameResources.Diagnostics));
            repairB.onClick.AddListener(() => PickRes(GameResources.Repairs));

            giveB.onClick.AddListener(Give);
        }
        else
        {
            gearB.onClick.RemoveAllListeners();
            fuelB.onClick.RemoveAllListeners();
            diagnosticsB.onClick.RemoveAllListeners();
            repairB.onClick.RemoveAllListeners();

            timer.Deactivate();
            gameEvents.TimeOutEvent -= TriggerTimeout;
        }
    }

    private void ShipCallback()
    {
        gameEvents.PlaySound(AudioEffect.Timer);
        GenerateOrder();
        requestPanel.transform.localScale = playerPanel.transform.localScale = Vector3.zero;
        requestPanel.SetActive(true);
        playerPanel.SetActive(true);
        DOTween.Sequence()
            .SetId("game_panel")
            .Append(requestPanel.transform.DOScale(new Vector3(1, 1, 1), 0.5f))
            .Join(playerPanel.transform.DOScale(new Vector3(1, 1, 1), 0.5f));        
        timer.Activate();
    }

    private void TriggerTimeout()
    {
        currentRep -= 40;
        gameEvents.PlaySound(AudioEffect.Timer);
        gameEvents.UpdateRep(false);
    }

    private void GenerateOrder()
    {
        currentIncome = 0;
        sourceRequest.gear = randomizer.GenerateInt(0, 5);
        sourceRequest.fuel = randomizer.GenerateInt(0, 5);
        sourceRequest.diagnostics = randomizer.GenerateInt(0, 5);
        sourceRequest.repairs = randomizer.GenerateInt(0, 5);

        UpdateOrder();
    }

    private void Give()
    {
        giveB.onClick.RemoveListener(Give);

        timer.Deactivate();
        currentIncome = playerRequest.gear + playerRequest.fuel + playerRequest.diagnostics + playerRequest.repairs;
        if(currentRep < 0)
        {
            currentIncome /= 2;
        }

        if (sourceRequest.Equals(playerRequest))
        {
            currentIncome += 10;
        }
        int misses = sourceRequest.GetMisses();
        currentRep += 5 * (4 - misses);
        currentRep -= 15 * misses;
        
        gameEvents.DoResult(currentRep, currentIncome);
        data.UpdateRep(currentRep);
        data.UpdateMoney(currentIncome);

        currentRep = 0;

        DOTween.Sequence()
            .SetId("game_panel")
            .Append(requestPanel.transform.DOScale(Vector3.zero, 0.5f))
            .Join(playerPanel.transform.DOScale(Vector3.zero, 0.5f))
            .OnKill(() => { playerPanel.SetActive(false); requestPanel.SetActive(false); });

        gameEvents.PlaySound(AudioEffect.Reward);
        gameEvents.SwitchGameState(false);
    }

    private void PickRes(GameResources id)
    {
        switch (id)
        {
            case GameResources.Gear:
                if (data.Gear > 0)
                {
                    playerRequest.gear++;
                    data.UpdateRes(id, -1, true);
                }
                break;
            case GameResources.Fuel:
                if (data.Fuel > 0)
                {
                    playerRequest.fuel++;
                    data.UpdateRes(id, -1, true);
                }
                break;
            case GameResources.Diagnostics:
                if (data.Diagnostics > 0)
                {
                    playerRequest.diagnostics++;
                    data.UpdateRes(id, -1, true);
                }
                break;
            case GameResources.Repairs:
                if (data.Repairs > 0)
                {
                    playerRequest.repairs++;
                    data.UpdateRes(id, -1, true);
                }
                break;
            default: throw new NotSupportedException();
        }
        gameEvents.PlaySound(AudioEffect.Resource);
        UpdateOrder(true);
    }

    private void UpdateOrder(bool player = false)
    {
        if (player)
        {
            gearP.text = playerRequest.gear.ToString();
            fuelP.text = playerRequest.fuel.ToString();
            diagnosticsP.text = playerRequest.diagnostics.ToString();
            repairsP.text = playerRequest.repairs.ToString();
        }
        else
        {
            gearO.text = sourceRequest.gear.ToString();
            fuelO.text = sourceRequest.fuel.ToString();
            diagnosticsO.text = sourceRequest.diagnostics.ToString();
            repairsO.text = sourceRequest.repairs.ToString();
        }
    }
}
