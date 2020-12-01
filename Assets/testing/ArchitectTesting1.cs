using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ArchitectTesting1 : MonoBehaviour
{

    public Text text;
    public TextMeshProUGUI tmprotext;
    TextArchitect architect;

    [TextArea (5,10)]
    public string say;
    public int charactersPerFrame = 1;
    public float speed = 1f;
    public bool useEncap = true;
    public bool
    
    // Start is called before the first frame update
    void Start()
    {
        architect = new TextArchitect(say);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            architect = new TextArchitect(say, "", charactersPerFrame, speed, useEncap);
        }

        text.text = architect.currentText; 
    }
}
