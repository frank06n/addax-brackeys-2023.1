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
    public void Initialise(float damage, Vector2 direction, float speed, Collider2D shooter, Vector2 endPoint, Action onUnfireComplete, Collider2D unfireTarget)
    {
        this.damage = damage;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        this.shooter = shooter;
        this.onUnfireComplete = onUnfireComplete;

        unfired = true;
        ulife = Vector2.Distance(transform.position, endPoint) / speed;
        ResolveHit(unfireTarget, true);
    }

    public float GetLifetime()
    {
        return ulife;
    }

    // Update is called once per frame
    private void Update()
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
        ResolveHit(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ResolveHit(collider);
    }

    private void ResolveHit(Collider2D collider, bool forUnfire=false)
    {
        if (unfired != forUnfire) return;
        if (collider.CompareTag(shooter.tag)) return;

        if (collider.gameObject.layer == LevelManager.instance.LAYER_VULNERABLE)
        {
            collider.GetComponentInParent<CharacterScript>().TakeDamage(damage);
        }
        if (!forUnfire) Destroy(gameObject);
    }
}
