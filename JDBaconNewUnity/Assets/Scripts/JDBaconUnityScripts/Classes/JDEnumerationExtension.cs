
public static class HeroAnimationTypeExtension
{
    public static string TypeToDirectionalString(this HeroAnimationType type)
    {
        switch (type.TypeToDirection())
        {
            default:
            case HeroAnimationType.D_STRAIT:
                return "FaceStrait";

            case HeroAnimationType.D_DOWN:
                return "FaceDown";

            case HeroAnimationType.D_UP:
                return "FaceUp";

            case HeroAnimationType.D_DIAGONAL_UP:
                return "FaceDiagonalUp";

            case HeroAnimationType.D_DIAGONAL_DOWN:
                return "FaceDiagonalDown";
        }
    }
    public static string TypeToWeaponString(this HeroAnimationType type)
    {
        switch (type.TypeToWeapon())
        {
            default:
            case HeroAnimationType.W_NONE:
                return "NoWeapon";

            case HeroAnimationType.W_SWORD_IDLE:
                return "SwordIdle";

            case HeroAnimationType.W_SWORD_ATTACK:
                return "SwordAttack";

            case HeroAnimationType.W_SHOTGUN_IDLE:
                return "ShotgunIdle";

            case HeroAnimationType.W_SHOTGUN_ATTACK:
                return "ShotgunAttack";
        }
    }
    public static string TypeToStandardString(this HeroAnimationType type)
    {
        switch (type.TypeToStandard())
        {
            default:
            case HeroAnimationType.S_STAND:
                return "Stand";

            case HeroAnimationType.S_WALK:
                return "Walk";

            case HeroAnimationType.S_JUMP:
                return "Jump";
        }
    }

    // Parse Animation Type cast into proper name for animation.
    public static string TypeToAnimationString(this HeroAnimationType type)
    {
        return type.TypeToDirectionalString() + type.TypeToWeaponString() + type.TypeToStandardString();
    }
    public static HeroAnimationType TypeToStandard(this HeroAnimationType type)
    {
        return (HeroAnimationType)(HeroAnimationType.MASK_STANDARD & type);
    }
    public static HeroAnimationType TypeToWeapon(this HeroAnimationType type)
    {
        return (HeroAnimationType)(HeroAnimationType.MASK_WEAPON & type);
    }
    public static HeroAnimationType TypeToDirection(this HeroAnimationType type)
    {
        return (HeroAnimationType)(HeroAnimationType.MASK_DIRECTION & type);
    }
}

public static class TagTypeExtension 
{
    public static string ToTypeString(this TagTypes tag)
    {
        switch (tag)
        {
            case TagTypes.COLLECTABLE:
                return "Collectable";
            case TagTypes.ENEMY:
                return "Enemy";
            case TagTypes.EVENTTRIGGER:
                return "EventTrigger";
            case TagTypes.LEVELTERRAIN:
                return "LevelTerrain";
            case TagTypes.PLAYER:
                return "Player";
            default:
            case TagTypes.UNTAGGED:
                return "Untagged";
        }
    }
    public static TagTypes ToTagType(string tagString)
    {
        switch (tagString)
        {
            case "Collectable":
                return  TagTypes.COLLECTABLE;
            case "Enemy":
                return TagTypes.ENEMY;
            case "EventTrigger":
                return TagTypes.EVENTTRIGGER;
            case "LevelTerrain":
                return TagTypes.LEVELTERRAIN;
            case "Player":
                return TagTypes.PLAYER;
            default:
            case "Untagged":
                return TagTypes.UNTAGGED;
        }
    }
}