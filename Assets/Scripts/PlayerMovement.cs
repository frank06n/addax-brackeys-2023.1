using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterScript
{
    private List<PickupLogic> pickupsAround;
    private bool furnaceAround;
    private bool atDoor;

    protected override void Awake()
    {
        base.Awake();
        pickupsAround = new List<PickupLogic>();
        furnaceAround = false;
    }


    private void Start()
    {
        TakeDamage(maxHealth - 10);
        LevelManager.instance.SetPlayerHealthFill(0);
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
        if (Input.GetKeyDown(KeyCode.E)) PickupObject();
        else if (Input.GetKeyDown(KeyCode.F)) ThrowHeld();
    }

    public void OnFurnaceProcessComplete()
    {
        
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        LevelManager.instance.SetPlayerHealthFill(health / maxHealth);
    }

    protected override void OnDeath() {

        LevelManager.instance.GotoMainMenu();
    }

    private void PickupObject()
    {
        ThrowHeld();
        if (pickupsAround.Count > 0) PickupObject(pickupsAround[0]);
        bool showAmmo = false;
        if (GetWeapon() != null && GetWeapon().GetWeaponType() == WeaponType.RANGED) 
        {
            showAmmo = true;
            LevelManager.instance.SetAmmoGun((GunLogic)GetWeapon());
        }
        LevelManager.instance.ShowAmmo(showAmmo);
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
        if (collision.CompareTag("Door"))
        {
            atDoor = true;
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
        if (collision.CompareTag("Door"))
        {
            atDoor = false;
        }
    }
}

