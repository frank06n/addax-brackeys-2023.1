using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterScript
{
    private List<PickupLogic> pickupsAround;

    protected override void Awake()
    {
        base.Awake();
        pickupsAround = new List<PickupLogic>();
    }

    protected override void OnUpdate()
    {
        if (GetWeapon() != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GetWeapon().LookTowards(mousePosition, false);

            if (Input.GetMouseButtonDown(0)) GetWeapon().Attack();
            if (Input.GetMouseButtonDown(1)) GetWeapon().UnAttack();
        }

        if (Input.GetKeyDown(KeyCode.E)) PickupObject();
        if (Input.GetKeyDown(KeyCode.F)) ThrowHeld();
        if (Input.GetKeyDown(KeyCode.Space)) LevelManager.instance.ToggleObjectivesPanel();
    }
    protected override Vector2 GetBaseVelocity()
    {
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(KeyCode.A)) direction.x = -1;
        else if (Input.GetKey(KeyCode.D)) direction.x = +1;
        if (Input.GetKey(KeyCode.W)) direction.y = +1;
        else if (Input.GetKey(KeyCode.S)) direction.y = -1;
        return direction.normalized;
    }
    protected override void OnWeaponPause()
    {
        // do a camera effect
    }
    
    private void PickupObject()
    {
        ThrowHeld();
        if (pickupsAround.Count > 0) PickupObject(pickupsAround[0]);
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

