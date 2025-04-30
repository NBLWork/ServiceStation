using System;
using UnityEngine;
using UnityEngine.UI;

public enum GameScreen
{
    Menu = 0,
    Gameplay = 1,
    BonusGame = 2,
    Settings = 3,
    Rules = 4
}
public class ScreenManager : MonoBehaviour
{
    public event Action<GameScreen> ChangeStateEvent;

    [SerializeField] private Button[] toMenu;
    [SerializeField] private Button toSettings;
    [SerializeField] private Button toBonusGame;
    [SerializeField] private Button toGameplay;
    [SerializeField] private Button toRules;
    [SerializeField] private Button toPrivacy;
    [SerializeField] private string privacyURL;

    private GameScreen currentState;

    #region MONO
    private void Awake()
    {
       currentState = GameScreen.Menu;
    }

    private void OnEnable()
    {
        for (int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.AddListener(() => { TriggerChange(GameScreen.Menu); });
        }
        toSettings.onClick.AddListener(() => { TriggerChange(GameScreen.Settings); });
        toBonusGame.onClick.AddListener(() => { TriggerChange(GameScreen.BonusGame); });
        toGameplay.onClick.AddListener(() => { TriggerChange(GameScreen.Gameplay); });
        toRules.onClick.AddListener(() => { TriggerChange(GameScreen.Rules); });

        toPrivacy.onClick.AddListener(CheckPrivacy);
    }

    private void Start()
    {
        TriggerChange(currentState);
    }

    private void OnDisable()
    {
        for (int i = 0; i < toMenu.Length; i++)
        {
            toMenu[i].onClick.RemoveAllListeners();
        }
        toSettings.onClick.RemoveAllListeners();
        toBonusGame.onClick.RemoveAllListeners();
        toGameplay.onClick.RemoveAllListeners();
        toRules.onClick.RemoveAllListeners();

        toPrivacy.onClick.RemoveListener(CheckPrivacy);
    }
    #endregion


    private void TriggerChange(GameScreen state)
    {
        currentState = state;
        ChangeStateEvent?.Invoke(state);
    }

    private void CheckPrivacy()
    {
        Application.OpenURL(privacyURL);
    }

}
