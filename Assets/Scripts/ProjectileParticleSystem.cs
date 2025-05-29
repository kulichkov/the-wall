using System.Collections;
using UnityEngine;

public class ProjectileParticleSystem : MonoBehaviour
{
    public ProjectileParticlesPool projectileParticlesPool;
    private ParticleSystem particles;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void SetEmitting(bool isEmmiting)
    {
        if (isEmmiting)
        {
            particles.Play();
        }
        else
        {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
    public void Release()
    {
        StartCoroutine(PostponeRelease());
    }

    private IEnumerator PostponeRelease()
    {
        yield return new WaitForSeconds(0.5f);
        projectileParticlesPool.Release(this);
    }
}
