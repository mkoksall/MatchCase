using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    void Start()
    {
        DragAndDrop.OnPlayParticle += PlayParticle;

        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnDestroy()
    {
        DragAndDrop.OnPlayParticle -= PlayParticle;
    }

    private void PlayParticle(Transform tileTransform)
    {
        _particleSystem.transform.position = tileTransform.position;
        _particleSystem.Play();
    }
}
