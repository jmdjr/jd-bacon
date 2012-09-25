using UnityEngine;
using System;
using System.Collections;

using Object = UnityEngine.Object;
using System.Collections.Generic;

public class WeaponSystem
{
    public interface IWeapon
    {
        string Name { get; set; }
        HeroAnimationType AnimationType { get; set; }
        int DamageAmount { get; set; }
        bool IsActive { get; set; }
        int CooldownTime { get; set; }
    }

    public class Sword : IWeapon
    {
        private string name = "Sword";
        private HeroAnimationType animationType = HeroAnimationType.W_SWORD;
        private int damageAmount = 1;
        private bool isActive = true;
        private int cooldownTime = 0;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public HeroAnimationType AnimationType
        {
            get
            {
                return animationType;
            }
            set
            {
                animationType = value;
            }
        }

        public int DamageAmount
        {
            get
            {
                return damageAmount;
            }
            set
            {
                damageAmount = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
            }
        }

        public int CooldownTime
        {
            get
            {
                return cooldownTime;
            }
            set
            {
                cooldownTime = value;
            }
        }
    }
}
