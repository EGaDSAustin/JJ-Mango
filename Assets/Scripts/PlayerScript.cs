using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public InputActionMap actionMap;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        actionMap.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = rb.velocity;

        //Movement
        float inputDir = actionMap.actions[0].ReadValue<float>();
        if ((inputDir > 0 && velocity.x < 4) || (inputDir < 0 && velocity.x > -4))
            velocity.x += inputDir * .05f;
        if (inputDir == 0)
            velocity.x *= .9f;

        //Jump
        if (actionMap.actions[1].WasPressedThisFrame() && Physics2D.Raycast(transform.position, Vector2.down * .5f))
            velocity.y = 3;

        rb.velocity = velocity;
    }
}
