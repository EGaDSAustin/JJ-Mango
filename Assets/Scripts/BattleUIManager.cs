using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLibraryClicked()
    {
        GameManager.Instance.LoadLibrary();
    }

    public void OnMainMenuClicked()
    {
        GameManager.Instance.LoadMainMenu();
    }
}
