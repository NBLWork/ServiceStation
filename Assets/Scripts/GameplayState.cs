using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class GameplayState : MonoBehaviour
{
    [SerializeField] private Button restart;
    [SerializeField] private GameObject restartBg;
    [SerializeField] private Image restartIcon;
    [SerializeField] private Sprite win;
    [SerializeField] private Sprite lose;

    private Text repText;
    private Text incomeText;

    [Inject] private readonly GameEvents gameEvents;

    #region MONO
    private void Awake()
    {
        repText = restartBg.transform.GetChild(1).GetComponent<Text>();
        incomeText = restartBg.transform.GetChild(2).GetComponent<Text>();
    }

    private void OnEnable()
    {
        gameEvents.GameStateEvent += ChangeGameState;
        gameEvents.ResultEvent += ShowResult;
        gameEvents.RepEvent += SetIcon;

        Invoke("Restart", 0.5f);
    }

    private void OnDisable()
    {
        if (IsInvoking())
        {
            CancelInvoke("Restart");
        }
        gameEvents.SwitchGameState(false);

        gameEvents.ResultEvent -= ShowResult;
        gameEvents.GameStateEvent -= ChangeGameState;
        gameEvents.RepEvent -= SetIcon;

        restartBg.SetActive(false);
        restart.onClick.RemoveListener(Restart);
    }
    #endregion

    private void Restart()
    {
        gameEvents.SwitchGameState(true);
    }

    private void ChangeGameState(bool isActive)
    {
        if (!isActive)
        {
            restartBg.SetActive(true);
            restart.onClick.AddListener(Restart);
        }
        else
        {
            restartBg.SetActive(false);
            SetIcon(true);
            restart.onClick.RemoveListener(Restart);
        }
    }

    private void ShowResult(int rep, int money)
    {
        repText.text = rep.ToString();
        incomeText.text = money.ToString();
    }

    private void SetIcon(bool inTime)
    {
        restartIcon.sprite = inTime ? win : lose;
        restartIcon.SetNativeSize();
    }
}
