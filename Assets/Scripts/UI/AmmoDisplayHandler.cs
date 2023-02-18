using System;
using UnityEngine;
using TMPro;

public class AmmoDisplayHandler : MonoBehaviour
{
    private TextMeshProUGUI textbox;
    private GunLogic gun;
    private int ammo, maxAmmo;

    private void Awake()
    {
        textbox = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textbox.text = $"{ammo} / {maxAmmo}";
    }

    public void SetGun(GunLogic gun)
    {
        if (this.gun != null && this.gun != gun) throw new InvalidOperationException();
        if (this.gun == gun) return;
        this.gun = gun;
        this.gun.OnAmmoUpdate = (ammo, maxAmmo) =>
        {
            this.ammo = ammo;
            this.maxAmmo = maxAmmo;
        };
    }
}
