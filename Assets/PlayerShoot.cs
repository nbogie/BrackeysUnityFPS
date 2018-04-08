using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    [Header("Weapon settings")]
    [SerializeField]
    private GameObject weaponImpactEffect;
    [SerializeField]
    private float weaponFireDelay = 0.5f;
    [SerializeField]
    private float accuracy = 0.01f;
    [SerializeField]
    private float weaponRange = 10f;
    [SerializeField]
    private int weaponDamage = 5;
    [SerializeField]
    private LayerMask layerMask;

    private Camera myCam;
    private float timeForNextFiring;

    #region Unity Callbacks
    void Start()
    {
        myCam = GetComponentInChildren<Camera>();
        layerMask = LayerMask.GetMask("Default", "RemotePlayers");
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (timeForNextFiring <= Time.time)
            {
                FireWeapon();
                timeForNextFiring = Time.time + weaponFireDelay;
            }
        }
    }
    #endregion

    [Client]
    private void FireWeapon()
    {
        RaycastHit hitInfo;
        Vector3 dir = myCam.transform.forward + Random.insideUnitSphere * accuracy;
        if (Physics.Raycast(myCam.transform.position, dir, out hitInfo, weaponRange, layerMask))
        {
            if (hitInfo.collider.tag == "Player")
            {
                CmdPlayerWasShot(hitInfo.collider.name, weaponDamage);
            }
            //Debug.Log("hit: " + hitInfo.point);
            SpawnWeaponImpact(hitInfo.point, hitInfo.normal);
        }
        //TODO: play a sound
        //TODO: show muzzle flash
        //TODO: apply a recoil
    }

    [Client]
    private void SpawnWeaponImpact(Vector3 point, Vector3 normal)
    {
        Instantiate(weaponImpactEffect, point, Quaternion.Euler(normal));
    }

    [Command]
    void CmdPlayerWasShot(string _ID, int dmg)
    {
        Debug.Log("Cmd: " + _ID + " has been shot, dmg " + dmg);
        Player p = GameManager.GetPlayer(_ID);

        p.TakeDamage(dmg);
    }
}
