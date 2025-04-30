using UnityEngine;

public class Engines : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;

    public void BoostEngines()
    {
        for(int i = 0; i < particles.Length; i++)
        {
            var mainM = particles[i].main;
            mainM.startSpeed = 30f;
        }
    }

    public void SlowEngines()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            var mainM = particles[i].main;
            mainM.startSpeed = 20f;
        }
    }

    public void ShutEngines(bool activate)
    {
        if (activate)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Play();
            }
        }
        else
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Stop();
            }
        }
    }
}
