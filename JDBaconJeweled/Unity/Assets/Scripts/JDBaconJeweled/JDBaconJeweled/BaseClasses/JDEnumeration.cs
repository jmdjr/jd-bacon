public enum JDIObjectTypes
{
    OBJECT = 0,
    CHARACTER,
    WEAPON,
    COLLECTABLE,
    SCRIPT
}
public enum TagTypes
{
    UNTAGGED,
    PLAYER,
    ENEMY,
    EVENTTRIGGER,
    LEVELTERRAIN,
    COLLECTABLE
}

public enum JDIStatTypes
{
    Level1_Progress,
    Level1_EnemiesKilled,
    Level1_JDCoinsCollected,
    Level2_Progress,
    Level2_EnemiesKilled,
    Level2_JDCoinsCollected,
    Level3_Progress,
    Level3_EnemiesKilled,
    Level3_JDCoinsCollected,
    Level4_Progress,
    Level4_EnemiesKilled,
    Level4_JDCoinsCollected
}

// Character animations will be stored as a set of 3 flagged bit ranges.  each range represents a collection of unique animations
//
// | Standard Animations |   Directions   |      Weapons     |
// |   0000 0000  0000   |   0000  0000   |  0000 0000 0000  |
//
// This allows for a maximum of ~62 unique animations, for ~14 directions, utilizing ~62 different weapons.
//  if these quantities need to be extended, a 32 bit int will be used instead, in which each of the three flag regions will be
//  scaled appropriately.
public enum HeroAnimationType
{
    // Standard Animations
    S_NONE = 0,
    S_IDLE = 1,
    S_WALK = 2,
    S_JUMP = 3,

    MASK_STANDARD = 4095,

    // Directions
    D_STRAIT = 1 << 12,
    D_DIAGONAL_UP = 2 << 12,
    D_UP = 3 << 12,
    D_DIAGONAL_DOWN = 4 << 12,
    D_DOWN = 5 << 12,

    MASK_DIRECTION = 255 << 12,

    // Weapons
    W_NONE = 1 << 20,
    W_SWORD_IDLE = 2 << 20,
    W_SWORD_ATTACK = 3 << 20,
    W_SHOTGUN_IDLE = 4 << 20,
    W_SHOTGUN_ATTACK = 5 << 20,
    W_GRENADE_IDLE = 6 << 20,
    W_GRENADE_ATTACK = 7 << 20,
    W_BACONAISSE_IDLE = 8 << 20,
    W_BACONAISSE_ATTACK = 9 << 20,

    MASK_WEAPON = 4095 << 20,
}
