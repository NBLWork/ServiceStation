using Reflex.Core;
using UnityEngine;

public class ReflexInit : MonoBehaviour, IInstaller
{
    [SerializeField] private GameData data;
        
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.AddSingleton(data);
        builder.AddSingleton(typeof(GameEvents));
        builder.AddSingleton(typeof(Randomizer));
    }

}
