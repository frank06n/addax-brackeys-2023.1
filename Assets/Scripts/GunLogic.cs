using UnityEngine;

public class GunLogic : WeaponLogic
{
    public float BulletDamage;
    public float BulletSpeed;

    public GameObject BulletPrefab;

    private Transform BulletPoint;
    private bool unfiring;

    private void Awake()
    {
        BulletPoint = transform.GetChild(1);
    }

    protected override void _Attack()
    {
        Vector2 speed = (BulletPoint.position - transform.position).normalized * BulletSpeed;
        GameObject bulletObj = Instantiate(BulletPrefab, BulletPoint.position, transform.rotation, transform);
        bulletObj.transform.parent = LevelManager.instance.bulletsHolder;

        BulletLogic bullet = bulletObj.GetComponent<BulletLogic>();
        bullet.Initialise(BulletDamage, speed, holder);
    }
    protected override void _UnAttack()
    {
        Vector2 direction = (BulletPoint.position - transform.position).normalized;
        RaycastHit2D pt = LevelManager.instance.UnFireRaycast(BulletPoint.position, direction, holder.tag);

        GameObject bulletObj = Instantiate(BulletPrefab, pt.point, transform.rotation, transform);
        bulletObj.transform.parent = LevelManager.instance.bulletsHolder;

        BulletLogic bullet = bulletObj.GetComponent<BulletLogic>();
        bullet.Initialise(BulletDamage, -direction, BulletSpeed, holder, BulletPoint.position, () => unfiring = false, pt.collider);
        unfiring = true;
        StartCoroutine(PlayUnAttackSfx(bullet.GetLifetime()));
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