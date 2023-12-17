using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObiWan : EnemyScript
{
    public Transform EnemyAttacksDisplay;
    public AudioClip mainLoop;
    public AudioClip[] attacks;

    public AudioSource musicPlayer;
    public AudioSource soundPlayer;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        StartCoroutine(musicalBattle());
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        //Move based off music timings
    }

    IEnumerator musicalBattle()
    {

        if (playMusic)
        {
            //Intro
            musicPlayer.Play();
            yield return new WaitUntil(() => !musicPlayer.isPlaying);
            //yield return new WaitForSeconds(10.681f);
            musicPlayer.clip = mainLoop;
            musicPlayer.loop = true;
            musicPlayer.Play();
        }

        while (true)
        {
            //bpm - 90
            //Attack at every 7th note
            float attackTime = 4;
            for (int i = 0; i < 8; i++)
            {
                yield return new WaitUntil(() => musicPlayer.time >= attackTime);
                attackTime += 5.333f;

                //Randomize Attacks
                int[] attackBuffer = new int[3];
                int randNum;
                //First 2 Attacks
                for (int b = 0; b < 2; b++)
                {
                    randNum = Random.Range(0, 7);
                    //Block (2/7)
                    if (randNum < 2)
                        attackBuffer[b] = 0;
                    //Stab (4/7)
                    else if (randNum < 6)
                        attackBuffer[b] = 1;
                    //Dodge (1/7) - Roll away from Player
                    else
                        attackBuffer[b] = 3;
                }
                //Last Attack
                randNum = Random.Range(0, 8);
                //Block (2/8)
                if (randNum < 2)
                    attackBuffer[2] = 0;
                //Stab (1/8)
                else if (randNum < 3)
                    attackBuffer[2] = 1;
                //Swing (3/8)
                else if (randNum < 6)
                    attackBuffer[2] = 2;
                //Dodge (1/8) - Roll Away from Player
                else if (randNum < 7)
                    attackBuffer[2] = 3;
                //Roll (1/8) - Roll Towards Player
                else
                    attackBuffer[2] = 4;

                //Put Word Display of Attacks in Scene
                for (int a = 0; a < 3; a++)
                {
                    string attackType = "";
                    switch (attackBuffer[a])
                    {
                        case 0: attackType = "BLOCK"; break;
                        case 1: attackType = "STAB"; break;
                        case 2: attackType = "SWING"; break;
                        case 3: attackType = "DODGE"; break;
                        case 4: attackType = "ROLL"; break;
                    }
                    EnemyAttacksDisplay.transform.GetChild(a).GetComponentInChildren<TextMeshPro>().text = attackType;
                }

                EnemyAttacksDisplay.GetComponent<Animator>().SetTrigger("Play");
                defaultMovement = false;
                bool facingRightReset = facingRight;
                movementDir = facingRight ? 1 : -1;
                blocking = false;

                //Enact the Attacks + Sounds
                for (int a = 0; a < 6; a++)
                {
                    facingRight = facingRightReset;
                    soundPlayer.clip = attacks[attackBuffer[a % 3]];
                    soundPlayer.Play();

                    //Add Attacks
                    if (a > 2)
                    {
                        switch (attackBuffer[a % 3])
                        {
                            case 0: blocking = true; break;
                            case 1: activateStab(); break;
                            case 2: activateSwing(); break;
                            case 3: activateRoll(!facingRight, .5f); break;
                            case 4: activateRoll(facingRight, 1); break;
                        }
                    }
                    yield return new WaitForSeconds(.222f);
                }

                EnemyAttacksDisplay.GetComponent<Animator>().ResetTrigger("Play");
                defaultMovement = true;
            }
            //Reset to beggining
            yield return new WaitUntil(() => musicPlayer.time < 1);
        }

    }
}
