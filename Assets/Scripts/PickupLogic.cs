using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLogic : MonoBehaviour
{   
    public enum PType
    {
        MAP, WEAPON
    };

    [SerializeField] private PType PickupType;
    [SerializeField] private int Value;
    [SerializeField] private float HoverYChange;
    [SerializeField] private float HoverSpeed;

    private Collider2D proximityCollider;
    private Vector2 initLocalPos;
    private Vector2 initLocalScale;
    private float hoverDelta;

    private bool hover;


    private void Awake()
    {
        hover = true;
        hoverDelta = Random.Range(0f, 2 * Mathf.PI);

        initLocalPos = transform.localPosition;
        initLocalScale = transform.localScale;

        proximityCollider = GetComponent<Collider2D>();
    }

    //private void Start()
    //{
        
    //}

    private void Update()
    {
        if (!hover) return;
        if (HoverYChange>0)
        {
            hoverDelta += HoverSpeed * Time.deltaTime;
            float change = Mathf.Sin(hoverDelta) * HoverYChange;
            transform.localPosition = initLocalPos + Vector2.up * change;
        }
    }

    public bool isWeapon()
    {
        return PickupType == PType.WEAPON;
    }

    public void OnPicked(Transform newParent)
    {
        hover = false;
        transform.parent = newParent;
        transform.localPosition = Vector3.zero;
        proximityCollider.enabled = false;
    }

    public void onDropped(Vector3 position)
    {
        transform.parent = null; // pickups_holder
        transform.position = position;
        initLocalPos = transform.localPosition;
        transform.localScale = initLocalScale;
        hover = true;
        proximityCollider.enabled = true;
        if (this.PickupType == PType.WEAPON) transform.rotation = Quaternion.identity;
    }
}
