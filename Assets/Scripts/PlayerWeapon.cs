using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    public float fireDelay = 0.5f;
    public float accuracy = 0.01f;
    public float range = 10f;
    public int damage = 5;

    public bool CanAutoFire()
    {
        return fireDelay > 0;
    }
}
