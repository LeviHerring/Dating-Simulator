﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelController : MonoBehaviour
{
    // Start is called before the first frame update
    List<string> data = new List<string>();

    int progress = 0;
    void Start()
    {
        LoadChapterFile("chapter0_start");
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void LoadChapterFile(string fileName)
    {
        data = FileManager.LoadFile(FileManager.savPath + "Resources/Story/" + fileName);
        progress = 0;
        cachedLastSpeaker = "";
    }

    void HandleLine(string line)
    {
        string[] dialogueAndActions = line.Split('"');

        if (dialogueAndActions.Length == 3)
        {
            HandleDialogue(dialogueAndActions[0], dialogueAndActions[1]);
            HandleEventsFromLine(dialogueAndActions[2]);
                
        }
        else
        {
            HandleEventsFromLine(dialogueAndActions[0]);
        }

    }

    string cachedLastSpeaker = "";
    void HandleDialogue(string dialogueDetails, string dialogue)
    {
        string speaker = cachedLastSpeaker;
        bool additive = dialogueDetails.Contains("+");

        if (additive)
            dialogueDetails = dialogueDetails.Remove(dialogueDetails.Length - 1);

        if (dialogueDetails.Length > 0)
        {
            if(dialogueDetails[dialogueDetails.Length-1] == ' ')
                dialogueDetails = dialogueDetails.Remove(dialogueDetails.Length - 1);
             
            speaker = dialogueDetails;
            cachedLastSpeaker = speaker;
        }

        if (speaker != "narrator")
        {
            Character character = CharacterManager.instance.GetCharacter(speaker);
            character.Say(dialogue, additive);
        }
        else
        {
            DialogueSystem.instance.Say(dialogue, speaker, additive);
        }
    }
     
       void HandleEventsFromLine(string events)
    {
        
        string actions = events.Split(' ');


        foreach(string action in actions)
        {
            HandleAction(action);
        }
    }

    void HandleAction(string action)
    {
        print("Handle action [" + action + "]");
        string[] data = action.Split('(', ')');
        if (data[0] == "setBackground")
        {
             
        }
    }

    void Command_SetLayerImage(string data, BCFC.LAYER layer)
      
}