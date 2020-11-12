using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Character Vietnam;
    public Character UK;
    // Start is called before the first frame update
    void Start()
    {

        Vietnam = CharacterManager.Instance.GetCharacter("Vietnam", enableCreatedCharacterOnStart: false);
        UK = CharacterManager.Instance.GetCharacter("UK", enableGetCreatedCharacterOnStart: false);

    }

    public string[] speech;
    int i = 0;

    public Vector2 moveTarget;
    public float moveSpeed;
    public smooth; 

    public float ant = sf;     // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Space))
        {
            if (i < speech.Length)
                Vietnam.Say(speech[i]);
            else
                DialogueSystem.instance.Close();

            i++;
        }
        if (Input.GetKey (KeyCode.M))
        {
            Vietnam.MoveTo(moveTarget, moveSpeed, smooth);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vietnam.StopMoving (true); 
        }
    }
}
