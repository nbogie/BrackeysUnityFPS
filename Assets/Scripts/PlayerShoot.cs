using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private WeaponManager weaponManager;
    private PlayerWeapon currentWeapon;

    private Camera camForShootRaycast;

    #region Unity Callbacks
    void Start()
    {
        layerMask = LayerMask.GetMask("Default", "RemotePlayers");
        weaponManager = GetComponent<WeaponManager>();
        camForShootRaycast = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {

            CancelInvoke();
            weaponManager.TogglePrimarySecondary();
        }

        currentWeapon = weaponManager.GetCurrentWeapon();

        //Regardless of *current* weapon type, 
        //cancel any previously invoked fire() when releasing trigger
        if (Input.GetButtonUp("Fire1"))
        {
            CancelInvoke();
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            if (currentWeapon.CanAutoFire())
            {
                InvokeRepeating("FireWeapon", 0.000001f, currentWeapon.fireDelay);
            }
            else
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
        Vector3 dir = camForShootRaycast.transform.forward + Random.insideUnitSphere * currentWeapon.accuracy;
        if (Physics.Raycast(camForShootRaycast.transform.position, dir, out hitInfo, currentWeapon.range, layerMask))
        {
            if (hitInfo.collider.tag == "Player")
            {
                CmdPlayerWasShot(hitInfo.collider.name, currentWeapon.damage);
            }
            SpawnWeaponImpact(hitInfo.point, hitInfo.normal);
        }
        //TODO: play a sound
        //TODO: show muzzle flash
        //TODO: apply a recoil
    }

    [Client]
    private void SpawnWeaponImpact(Vector3 point, Vector3 normal)
    {
        Instantiate(weaponManager.GetImpactEffect(), point, Quaternion.Euler(normal));
    }

    [Command]
    void CmdPlayerWasShot(string _ID, int dmg)
    {
        Debug.Log("Cmd: " + _ID + " has been shot, dmg " + dmg);
        Player p = GameManager.GetPlayer(_ID);

        p.RpcTakeDamage(dmg);
    }
}
