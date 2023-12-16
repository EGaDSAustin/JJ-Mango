using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    public const float CHARACTER_TIME = 0.03f;
    public TextMeshProUGUI mTextMeshPro;

    private String mCurrentString;
    private int mCharacterProgress = 0;

    private Coroutine mTextScrollingCoroutine;

    public String[] mTextList;

    IEnumerator PrintScrolling()
    {
        mTextMeshPro.text = "";

        while(mCharacterProgress < mCurrentString.Length)
        {
            // suspend execution for 5 seconds
            yield return new WaitForSeconds(CHARACTER_TIME);

            mTextMeshPro.text += mCurrentString[mCharacterProgress];

            ++mCharacterProgress;
        }
    }

    private void SetText(String newTextString)
    {
        mCurrentString = newTextString;
        mCharacterProgress = 0;

        if (mCurrentString.Length <= 0)
        {
            GetComponentInChildren<Canvas>().enabled = false;
        } else
        {
            GetComponentInChildren<Canvas>().enabled = true;

            if (mTextScrollingCoroutine != null)
            {
                StopCoroutine(mTextScrollingCoroutine);
            }

            mTextScrollingCoroutine = StartCoroutine("PrintScrolling");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetText("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onTextAreaEntered(int areaID)
    {
        SetText(mTextList[areaID]);
    }
}
