using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BuyWeaponButton : JDMonoGuiBehavior
{
    JDWeapon weaponReference = null;
    LevelManager level = null;
    GameStatistics stats = null;

    public override void Awake()
    {
        base.Awake();

        level = LevelManager.Instance;
        stats = GameStatistics.Instance;
    }

    public void SetWeapon(BuyWeaponButton weaponReference)
    {
        this.weaponReference = weaponReference.weaponReference;
    }

    public void PurchaseWeapon(JDWeapon weapon)
    {
    }
}
