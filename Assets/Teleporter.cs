using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    private AudioSource audioSource;

    [SerializeField]
    private Teleporter otherTeleporter;
    [SerializeField]
    private GameObject effect;

    [SerializeField]
    private Transform exitTransform;

    [SerializeField]
    private Material tpMaterial;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ColorRecursively(transform);
    }
    void ColorRecursively(Transform givenTransform)
    {
        Renderer r = givenTransform.GetComponent<Renderer>();
        if (r)
        {
            r.material = tpMaterial;
        }
            
        foreach (Transform t in givenTransform)
        {
            ColorRecursively(t);
        }
    }

    private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag == "Player")
        {
            TeleportThingTo(other.gameObject, otherTeleporter);

        }
	}

    private void TeleportThingTo(GameObject obj, Teleporter otherTele)
    {
        obj.transform.position = otherTele.exitTransform.position;
        obj.transform.rotation = otherTele.exitTransform.rotation;
        TriggerGraphicalEffect();
        PlaySound();
        otherTele.TriggerGraphicalEffect();
        otherTele.PlaySound();
    }

    private void PlaySound()
    {
        audioSource.Play();
        audioSource.pitch = Random.Range(0.7f, 0.85f);
    }

    void TriggerGraphicalEffect()
    {
        Instantiate(effect, transform.position, transform.rotation);
    }
}
