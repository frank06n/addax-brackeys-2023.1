using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public Transform gunPivot;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Transform holdPoint;

    private List<PickupLogic> pickupsAround;

    private PickupLogic heldObject;
    private GunLogic gun;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        holdPoint = transform.GetChild(0);

        pickupsAround = new List<PickupLogic>();
    }

    private void Update()
    {
        if (gun!=null && gun.unfiring)
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

        if (gun != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool inverted = sr.flipX;
            gun.LookTowards(mousePosition, false);

            if (Input.GetMouseButtonDown(0)) gun.FireBullet();
            if (Input.GetMouseButtonDown(1)) gun.UnFireBullet();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (this.heldObject != null)
            {
                PickupLogic droppedPk = this.heldObject;
                this.heldObject = null;
                if (droppedPk.isWeapon()) this.gun = null;
                droppedPk.onPlayerDropped(transform.position);
            }

            if (pickupsAround.Count > 0)
            {
                this.heldObject = pickupsAround[pickupsAround.Count - 1];
                pickupsAround.Remove(this.heldObject);

                this.heldObject.OnPlayerPicked(holdPoint);
                if (this.heldObject.isWeapon()) this.gun = this.heldObject.GetComponent<GunLogic>();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup"))
        {
            pickupsAround.Add(collision.gameObject.GetComponent<PickupLogic>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup"))
        {
            pickupsAround.Remove(collision.gameObject.GetComponent<PickupLogic>());
        }
    }
}

