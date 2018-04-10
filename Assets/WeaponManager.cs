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

    private WeaponGraphics currentGraphics;

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
        GameObject o = Instantiate(weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        o.transform.SetParent(weaponHolder);

        currentGraphics = o.GetComponent<WeaponGraphics>();
        if (!currentGraphics)
        {
            Debug.LogError("No WeaponGraphics script on the current weapon " + o.name);
        }
    }

    public GameObject GetImpactEffect()
    {
        return impactEffect;
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

}
