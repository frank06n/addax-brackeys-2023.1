using UnityEngine;

public abstract class CharacterScript : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float maxHealth;
    protected float health;

    [SerializeField] private Sprite spr_Dead;
    [SerializeField] private Sprite spr_Alive;


    [SerializeField] private string sfx_Hit;
    [SerializeField] private string rsfx_Hit;
    [SerializeField] private string sfx_Death;
    [SerializeField] private string rsfx_Death;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private new Collider2D collider;

    private Transform holdPoint;

    private PickupLogic heldObject;
    private WeaponLogic weapon;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = transform.GetChild(1).GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        holdPoint = transform.GetChild(0);

        health = maxHealth;
    }

    protected abstract Vector2 GetBaseVelocity();
    protected abstract void OnWeaponPause();
    protected abstract void OnUpdate();

    protected virtual void OnDeath() {
        sr.sprite = spr_Dead;
    }
    protected virtual void OnUnkillDone()
    {
        sr.sprite = spr_Alive;
    }

    public virtual void TakeDamage(float damage)
    {
        string sfxToPlay = "";
        if (!LevelManager.instance.IsReverse())
        {
            sfxToPlay = sfx_Hit;
            if (health != 0 && (health -= damage) <= 0)
            {
                sfxToPlay = sfx_Death;
                OnDeath();
            }
        }
        else
        {
            sfxToPlay = rsfx_Hit;
            if (health == 0) sfxToPlay = rsfx_Death;
            if (health != maxHealth && (health += damage) >= maxHealth)
            {
                OnUnkillDone();
            }
        }
        health = Mathf.Clamp(health, 0, maxHealth);
        LevelManager.instance.audioPlayer.Play(sfxToPlay);
    }

    private void Update()
    {
        if (weapon != null && weapon.RequiresPause())
        {
            rb.velocity = Vector2.zero;
            OnWeaponPause();
            return;
        }

        Vector2 baseVel = GetBaseVelocity();
        rb.velocity = baseVel * speed;

        if (baseVel.x != 0) sr.flipX = baseVel.x < 0;

        OnUpdate();
    }

    protected PickupLogic GetHeldObject()
    {
        return this.heldObject;
    }
    public WeaponLogic GetWeapon()
    {
        return this.weapon;
    }

    protected void ThrowHeld()
    {
        if (this.heldObject == null) return;

        PickupLogic droppedPk = heldObject;
        heldObject = null;
        if (droppedPk.isWeapon())
        {
            weapon.holder = null;
            weapon = null;
        }
        droppedPk.OnDropped(transform.position);
    }
    protected void PickupObject(PickupLogic newObj)
    {
        if (this.heldObject != null) ThrowHeld();

        heldObject = newObj;
        heldObject.OnPicked(holdPoint);

        if (heldObject.isWeapon())
        {
            weapon = heldObject.GetComponent<WeaponLogic>();
            weapon.holder = collider;
        }
    }
}
