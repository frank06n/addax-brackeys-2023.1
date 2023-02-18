using System.Collections;
using UnityEngine;

public enum WeaponType
{
    MELEE, RANGED
} 

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
    [SerializeField] private string attackSfx, unAttackSfx;

    public void Attack()
    {
        if (LevelManager.instance.IsReverse())
            _UnAttack();
        else
        {
            _Attack();
            LevelManager.instance.audioPlayer.Play(attackSfx);
        }
    }
    protected abstract void _Attack();
    protected abstract void _UnAttack();
    public abstract WeaponType GetWeaponType();

    public virtual bool RequiresPause() { return false; }
    protected virtual void OnHolderChanged(Collider2D old, Collider2D newt) { }

    protected IEnumerator PlayUnAttackSfx(float ulife)
    {
        float sfxTime = LevelManager.instance.audioPlayer.Length(unAttackSfx);
        //Debug.Log("play unattacksfx after " + (ulife - sfxTime));
        yield return new WaitForSeconds(ulife - sfxTime);
        LevelManager.instance.audioPlayer.Play(unAttackSfx);
    }

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
