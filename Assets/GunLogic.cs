using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLogic : MonoBehaviour
{
    public float BulletDamage;
    public float BulletSpeed;
    public GameObject BulletPrefab;

    //[SerializeField] protected string BulletFireSfx;

    private Transform BulletPoint;
    public bool unfiring;

    private void Awake()
    {
        BulletPoint = transform.GetChild(1);
    }

    protected virtual void OnAwake() { }

    public void FireBullet()
    {
        Vector2 speed = (BulletPoint.position - transform.position).normalized * BulletSpeed;
        GameObject bulletObj = Instantiate(BulletPrefab, BulletPoint.position, transform.rotation, transform);
        bulletObj.transform.parent = null;// LevelManager.instance.BulletsHolder;

        BulletLogic bullet = bulletObj.GetComponent<BulletLogic>();
        bullet.Initialise(BulletDamage, speed, "Player");

        Debug.Log(BulletPoint.position);

        //SceneManager2.instance.sfxPlayer.Play(BulletFireSfx);
    }
    public void UnFireBullet()
    {
        Vector2 direction = (BulletPoint.position - transform.position).normalized;
        RaycastHit2D pt = Physics2D.Raycast(BulletPoint.position, direction, 50, Physics2D.AllLayers, -10, 0);
        
        Vector2 speed = -direction * BulletSpeed;
        GameObject bulletObj = Instantiate(BulletPrefab, pt.point, transform.rotation, transform);
        bulletObj.transform.parent = null;// LevelManager.instance.BulletsHolder;

        BulletLogic bullet = bulletObj.GetComponent<BulletLogic>();
        bullet.Initialise(BulletDamage, speed, "Player", true, BulletPoint.position, () =>
        {
            this.unfiring = false;
        });
        this.unfiring = true;

        //SceneManager2.instance.sfxPlayer.Play(BulletFireSfx);
    }

    public void LookTowards(Vector2 position, bool inverted = false)
    {
        float lookAngle = Vector2.SignedAngle(Vector2.right, position - (Vector2)transform.position);
        if (inverted) lookAngle += 180;

        LookTowards(lookAngle);
    }

    protected void LookTowards(float lookAngle)
    {
        if (lookAngle > 180) lookAngle -= 360;
        if (lookAngle < -180) lookAngle += 360;
        transform.rotation = Quaternion.Euler(0, 0, lookAngle);

        float scaleY = (Mathf.Abs(lookAngle) <= 90) ? +1 : -1;
        transform.localScale = new Vector3(1, scaleY, 1);
    }
}