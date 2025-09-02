using System.Collections;
using System;
using TMPro;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public string line;

    public float textSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(textComponent.text == line)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = line;
            }
        }
    }

    public void StartDialog()
    {
        gameObject.SetActive(true);
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    public void SetLine(string input)
    {
        line = input;
    }

    IEnumerator TypeLine()
    {
        foreach(char c in line.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        gameObject.SetActive(false);
    }
}
