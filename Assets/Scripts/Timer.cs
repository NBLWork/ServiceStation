using System.Collections;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private int timeMin;
    [SerializeField] private int timeMax;
    [SerializeField] private Text timerUI;

    private float timeLeft;
    private System.Random rand = new System.Random();

    private Color initialColor;

    [Inject] private readonly GameEvents gameEvents;

    private void Awake()
    {
        initialColor = timerUI.color;
    }

    private IEnumerator StartTimer()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimer(timeLeft);
            yield return null;
        }
        if (timeLeft <= 0)
        {
            timerUI.color = Color.red;
            gameEvents.TriggerTimeout();
        }
    }

    private void UpdateTimer(float timeLeft)
    {
        if (timeLeft < 0)
        {
            timeLeft = 0;
        }
        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        timerUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Activate()
    {
        timerUI.color = initialColor;
        timeLeft = rand.Next(timeMin, timeMax);
        StartCoroutine(StartTimer());
    }

    public void Deactivate()
    {
        StopAllCoroutines();
    }

}
