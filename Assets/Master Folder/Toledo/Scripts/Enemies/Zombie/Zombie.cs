﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy {

    public Zombie() {

        Name = "Zombie";
        HP = 100;
        MaxHP = 100;
        CombatSpeed = 2f;
        CombaAnimSpeed = 0.8f;
        PatrolSpeed = 1.5f;
        PatrolAnimSpeed = 0.9f;

    }

}
