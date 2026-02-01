using UnityEngine;

public class fireParticle : MonoBehaviour
{
    public ParticleSystem ps;
    public void startParticle()
    {
        ps.Play();
    }
    public void Stop()
    {
        ps.Stop();
    }
}
