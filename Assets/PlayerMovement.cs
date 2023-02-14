using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public Transform gunPivot;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public GunLogic gun;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (gun.unfiring)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.A)) direction.x = -1;
        else if (Input.GetKey(KeyCode.D)) direction.x = +1;
        if (Input.GetKey(KeyCode.W)) direction.y = +1;
        else if (Input.GetKey(KeyCode.S)) direction.y = -1;
        rb.velocity = direction.normalized * speed;

        if (direction.x != 0) sr.flipX = direction.x == -1;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool inverted = sr.flipX;
        gun.LookTowards(mousePosition, false);

        if (Input.GetMouseButtonDown(0))
        {
            gun.FireBullet();
        }
        if (Input.GetMouseButtonDown(1))
        {
            gun.UnFireBullet();
        }
    }
}

