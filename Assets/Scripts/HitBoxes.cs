using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxes : MonoBehaviour
{
    public float strength = 500;
    public bool attackBoth = false;

    [SerializeField] private GameObject droplets;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 6)
        {
            //Calculate Force
            Vector2 pushForce = new Vector2(1, .5f) * strength;
            if (collision.transform.position.x < transform.parent.position.x)
                pushForce.x *= -1;

            CharacterScript character = collision.GetComponent<CharacterScript>();
            character.resetAttacks();
            character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            character.GetComponent<Rigidbody2D>().AddForce(pushForce * (character.blocking ? .666f : 1));
            character.flingTime = Time.time + strength * .0005f;

            Destroy(Instantiate(droplets, collision.transform.position, Quaternion.identity), 2f);

            //Ball form dangerous to both
            if (attackBoth)
            {
                pushForce.x *= -1;
                character = GetComponentInParent<CharacterScript>();
                character.resetAttacks();
                character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                character.GetComponent<Rigidbody2D>().AddForce(pushForce * (character.blocking ? .666f : 1));
                character.flingTime = Time.time + strength * .0005f;
            }
        }    
    }
}
