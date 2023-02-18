using UnityEngine;

public class KnifeAttackPointLogic : MonoBehaviour
{
    private string holderTag;
    private new Collider2D collider;
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public void SetEnabled(Collider2D collider, bool enabled)
    {
        holderTag = enabled ? collider.tag : "";
        gameObject.SetActive(enabled);
        Physics2D.IgnoreCollision(this.collider, collider, enabled);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(holderTag)) return;
        if (collision.gameObject.layer == LevelManager.instance.LAYER_VULNERABLE)
        {
            collision.GetComponentInParent<CharacterScript>().TakeDamage(10);
        }
        //Debug.Log("Knife hit: " + collision.transform.parent.name);
    }
}
