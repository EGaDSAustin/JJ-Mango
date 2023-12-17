using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : CharacterScript
{
    public bool defaultMovement = true;

    PlayerScript player;

    // Start is called before the first frame update
    new protected void Start()
    {
        //Slower general speed, but higher blocking speed
        maxSpeed = 3.5f;
        blockingSpeed = .98f;

        player = FindObjectOfType<PlayerScript>();

        base.Start();
    }

    // Update is called once per frame
    new protected void Update()
    {
        if (defaultMovement && cooldown - .25 < Time.time)
        {
            //Face Player
            facingRight = player.transform.position.x > transform.position.x;

            float playerDistance = Mathf.Abs(player.transform.position.x - transform.position.x);

            //Move Closer if far away or if player is pushing against
            if (playerDistance > 4 || playerDistance < 1)
                movementDir = Mathf.Clamp(player.transform.position.x - transform.position.x, -1, 1);
            else
                //else waver
                movementDir = Mathf.Sin(Time.time * 3) * .5f;

            //Only Blocks if close enough
            blocking = Mathf.Abs(player.transform.position.x - transform.position.x) < 6;

            //Jump Randomly for Fun if player not close
            if (playerDistance > 1)
                attemptJump = Random.Range(0, 50f) < .02f;
        }

        //Physics Updating
        base.Update();
    }
}
