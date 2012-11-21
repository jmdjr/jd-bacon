using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class JDEnemyCharacter : JDCharacter
{
    public JDEnemyCharacter()
    {
        this.Name = "Test Enemy";
        this.CollisionDamage = 1;
        this.HitPoints = 10;
    }
}
