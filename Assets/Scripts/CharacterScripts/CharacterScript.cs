using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    //Assets
    public PhysicsMaterial2D ballPhysics;

    //Children Class Determine Movement Vectors
    protected float movementDir = 0;
    protected bool attemptJump = false;
    protected bool facingRight = true;
    //Movement Attributes
    protected float blockingSpeed = .97f;
    protected float maxSpeed = 4;
    float capsuleSize = .5f;

    //Attacking Vars
    protected float cooldown = 0;
    public bool blocking = false;
    public Vector2 rollForce = Vector2.zero;
    bool resetAfterCooldown = false;

    //Fling Variables
    public float flingTime = 0;

    Rigidbody2D rb;

    // Start is called before the first frame update
    protected void Start()
    {
        capsuleSize = GetComponent<CapsuleCollider2D>().size.y / 2 * transform.localScale.y;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (flingTime < Time.time)
        {
            Vector2 velocity = rb.velocity;

            bool onGround = Physics2D.Raycast(transform.position, Vector2.down, capsuleSize + .05f, 1 << 0).collider != null;

            //Disable Directional Movement if Attacking
            if (cooldown - .25 > Time.time)
            {
                resetAfterCooldown = true;
                movementDir = 0;
            }
            //After Attacking, reset Attacks
            else if (resetAfterCooldown)
                resetAttacks();

            //Quick Turn Around
            if (((movementDir > 0 && velocity.x < 0) || (movementDir < 0 && velocity.x > 0)) && onGround)
                velocity.x = 0;
            //Accelerate to Max Veloc
            if ((movementDir > 0 && velocity.x < maxSpeed) || (movementDir < 0 && velocity.x > -maxSpeed))
                velocity.x += movementDir * .05f;
            //Decelerate quicker when not moving
            if (movementDir == 0 && onGround)
                velocity.x *= .9f;

            //Blocking slows movement
            if (blocking)
                velocity.x *= blockingSpeed;

            //Jump
            if (attemptJump && onGround)
                velocity.y = 3;

            //If not on ground, movement not as effective
            if (!onGround)
                velocity = (velocity * .25f) + (rb.velocity * .75f);

            rb.velocity = velocity;

            //Applies BALL FORM Force
            rb.AddForce(rollForce);
        }
        else
        {
            //Some force falloff for the fling force
            Vector2 velocity = rb.velocity;
            velocity.x *= .999f;
            rb.velocity = velocity;
        }
    }

    protected void activateStab()
    {

    }

    protected void activateSwing()
    {

    }

    protected void activateRoll(bool rollRight)
    {
        cooldown = Time.time + .75f;
        rollForce = Vector2.right * (rollRight ? 30 : -30);
        GetComponent<CapsuleCollider2D>().size = Vector2.one;
        GetComponent<Collider2D>().sharedMaterial = ballPhysics;
        transform.GetChild(0).gameObject.SetActive(true);
        blocking = false;
    }

    public void resetAttacks()
    {
        //Reset Collider after rolling
        GetComponent<CapsuleCollider2D>().size = new Vector2(1, 2);
        GetComponent<Collider2D>().sharedMaterial = null;
        rollForce = Vector2.zero;

        //Disable HitBoxes
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        resetAfterCooldown = false;
    }
}
