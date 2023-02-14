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

    private float initLocalY;
    private float hoverDelta;

    private bool hover;


    private void Start()
    {
        hover = true;
        initLocalY = transform.localPosition.y;
        hoverDelta = Random.Range(0f, 2 * Mathf.PI);
    }

    private void Update()
    {
        if (!hover) return;
        if (HoverYChange>0)
        {
            hoverDelta += HoverSpeed * Time.deltaTime;
            float change = Mathf.Sin(hoverDelta) * HoverYChange;
            transform.localPosition = new Vector3(transform.localPosition.x, initLocalY + change, 0);
        }
    }

    public bool isWeapon()
    {
        return PickupType == PType.WEAPON;
    }

    public void OnPlayerPicked(Transform newParent)
    {
        hover = false;
        transform.parent = newParent;
        transform.localPosition = Vector3.zero;
    }

    public void onPlayerDropped(Vector3 playerPos)
    {
        transform.parent = null; // pickups_holder
        transform.position = playerPos;
        initLocalY = transform.localPosition.y;
        hover = true;
        if (this.PickupType == PType.WEAPON) transform.rotation = Quaternion.identity;
    }
}
