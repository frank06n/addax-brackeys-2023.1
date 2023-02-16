using UnityEngine;

public abstract class WeaponLogic : MonoBehaviour
{
    private Collider2D _holder; 
    public Collider2D holder
    {
        get { return _holder; }
        set
        {
            OnHolderChanged(_holder, value);
            _holder = value;
        }
    }
    
    public abstract void Attack();
    public abstract void UnAttack();

    public virtual bool RequiresPause() { return false; }
    protected virtual void OnHolderChanged(Collider2D old, Collider2D newt) { }

    public void LookTowards(Vector2 position, bool inverted = false)
    {
        float lookAngle = Vector2.SignedAngle(Vector2.right, position - (Vector2)transform.position);
        if (inverted) lookAngle += 180;

        LookTowards(lookAngle);
    }

    private void LookTowards(float lookAngle)
    {
        if (lookAngle > 180) lookAngle -= 360;
        if (lookAngle < -180) lookAngle += 360;
        transform.rotation = Quaternion.Euler(0, 0, lookAngle);

        float scaleY = (Mathf.Abs(lookAngle) <= 90) ? +1 : -1;
        transform.localScale = new Vector3(1, scaleY, 1);
    }

}
