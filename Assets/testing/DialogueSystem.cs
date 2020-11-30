using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
	public static DialogueSystem instance;

	public ELEMENTS elements;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start()
	{

	}

	/// <summary>
	/// Say something and show it on the speech box.
	/// </summary>
	public void Say(string speech, string speaker = "")
	{
		StopSpeaking();

		speaking = StartCoroutine(Speaking(speech, false, speaker));
	}

	/// <summary>
	/// Say something to be added to what is already on the speech box.
	/// </summary>
	public void SayAdd(string speech, string speaker = "")
	{
		StopSpeaking();

		speechText.text = targetSpeech;

		speaking = StartCoroutine(Speaking(speech, true, speaker));
	}

	public void StopSpeaking()
	{
		if (isSpeaking)
		{
			StopCoroutine(speaking);
		}
		if (textArchitect != null && textArchitect.isConstructing)
        {
			textArchitect.Stop();
        }
		speaking = null;
	}

	public bool isSpeaking { get { return speaking != null; } }
	[HideInInspector] public bool isWaitingForUserInput = false;

	public string targetSpeech = "";
	Coroutine speaking = null;
	TextArchitect textArchitect = null;
	IEnumerator Speaking(string speech, bool additive, string speaker = "")
	{
		speechPanel.SetActive(true);

		string additiveSpeech = additive ? speechText.text : "";
		targetSpeech = additiveSpeech + speech;

		 textArchitect = new TextArchitect(speech, additiveSpeech);
		

		speakerNameText.text = DetermineSpeaker(speaker);//temporary
s
		isWaitingForUserInput = false;

		while (textArchitect.isConstructing)
		{
			if (Input.GetKeyDown(KeyCode.Space))
				textArchitect.skip = true;

			speechText.text = TextArchitect.currentText;
			 
			yield return new WaitForEndOfFrame();
		}
		  speechText.text = TextArchitect.currentText;


		//text finished
		isWaitingForUserInput = true;
		while (isWaitingForUserInput)
			yield return new WaitForEndOfFrame();

		StopSpeaking();
	}

	string DetermineSpeaker(string s)
	{
		string retVal = speakerNameText.text;//default return is the current name
		if (s != speakerNameText.text && s != "")
			retVal = (s.ToLower().Contains("narrator")) ? "" : s;

		return retVal;
	}

	/// <summary>
	/// Close the entire speech panel. Stop all dialogue.
	/// </summary>
	public void Close()
	{
		StopSpeaking();
		speechPanel.SetActive(false);
	}

	[System.Serializable]
	public class ELEMENTS
	{
		/// <summary>
		/// The main panel containing all dialogue related elements on the UI
		/// </summary>
		public GameObject speechPanel;
		public TextMeshProUGUI speakerNameText;
		public TextMeshProUGUI speechText;
	}
	public GameObject speechPanel { get { return elements.speechPanel; } }
	public TextMeshProUGUI speakerNameText { get { return elements.speakerNameText; } }
	public TextMeshProUGUI speechText { get { return elements.speechText; } }
}