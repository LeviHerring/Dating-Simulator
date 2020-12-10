using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTesting : MonoBehaviour
{
    public Character Vietnam;

    public string speaker;

    // Use this for initialization
    void Start()
    {
        Vietnam = CharacterManager.instance.GetCharacter(speaker, enableCreatedCharacterOnStart: true);
        DialogueSystem.instance.Close();
    }

    public string[] speech;
    int i = 0;

    public Vector2 moveTarget;
    public float moveSpeed;
    public bool smooth;

    public int bodyIndex, expressionIndex = 0;
    public float speed = 5f;
    public bool smoothtransitions = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (i < speech.Length)
            {
                Vietnam.Say(speech[i], Input.GetKey(KeyCode.A));
            }
            else
                DialogueSystem.instance.Close();

            i++;
        }

        if (Input.GetKey(KeyCode.M))
        {
            Vietnam.MoveTo(moveTarget, moveSpeed, smooth);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Vietnam.StopMoving(true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (Input.GetKey(KeyCode.T))
                Vietnam.TransitionBody(Vietnam.GetSprite(bodyIndex), speed, smoothtransitions);
            else
                Vietnam.SetBody(bodyIndex);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.T))
                Vietnam.TransitionExpression(Vietnam.GetSprite(expressionIndex), speed, smoothtransitions);
            else
                Vietnam.SetExpression(expressionIndex);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Input.GetKey(KeyCode.UpArrow))
                Vietnam.FaceLeft();
            else if (Input.GetKey(KeyCode.DownArrow))
                Vietnam.FaceRight();
            else
                Vietnam.Flip();
        }
    }
}