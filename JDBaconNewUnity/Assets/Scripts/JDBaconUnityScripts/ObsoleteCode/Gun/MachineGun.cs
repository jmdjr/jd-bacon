using UnityEngine;
using System.Collections;

public class MachineGun : Weapon
{
    // Type of bullet that belongs to the gun
    public Bullet mBullet;

    // Used to keep the bullets moving in the same direction regardless of character position
    public bool mLockRotation;
    private Quaternion mStartingRotation;

    /// <summary>
    /// Gizmo is only shown in the editor, invisible in the game
    /// Used for reference of where the weapon will be created at
    /// </summary>
    void onDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * .5f);
    }

    // Use this for initialization
    void Start()
    {
        if (mBullet == null)
        {
            Debug.LogError("Bullet Type not set");
            Debug.Break(); 
        }

        
        mStartingRotation = transform.rotation;
    }


    // Function that is modified by the weapon being used
    public override IEnumerator Attack()
    {
        Debug.LogError("Firing Machine Gun");
        Debug.Break();
        mCreator = transform.parent.gameObject.GetComponent<Character>();

        // if it is on screen
        if (mCreator.renderer.isVisible)
        {
            
            if (mLockRotation)
            {
                transform.rotation = mStartingRotation;
            }

            Bullet bullet = (Bullet)GameObject.Instantiate(mBullet, transform.position, transform.rotation);
            bullet.setCharacter(mCreator);
            yield return new WaitForSeconds(attackRate);
        }

    }
}

