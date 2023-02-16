using System;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    private float damage;
    private Collider2D shooter;

    private bool unfired;
    private Action onUnfireComplete;
    private float ulife;

    public void Initialise(float damage, Vector2 velocity, Collider2D shooter)
    {
        this.damage = damage;
        GetComponent<Rigidbody2D>().velocity = velocity;
        this.shooter = shooter;

        unfired = false;
    }
    public void Initialise(float damage, Vector2 direction, float speed, Collider2D shooter, Vector2 endPoint, Action onUnfireComplete)
    {
        this.damage = damage;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        this.shooter = shooter;
        this.onUnfireComplete = onUnfireComplete;

        unfired = true;
        ulife = Vector2.Distance(transform.position, endPoint) / speed;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), shooter);

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
        if (unfired || collision.collider.CompareTag(shooter.tag)) return;
        Debug.Log("Bullet hit: " + collision.collider.tag);

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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (unfired || collider.CompareTag(shooter.tag)) return;
        Debug.Log("Bullet hit: " + collider.tag);
        Destroy(gameObject);
    }
}
