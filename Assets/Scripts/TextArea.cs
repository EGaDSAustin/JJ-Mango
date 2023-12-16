using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextArea : MonoBehaviour
{
    public TextBox textBox;
    public int areaID;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            textBox.onTextAreaEntered(areaID);
        }
    }
}
