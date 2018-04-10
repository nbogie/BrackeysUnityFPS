using UnityEngine;

public class WeaponGraphics : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem effectPSysPrefab;

    [SerializeField]
    private Transform firePoint;

    public void TriggerVisualFireEffect()
    {
        ParticleSystem ps = Instantiate(effectPSysPrefab, firePoint.position, firePoint.rotation) as ParticleSystem;
        ps.Play();
        Destroy(ps.gameObject, 3f);
    }
}
