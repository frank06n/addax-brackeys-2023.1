using System;
using UnityEngine;
using TMPro;

public class AmmoDisplayHandler : MonoBehaviour
{
    private TextMeshProUGUI textbox;
    private int ammo, maxAmmo;
    private GunLogic gun;

    private void Awake()
    {
        textbox = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        textbox.text = $"{ammo} / {maxAmmo}";
    }

    public void SetGun(GunLogic gun)
    {
        if (this.gun != null && this.gun != gun)
        {
            this.gun.OnAmmoUpdate = null;
        }
        this.gun = gun;
        this.gun.OnAmmoUpdate = () =>
        {
            this.ammo = this.gun.GetAmmo();
            this.maxAmmo = this.gun.GetMaxAmmo();
        };
        this.gun.OnAmmoUpdate();
    }
}
