using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private new Collider2D collider;

    private Transform holdPoint;

    private List<PickupLogic> pickupsAround;

    private PickupLogic heldObject;
    private WeaponLogic weapon;

    // Animator
    private Animator animator;

    // Boolean for checking if player moves
    private bool isMoving;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        holdPoint = transform.GetChild(0);

        pickupsAround = new List<PickupLogic>();

        animator=GetComponent<Animator>();
        isMoving = false;
    }

    private void Update()
    {
        isMoving = false;
        if (weapon!=null && weapon.RequiresPause())
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.A))  {
            direction.x = -1;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D)) {
            direction.x = +1;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.W)) {
            direction.y = +1;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S)) {
            direction.y = -1;
            isMoving = true;
        }
        rb.velocity = direction.normalized * speed;

        if (isMoving) animator.SetFloat("Speed", 1);
        else animator.SetFloat("Speed", 0);

        if (direction.x != 0) sr.flipX = direction.x == -1;

        if (weapon != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            weapon.LookTowards(mousePosition, false);

            if (Input.GetMouseButtonDown(0)) weapon.Attack();
            if (Input.GetMouseButtonDown(1)) weapon.UnAttack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (this.heldObject != null)
            {
                PickupLogic droppedPk = heldObject;
                heldObject = null;
                if (droppedPk.isWeapon())
                {
                    weapon.holder = null;
                    weapon = null;
                }
                droppedPk.onDropped(transform.position);
            }

            if (pickupsAround.Count > 0)
            {
                heldObject = pickupsAround[pickupsAround.Count - 1];
                pickupsAround.Remove(heldObject);

                heldObject.OnPicked(holdPoint);
                if (heldObject.isWeapon())
                {
                    weapon = heldObject.GetComponent<WeaponLogic>();
                    weapon.holder = collider;
                }
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

