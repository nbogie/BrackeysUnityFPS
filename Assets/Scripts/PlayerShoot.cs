using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    public PlayerWeapon currentWeapon;

    [Header("Weapon settings")]
    [SerializeField]
    private GameObject weaponImpactEffect;
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
        if (currentWeapon.CanAutoFire())
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("FireWeapon", 0.000001f, currentWeapon.fireDelay);
            }
            if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke();
            }
        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                FireWeapon();
            }
        }
    }
    #endregion

    [Client]
    private void FireWeapon()
    {
        RaycastHit hitInfo;
        Vector3 dir = myCam.transform.forward + Random.insideUnitSphere * currentWeapon.accuracy;
        if (Physics.Raycast(myCam.transform.position, dir, out hitInfo, currentWeapon.range, layerMask))
        {
            if (hitInfo.collider.tag == "Player")
            {
                CmdPlayerWasShot(hitInfo.collider.name, currentWeapon.damage);
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

        p.RpcTakeDamage(dmg);
    }
}
