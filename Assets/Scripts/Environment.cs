using UnityEngine;
using UnityEngine.Serialization;

public class Environment : MonoBehaviour
{
    [FormerlySerializedAs("ships")] [SerializeField] private Plane[] planes;

    private void OnEnable()
    {
        for (int i = 0; i < planes.Length; i++)
        {
            planes[i].Init();
            planes[i].MovePlane();
        }
    }
}
