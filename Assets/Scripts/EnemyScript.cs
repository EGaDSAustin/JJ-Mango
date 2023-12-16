using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public bool blocking = false;

    bool defaultMovement = true;

    Rigidbody2D rb;
    PlayerScript player;

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerScript>();
    }

    // Update is called once per frame
    protected void Update()
    {
        Vector2 velocity = rb.velocity;

        float inputDir = 0;

        if (defaultMovement)
        {
            //Move Closer if far away
            if (Mathf.Abs(player.transform.position.x - transform.position.x) > 4)
                inputDir = Mathf.Clamp(player.transform.position.x - transform.position.x, -1, 1);
            else
                //else waver
                inputDir = Mathf.Sin(Time.time * 3) * .5f;

            //Only Blocks if close enough
            blocking = Mathf.Abs(player.transform.position.x - transform.position.x) < 6;
        }

        //Movement
        if ((inputDir > 0 && velocity.x < 3.5) || (inputDir < 0 && velocity.x > -3.5))
            velocity.x += inputDir * .05f;
        if (Mathf.Abs(inputDir) < .1)
            velocity.x *= .9f;

        if (blocking)
            velocity.x *= .98f;

        //Jump
        if (Random.Range(0, 50f) < .01f && Physics2D.Raycast(transform.position, Vector2.down, 1.1f, ~(1 << 6)).collider != null)
            velocity.y = 3;

        rb.velocity = velocity;
    }
}
