﻿using System.Collections;
using UnityEngine;

public class KnifeLogic : WeaponLogic
{
    public float attackDamage;
    public float attackSpeed;
    public AnimationCurve attackAnimCurve;

    private Vector3 initPos;
    private bool attacking;
    private KnifeAttackPointLogic attackPoint;


    private void Awake()
    {
        attacking = false;
        attackPoint = GetComponentInChildren<KnifeAttackPointLogic>();
        attackPoint.gameObject.SetActive(false);
    }

    protected override void _Attack()
    {
        if (attacking) return;
        StartCoroutine(AttackAnim());
    }
    protected override void _UnAttack()
    {
        _Attack();
    }
    private IEnumerator AttackAnim()
    {
        Collider2D mholder = holder;
        initPos = transform.localPosition;
        attackPoint.SetEnabled(mholder, true);
        this.attacking = true;


        float animProg = 0;
        while (holder != null && animProg <= 1)
        {
            float angle = Mathf.Deg2Rad * transform.localRotation.eulerAngles.z;
            Vector3 displacement = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 0.05f;
            transform.localPosition = initPos + displacement*attackAnimCurve.Evaluate(animProg);
            animProg += attackSpeed * Time.deltaTime;
            yield return null;
        }

        if (holder!=null) transform.localPosition = initPos;
        attackPoint.SetEnabled(mholder, false);
        this.attacking = false;
    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.MELEE;
    }
}