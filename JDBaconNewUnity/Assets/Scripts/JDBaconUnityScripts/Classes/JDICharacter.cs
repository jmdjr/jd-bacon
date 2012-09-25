
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using SmoothMoves;

using Object = UnityEngine.Object;

public interface JDICharacter : JDIObject, JDIAmCollidable
{
    JDIAnimator Animator {get;set;}     // Animator related to this Character
    int MaxHitPoints { get; set; }      // Max amount of HP if fully recovered
    int HitPoints { get; set; }         // Current anount of HP
    int CollisionDamage { get; set; }   // Amount of damage a body check would generate

    void TakeDamage(int damage);        // Function for inflicting damage to Character
    int InflictingDamage();             // Calculates amount of damage to inflict

    Event WasHitWithWeapon(JDICharacter other, JDIWeapon weapon);
}