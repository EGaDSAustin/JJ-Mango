using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : CharacterScript
{
    bool defaultMovement = true;

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
        if (defaultMovement)
        {
            //Move Closer if far away
            if (Mathf.Abs(player.transform.position.x - transform.position.x) > 4)
                movementDir = Mathf.Clamp(player.transform.position.x - transform.position.x, -1, 1);
            else
                //else waver
                movementDir = Mathf.Sin(Time.time * 3) * .5f;

            //Only Blocks if close enough
            blocking = Mathf.Abs(player.transform.position.x - transform.position.x) < 6;

            //Jump Randomly for Fun
            attemptJump = Random.Range(0, 50f) < .0125f;
        }

        //Physics Updating
        base.Update();
    }
}
