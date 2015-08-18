using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Base entity for all sprites in the game, contains the basic
/// Information needed by every character
/// </summary>
public class Character : MonoBehaviour {

    // Current Health of the Character
    protected int mHealth;
    public int Health { 
        get { return mHealth; }
        set { mHealth = value; }
    }

    // Max Health of the character
    protected int mMaxHealth;
    public int MaxHealth { 
        get { return mMaxHealth; }
        set { mMaxHealth = value; }
    }

    // Is the character dead
    protected bool isAlive = true;
    public bool IsAlive() { return isAlive; }

    // Can the Character be hurt
    protected bool isInvincible = false;
    public bool IsInvincible { get { return isInvincible; } }

    /// <summary>
    /// Function that sets isAlive immediately to false,
    /// virtual for adding death animations or such
    /// </summary>
    public virtual void Dead()
    {
        isAlive = false;    
    }

    /// <summary>
    /// Adds or subtracts health to the character
    /// </summary>
    /// <param name="variable"> integer representing amount health witll change</param>
    public virtual void ChangeCurrentHealth(int variable)
    {
        mHealth = Math.Min(mHealth + variable, mHealth);
        if (mHealth <= 0)
        {
            mHealth = 0;
            isAlive = false;
        }
    }

    /// <summary>
    /// Same as Change Current Health specific for healing, useful if animation
    /// Is tied to the function
    /// </summary>
    /// <param name="variable">amount of health to heal</param>
    public virtual void HealDamage(int variable)
    {
        mHealth = Math.Min(mHealth + variable, mHealth);
    }

    /// <summary>
    /// Same as Change Current Health specific for healing, useful if animation
    /// Is tied to the function
    /// </summary>
    /// <param name="variable">amount of health to heal</param>
    public virtual void TakeDamage(int variable)
    {
        mHealth = Math.Max(mHealth - variable, mHealth);
        if (mHealth <= 0)
        {
            mHealth = 0;
            isAlive = false;
        }
    }

    /// <summary>
    /// Funciton called when creature dies, Animation helpful
    /// </summary>
    public virtual void Die(){
        Destroy(gameObject);
    }
}
