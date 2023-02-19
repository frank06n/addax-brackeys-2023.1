using System;
using UnityEngine;

public class GunLogic : WeaponLogic
{
    public float BulletDamage;
    public float BulletSpeed;

    private int Ammo;
    [SerializeField] private int MaxAmmo;

    public Action<int, int> OnAmmoUpdate;
    

    public GameObject BulletPrefab;

    private Transform BulletPoint;
    private bool unfiring;

    private void Awake()
    {
        BulletPoint = transform.GetChild(1);
        Ammo = 0;
    }

    protected override void _Attack()
    {
        if (MaxAmmo != -1 && Ammo==0)
        {
            LevelManager.instance.ShowHint("Cant Shoot! No Ammo!");
            // empty gun sound
            return;
        }

        Vector2 speed = (BulletPoint.position - transform.position).normalized * BulletSpeed;
        GameObject bulletObj = Instantiate(BulletPrefab, BulletPoint.position, transform.rotation, transform);
        bulletObj.transform.parent = LevelManager.instance.bulletsHolder;

        BulletLogic bullet = bulletObj.GetComponent<BulletLogic>();
        bullet.Initialise(BulletDamage, speed, holder);

        if (OnAmmoUpdate != null) OnAmmoUpdate(--Ammo, MaxAmmo);
    }
    protected override void _UnAttack()
    {
        if (MaxAmmo != -1 && Ammo == MaxAmmo)
        {
            LevelManager.instance.ShowHint("Your gun is full!");
            // full gun sound
            return;
        }

        Vector2 direction = (BulletPoint.position - transform.position).normalized;
        RaycastHit2D pt = LevelManager.instance.UnFireRaycast(BulletPoint.position, direction, holder.tag);

        GameObject bulletObj = Instantiate(BulletPrefab, pt.point, transform.rotation, transform);
        bulletObj.transform.parent = LevelManager.instance.bulletsHolder;

        BulletLogic bullet = bulletObj.GetComponent<BulletLogic>();
        bullet.Initialise(BulletDamage, -direction, BulletSpeed, holder, BulletPoint.position, () => unfiring = false, pt.collider);
        unfiring = true;
        StartCoroutine(PlayUnAttackSfx(bullet.GetLifetime()));

        if (OnAmmoUpdate != null) OnAmmoUpdate(++Ammo, MaxAmmo);
    }

    public override bool RequiresPause()
    {
        return unfiring;
    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.RANGED;
    }
}