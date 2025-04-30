using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void Awake()
    {
        SetSafeArea();
    }

    private void SetSafeArea()
    {
        var safeArea = Screen.safeArea;
        var localRectTransform = GetComponent<RectTransform>();

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMax.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.y /= Screen.height;

        localRectTransform.anchorMin = anchorMin;
        localRectTransform.anchorMax = anchorMax;
    }
}
