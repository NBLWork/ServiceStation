using System;
using DG.Tweening;
using Reflex.Attributes;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 stopPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float movingTime;

    [SerializeField] private Engines engines;

    private Transform planeTransform;
    private bool init = false;

    [Inject] private readonly GameEvents gameEvents;

    private void Awake()
    {
        planeTransform = transform;
        init = true;
    }

    public void HidePlane(Action callback)
    {
        gameEvents.PlaySound(AudioEffect.Engine);
        engines.BoostEngines();
        planeTransform.DOMove(endPos, 2f)
            .SetId("plane")
            .OnComplete(() => { engines.ShutEngines(false); callback(); });
    }

    public void ShowPlane(Action callback)
    {
        planeTransform.position = startPos;
        engines.ShutEngines(true);
        engines.BoostEngines();
        planeTransform.DOMove(stopPos, 1.5f)
            .SetId("plane")
            .OnComplete(() => {
                engines.SlowEngines();
                callback(); });
    }

    public void ResetPlane()
    {
        engines.ShutEngines(false);
        planeTransform.position = endPos;
    }

    public void MovePlane()
    {        
        planeTransform.position = startPos;
        engines.ShutEngines(true);
        planeTransform.DOMove(endPos, movingTime)
            .SetId("plane_move")
            .OnComplete(() => {
                engines.ShutEngines(false);
                Invoke(nameof(MovePlane), 1f);
            });
    }

    public void Init()
    {
        if (!init)
        {
            planeTransform = transform;
            init = true;
        }
    }
}
