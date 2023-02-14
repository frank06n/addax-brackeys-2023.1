using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    private float damage;
    private string shooterTag;

    private bool unfired;
    private Action onUnfireComplete;
    private float ulife;

    public void Initialise(float damage, Vector2 velocity, string shooterTag)
    {
        this.damage = damage;
        GetComponent<Rigidbody2D>().velocity = velocity;
        this.shooterTag = shooterTag;

        unfired = false;
    }
    public void Initialise(float damage, Vector2 direction, float speed, string shooterTag, Vector2 endPoint, Action onUnfireComplete)
    {
        this.damage = damage;
        GetComponent<Rigidbody2D>().velocity = direction*speed;
        this.shooterTag = shooterTag;
        this.onUnfireComplete = onUnfireComplete;

        unfired = true;
        ulife = Vector2.Distance(transform.position, endPoint) / speed;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag(shooterTag).GetComponent<Collider2D>());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!unfired) return;
        ulife -= Time.deltaTime;
        if (ulife <= 0)
        {
            Destroy(gameObject);
            if (onUnfireComplete != null) onUnfireComplete();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (unfired || collision.collider.CompareTag(shooterTag)) return;

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
