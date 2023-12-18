using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour
{
    [SerializeField] private GameObject droplets;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterScript>() != null)
        {
            GameManager.Instance.DestroyCharacter(collision.gameObject);
            Destroy(Instantiate(droplets, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity), 2f);
        }
    }
}