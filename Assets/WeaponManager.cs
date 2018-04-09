using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon settings")]
    [SerializeField]
    public PlayerWeapon primaryWeapon;
    [SerializeField]
    public PlayerWeapon secondaryWeapon;
    [SerializeField]
    private GameObject impactEffect;
    [SerializeField]
    private Transform weaponHolder;

    private GameObject currentWeaponGfxInstance;

    [SerializeField]
    public PlayerWeapon currentWeapon;

    private void Start()
    {
        EquipWeapon(primaryWeapon);

    }
    public void TogglePrimarySecondary()
    {
        if (currentWeapon == primaryWeapon)
        {
            EquipWeapon(secondaryWeapon);
        }
        else
        {
            EquipWeapon(primaryWeapon);
        }
    }

    void EquipWeapon(PlayerWeapon weapon)
    {
        if (weaponHolder.childCount > 0)
        {
            Destroy(weaponHolder.GetChild(0).gameObject);
        }

        currentWeapon = weapon;
        currentWeaponGfxInstance = Instantiate(weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        currentWeaponGfxInstance.transform.SetParent(weaponHolder);
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
