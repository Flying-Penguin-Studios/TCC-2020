using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Events : MonoBehaviour
{
    protected Boss Boss;
    [SerializeField] BoxCollider Alabarda;
    [SerializeField] CapsuleCollider DashColl;

    void Start()
    {
        Boss = GetComponentInParent<Boss>();
    }

    public void TurnCollider(int _Val)
    {
        Alabarda.enabled = _Val == 0 ? false : true;
    }

    public void DashCollider(int _Val)
    {
        DashColl.enabled = _Val == 0 ? false : true;
    }

    public void WalkAttack()
    {
        Boss.WalkOnAttack();
    }

    public void Dash()
    {
        //StartCoroutine(Boss.Dash());
    }

    public void UnlockAttack()
    {
        Boss.UnlockAttack();
    }

    public void SetAttacks()
    {
        Boss.SetBool("CanAttack", true);
    }
}
