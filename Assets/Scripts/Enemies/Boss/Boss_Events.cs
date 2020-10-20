﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Events : MonoBehaviour
{
    protected Boss Boss;
    [SerializeField] Collider Alabarda;

    void Start()
    {
        Boss = GetComponentInParent<Boss>();
    }

    public void TurnCollider(int _Val)
    {
        Alabarda.enabled = _Val == 0 ? false : true;
    }

    public void WalkAttack()
    {
        Boss.WalkOnAttack();
    }
}