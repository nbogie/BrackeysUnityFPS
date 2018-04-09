using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [SerializeField]
    public PlayerWeapon primaryWeapon;

    [SerializeField]
    public PlayerWeapon currentWeapon;

    [Header("Weapon settings")]
    [SerializeField]
    private GameObject impactEffect;

    [SerializeField]
    private Transform weaponHolder;

    private void Start()
    {
        EquipWeapon(primaryWeapon);

    }
    void EquipWeapon(PlayerWeapon weapon)
    {
        currentWeapon = weapon;
        GameObject weapObj = Instantiate(weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        weapObj.transform.SetParent(weaponHolder);
    }
    public GameObject GetImpactEffect()
    {
        return impactEffect;
    }
    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

}
