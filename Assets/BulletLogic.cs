using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    private float damage;
    private string shooterTag;

    private bool unfired;
    private Vector2 endPoint;
    private Action onUnfireComplete;

    public void Initialise(float damage, Vector2 speed, string shooterTag)
    {
        Initialise(damage, speed, shooterTag, false, Vector2.zero, null);
    }
    public void Initialise(float damage, Vector2 speed, string shooterTag, bool unfired, Vector2 endPoint, Action onUnfireComplete)
    {
        this.damage = damage;
        GetComponent<Rigidbody2D>().velocity = speed;
        this.shooterTag = shooterTag;
        this.unfired = unfired;
        this.endPoint = endPoint;
        this.onUnfireComplete = onUnfireComplete;
    }

    // Update is called once per frame
    void Update()
    {
        if (!unfired) return;
        if (Vector2.Distance(transform.position, endPoint) <= 0.2)
        {
            Destroy(gameObject);
            if (onUnfireComplete != null) onUnfireComplete();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (unfired || collision.collider.CompareTag(shooterTag)) return;

        Debug.Log("Bullet Hit: " + collision.collider.name);
        Destroy(gameObject);

        /*if (collision.collider.CompareTag("Player"))
        {
            LevelManager.instance.player.Damage(damage);
        }
        else if (collision.collider.CompareTag("EnemyTurret"))
        {
            collision.collider.GetComponent<EnemyTurretLogic>().Damage(damage);
        }
        else
        {
            // on hitting wall
            SceneManager2.instance.sfxPlayer.Play("bullet_hit_platform");
        }*/
    }
}
