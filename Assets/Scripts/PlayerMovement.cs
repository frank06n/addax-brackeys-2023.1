using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterScript
{
    private List<PickupLogic> pickupsAround;
    private bool furnaceAround;

    protected override void Awake()
    {
        base.Awake();
        pickupsAround = new List<PickupLogic>();
        furnaceAround = false;
    }

    protected override void OnUpdate()
    {
        if (GetWeapon() != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GetWeapon().LookTowards(mousePosition, false);

            if (Input.GetMouseButtonDown(0)) GetWeapon().Attack();
        }

        PickupOrThrow();
        if (Input.GetKeyDown(KeyCode.LeftShift)) LevelManager.instance.ToggleObjectivesPanel();
        if (Input.GetKeyDown(KeyCode.Space) && furnaceAround)
            LevelManager.instance.FurnaceInteract();
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
    protected override void OnWeaponPause(){}

    private void PickupOrThrow()
    {
        int i = 0;
        if (Input.GetKeyDown(KeyCode.E)) i = +1;
        else if (Input.GetKeyDown(KeyCode.F)) i = -1;

        if (LevelManager.instance.IsReverse()) i *= -1;

        if (i == 1) PickupObject();
        else if (i == -1) ThrowHeld();
    }

    public void OnFurnaceProcessComplete()
    {
        
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        LevelManager.instance.SetPlayerHealthFill(health / maxHealth);
    }

    protected override void OnDeath() { }

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
        if (collision.CompareTag("Furnace"))
        {
            furnaceAround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup"))
        {
            pickupsAround.Remove(collision.gameObject.GetComponent<PickupLogic>());
        }
        if (collision.CompareTag("Furnace"))
        {
            furnaceAround = false;
        }
    }
}

