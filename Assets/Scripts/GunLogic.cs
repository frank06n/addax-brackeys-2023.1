using UnityEngine;

public class GunLogic : WeaponLogic
{
    public float BulletDamage;
    public float BulletSpeed;

    public GameObject BulletPrefab;

    [SerializeField] protected string BulletFireSfx;

    private Transform BulletPoint;
    private bool unfiring;

    private void Awake()
    {
        BulletPoint = transform.GetChild(1);
    }

    protected virtual void OnAwake() { }

    public override void Attack()
    {
        Vector2 speed = (BulletPoint.position - transform.position).normalized * BulletSpeed;
        GameObject bulletObj = Instantiate(BulletPrefab, BulletPoint.position, transform.rotation, transform);
        bulletObj.transform.parent = null;// LevelManager.instance.BulletsHolder;

        BulletLogic bullet = bulletObj.GetComponent<BulletLogic>();
        bullet.Initialise(BulletDamage, speed, holder);

        //SceneManager2.instance.sfxPlayer.Play(BulletFireSfx);
    }
    public override void UnAttack()
    {
        Vector2 direction = (BulletPoint.position - transform.position).normalized;
        //RaycastHit2D pt = Physics2D.Raycast(BulletPoint.position, direction, 50, LevelManager.instance.raycastLayers, -10, 0);
        RaycastHit2D pt = LevelManager.instance.UnFireRaycast(BulletPoint.position, direction, holder.tag);

        GameObject bulletObj = Instantiate(BulletPrefab, pt.point, transform.rotation, transform);
        bulletObj.transform.parent = null;// LevelManager.instance.BulletsHolder;

        BulletLogic bullet = bulletObj.GetComponent<BulletLogic>();
        bullet.Initialise(BulletDamage, -direction, BulletSpeed, holder, BulletPoint.position, () =>
        {
            this.unfiring = false;
        });
        this.unfiring = true;

        //SceneManager2.instance.sfxPlayer.Play(BulletFireSfx);
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