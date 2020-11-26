using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Character Vietnam; 

    // Start is called before the first frame update
    void Start()
    {
        Vietnam = CharacterManager.instance.GetCharacter("Vietnam"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
