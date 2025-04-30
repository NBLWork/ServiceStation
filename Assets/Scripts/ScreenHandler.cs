using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PanelStates
{
    public GameObject panel;
    public List<bool> states = new List<bool>();
}
public class ScreenHandler : MonoBehaviour
{
    [SerializeField] private PanelStates[] panels;

    [FormerlySerializedAs("uiManager")] [SerializeField] private ScreenManager screenManager;

    #region MONO
    private void OnEnable()
    {
        screenManager.ChangeStateEvent += SwitchState;
    }

    private void OnDisable()
    {
        screenManager.ChangeStateEvent -= SwitchState;
    }
    #endregion

    private void SwitchState(GameScreen state)
    {
        int currentStateIndex = (int)state;
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].panel.SetActive(panels[i].states[currentStateIndex]);
        }
    }
}
