﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class JDEnemy : JDCharacter
{
    public JDEnemy()
    {
        this.Name = "Test Enemy";
        this.CollisionDamage = 1;
        this.HitPoints = 10;
    }
}
