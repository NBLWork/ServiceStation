using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Planes : MonoBehaviour
{
    [FormerlySerializedAs("ships")] [SerializeField] private Plane[] planes;

    private Plane currentPlane;

    private Action callback;
    private System.Random rand = new System.Random();

    private void HideCallback()
    {
        currentPlane = planes[rand.Next(0, planes.Length)];
        currentPlane.ShowPlane(ShowCallback);
    }

    private void ShowCallback()
    {
        callback();
    }

    public void ActivatePlane(Action shipCallback)
    {
        callback = shipCallback;

        if(currentPlane != null)
        {
            currentPlane.HidePlane(HideCallback);
        }
        else
        {
            HideCallback();
        }
    }

    public void ResetPlane()
    {
        if(currentPlane != null)
        {
            currentPlane.ResetPlane();
        }
    }
}
