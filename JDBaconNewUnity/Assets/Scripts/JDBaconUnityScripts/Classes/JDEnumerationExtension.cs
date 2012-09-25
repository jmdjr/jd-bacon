
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

            case HeroAnimationType.W_SWORD:
                return "Sword";

            case HeroAnimationType.W_SHOTGUN:
                return "Shotgun";
        }
    }
    public static string TypeToStandardString(this HeroAnimationType type)
    {
        switch (type.TypeToStandard())
        {
            default:
            case HeroAnimationType.S_STAND:
                return "Stand";

            case HeroAnimationType.S_WALK_SWING:
                return "WalkSwing";

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