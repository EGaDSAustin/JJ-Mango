using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerScript : CharacterScript
{
    public bool attackPenguin = false;
    public Image[] keybindImages;
    public InputActionMap actionMap;

    // Start is called before the first frame update
    new void Start()
    {
        actionMap.Enable();

        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        //Movement
        movementDir = actionMap.actions[0].ReadValue<float>();

        attemptJump = actionMap.actions[1].WasPressedThisFrame();

        //Attack Moves (not in library)
        if (attackPenguin)
        {
            //Keybinds to images
            for (int i = 0; i < 5; i++)
                keybindImages[i].color = actionMap.actions[i + 2].ReadValue<float>() > .5 ? Color.white : Color.white * .85f;

            if (cooldown < Time.time)
            {
                //Facing Direction if in cooldown
                if (movementDir != 0)
                    facingRight = movementDir > 0;

                //Blocking
                blocking = actionMap.actions[2].ReadValue<float>() > .5;

                //Other Attacks
                if (actionMap.actions[3].WasPressedThisFrame())
                    activateStab();
                else if (actionMap.actions[4].WasPressedThisFrame())
                    activateSwing();

                //Roll Left
                else if (actionMap.actions[5].WasPressedThisFrame())
                    activateRoll(false);
                //Roll Right
                else if (actionMap.actions[6].WasPressedThisFrame())
                    activateRoll(true);
            }
        }

        //Physics Updating
        base.Update();
    }
}