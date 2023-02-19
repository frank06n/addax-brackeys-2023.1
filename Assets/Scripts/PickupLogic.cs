using UnityEngine;

public class PickupLogic : MonoBehaviour
{   
    public enum PType
    {
        WEAPON, MAP, SC_METAL, SC_WOOD, KEY, TRASH
    };

    [SerializeField] private PType PickupType;

    private Collider2D proximityCollider;
    private Vector2 initLocalScale;

    private SpriteRenderer sr;


    private void Awake()
    {
        initLocalScale = transform.localScale;

        proximityCollider = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
    }

    public bool isWeapon()
    {
        return PickupType == PType.WEAPON;
    }

    public void OnPicked(Transform newParent)
    {
        transform.parent = newParent;
        transform.localPosition = Vector3.zero;
        proximityCollider.enabled = false;
        this.sr.sortingLayerName = "Held";
        LevelManager.instance.audioPlayer.Play("sfx_item_pick");
    }

    public void OnDropped(Vector3 position)
    {
        transform.parent = LevelManager.instance.pickupsHolder;
        transform.position = position;
        transform.localScale = initLocalScale;
        proximityCollider.enabled = true;
        if (this.PickupType == PType.WEAPON) transform.rotation = Quaternion.identity;
        this.sr.sortingLayerName = "Entities";
        LevelManager.instance.audioPlayer.Play("sfx_item_drop");
    }
}
