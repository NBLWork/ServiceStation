using UnityEngine;
using UnityEngine.UI;

public class SettingsOption : MonoBehaviour
{
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;
    [SerializeField] private Sprite leftOff;
    [SerializeField] private Sprite leftOn;
    [SerializeField] private Sprite rightOff;
    [SerializeField] private Sprite rightOn;

    private Toggle toggle;
    private Vector2 handlePosition;

    #region MONO
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        Switch(toggle.isOn);
        toggle.onValueChanged.AddListener(Switch);
    }
    
    #endregion

    private void Switch(bool isOn)
    {
        leftImage.sprite = isOn ? leftOff : leftOn;
        rightImage.sprite = isOn ? rightOn : rightOff;
    }
}
