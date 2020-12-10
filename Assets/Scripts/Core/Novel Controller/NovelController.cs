using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelController : MonoBehaviour
{
    public static NovelController instance;
    // Start is called before the first frame update
    List<string> data = new List<string>();

    int progress = 0;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        LoadChapterFile("chapter0_start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Next();
        }
    }


    public void LoadChapterFile(string fileName)
    {

        data = FileManager.ReadTextAsset(Resources.Load<TextAsset>($"Story/{fileName}"));
        cachedLastSpeaker = "";

        if (handlingChapterFile != null)
            StopCoroutine(handlingChapterFile);
        handlingChapterFile = StartCoroutine(HandlingChapterFile());

        //auto start the chapter.
        Next();
    }

    bool _next = false;
    public void Next()
    {
        _next = true;
    }

    public bool isHandlingChapterFile { get { return handlingChapterFile != null; } }
    [HideInInspector] public int chapterProgress = 0;
    Coroutine handlingChapterFile = null;
    IEnumerator HandlingChapterFile()
    {
        chapterProgress = 0;

        while (chapterProgress < data.Count)
        {
            if (_next)
            {
                string line = data[chapterProgress];

                if (line.StartsWith("choice"))
                {
                    yield return HandlingChoiceLine(line);
                    chapterProgress++;
                }
                else
                {
                    HandleLine(line);
                    progress++;
                    while (isHandlingLine)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }

                
            }
            yield return new WaitForEndOfFrame();
        }

        handlingChapterFile = null;
    }

    IEnumerator HandlingChoiceLine(string line)
    {
        string title = line.Split('"')[1];
        List<string> choices = new List<string>();
        List<string> actions = new List<string>();

        bool gatheringChoices = true;
        while (gatheringChoices)
        {
            chapterProgress++;
            line = data[chapterProgress];

            if (line == "{")
                continue;

            line = line.Replace("    ", "");//remove the tabs that have become quad spaces.

            if (line != "}")
            {
                choices.Add(line.Split('"')[1]);
                actions.Add(data[chapterProgress + 1].Replace("    ", ""));
                chapterProgress++;
            }
            else
            {
                gatheringChoices = false;
            }

        }

        if (choices.Count > 0)
        {
            ChoiceScreen.Show(title, choices.ToArray()); yield return new WaitForEndOfFrame();
            while (ChoiceScreen.isWaitingForChoiceToBeMade)
                yield return new WaitForEndOfFrame();

            //choice is made. execute the paired action.
            string action = actions[ChoiceScreen.lastChoiceMade.index];
            HandleLine(action);

            while (isHandlingLine)
                yield return new WaitForEndOfFrame();
        }
        else
        {
            Debug.LogError("Invalid choice operation. No choices were found.");
        }

        
    }

        void HandleLine(string rawLine)
    {
        CLM.LINE line = CLM.Interpret(rawLine);




        StopHandlingLine();
        handlingLine = StartCoroutine(HandlingLine(line));
    }

    void StopHandlingLine()
    {
        if (isHandlingLine)
            StopCoroutine(handlingLine);
        handlingLine = null;
    }

    [HideInInspector]

    public string cachedLastSpeaker = "";

    bool isHandlingLine{ get { return handlingLine != null; } }
    Coroutine handlingLine = null;
    IEnumerator HandlingLine(CLM.LINE line)
    {
        _next = false;
        int lineProgress = 0;
        while(lineProgress < line.segments.Count)
        {
            _next = false;
            CLM.LINE.SEGMENT segment = line.segments[lineProgress];

            if (lineProgress > 0)
            {
                if (segment.trigger == CLM.LINE.SEGMENT.TRIGGER.autoDelay)
                {
                   for (float timer = segment.autoDelay; timer >= 0; timer -= Time.deltaTime)
                    {
                        yield return new WaitForEndOfFrame();
                        if (_next)
                            break; 
                    }
                }
                else
                {
                    while(!_next)
                        yield return new WaitForEndOfFrame();
                }
            }
            _next = false;

            segment.Run();
            while(segment.isRunning)
            {
                yield return new WaitForEndOfFrame();

                if (_next)
                {
                    if (!segment.architect.skip)
                        segment.architect.skip = true;
                    else
                        segment.ForceFinish();
                    _next = false;
                }
            }

            lineProgress++;

            yield return new WaitForEndOfFrame();

        }
        for(int i = 0; i < line.actions.Count; i++)
        {
            HandleAction(line.actions[i]);
        }
        handlingLine = null;
    }

    
    

    public void HandleAction(string action)
    {
        print("Handle action [" + action + "]");
        string[] data = action.Split('(', ')');

        switch (data[0])
        {
            case "enter":
             Command_Enter(data[1]);
            break;

            case "exit":
            Command_Exit(data[1]);
            break;

            //case "savePlayerName":
            //Command_SavePlayerName(InputScreen.currentInput);//saves the player name as what was last input by the player
            //break;

            case "setBackground":
                Command_SetLayerImage(data[1], BCFC.instance.background);
                break;

            //case "setCinematic":
            //Command_SetLayerImage(data[1], BCFC.instance.cinematic);
            //break;

            case "setForeground":
                Command_SetLayerImage(data[1], BCFC.instance.foreground);
                break;

            case "playSound":
                Command_PlaySound(data[1]);
                break;

            case "playMusic":
                Command_PlayMusic(data[1]);
                break;

            //case "playAmbiance":
            //Command_PlayAmbiance(data[1]);
            //break;

            //case "stopAmbiance":
            //if (data[1] != "")
            //Command_StopAmbiance(data[1]);
            // else
            //Command_StopAllAmbiance();
            //break;

            case "move":
                Command_MoveCharacter(data[1]);
                break;

            case "setPosition":
                Command_SetPosition(data[1]);
                break;

            case "setFace":
                Command_SetFace(data[1]);
                break;

            case "setBody":
                Command_SetBody(data[1]);
                break;

            case "flip":
                Command_Flip(data[1]);
                break;

            case "faceLeft":
                Command_FaceLeft(data[1]);
                break;

            case "faceRight":
                Command_FaceRight(data[1]);
                break;
            
            case "transBackground":
                Command_TransLayer(BCFC.instance.background, data[1]);
                break;

            //case "transCinematic":
                //Command_TransLayer(BCFC.instance.cinematic, data[1]);
                //break;

            case "transForeground":
                Command_TransLayer(BCFC.instance.foreground, data[1]);
                break;
            
            case "showScene":
                Command_ShowScene(data[1]);
                break;

            case "Load":
                Command_Load(data[1]);
                break;



        }

        void Command_Load(string chapterName)
        {
            NovelController.instance.LoadChapterFile(chapterName);
        }

        void Command_SetLayerImage(string layerData, BCFC.LAYER layer)
        {
            string texName = layerData.Contains(",") ? layerData.Split(',')[0] : layerData;
            Texture2D tex = texName == "null" ? null : Resources.Load("Images/UI/backdrops/" + texName) as Texture2D;
            float spd = 2f;
            bool smooth = false;

            if (layerData.Contains(","))
            {
                string[] parameters = layerData.Split(',');
                foreach (string p in parameters)
                {
                    float fVal = 0;
                    bool bVal = false;
                    if (float.TryParse(p, out fVal))
                    {
                        spd = fVal; continue;
                    }
                    if (bool.TryParse(p, out bVal))
                    {
                        smooth = bVal; continue;
                    }
                }

            }

            layer.TransitionToTexture(tex, spd, smooth);

        }

        void Command_PlaySound(string soundData)
        {
            AudioClip clip = Resources.Load("Audio/SFX/" + soundData) as AudioClip;

            if (clip != null)
                AudioManager.instance.PlaySFX(clip);
            else
                Debug.LogError("Clip does not exist - " + soundData);
        }

        void Command_PlayMusic(string musicData)
        {
            AudioClip clip = Resources.Load("Audio/Music/" + musicData) as AudioClip;

            if (clip != null)
                AudioManager.instance.PlaySFX(clip);
            else
                Debug.LogError("Clip does not exist - " + musicData);
        }

        void Command_MoveCharacter(string moveData)
        {
            string[] parameters = moveData.Split(',');
            string character = parameters[0];
            float locationX = float.Parse(parameters[1]);
            float locationY = parameters.Length >= 3 ? float.Parse(parameters[2]) : 0;
            float speed = parameters.Length >= 4 ? float.Parse(parameters[3]) : 7f;
            bool smooth = parameters.Length == 5 ? bool.Parse(parameters[4]) : true;

            Character c = CharacterManager.instance.GetCharacter(character);
            c.MoveTo(new Vector2(locationX, locationY), speed, smooth);
        }
        void Command_SetPosition(string positionData)
        {
            string[] parameters = positionData.Split(',');
            string character = parameters[0];
            float locationX = float.Parse(parameters[1]);
            float locationY = float.Parse(parameters[2]);

            Character c = CharacterManager.instance.GetCharacter(character);
            c.SetPosition(new Vector2(locationX, locationY));
        }
        void Command_SetFace(string faceData)
        {
            string[] parameters = faceData.Split(',');
            string character = parameters[0];
            string expression = parameters[1];
            float speed = parameters.Length == 3 ? float.Parse(parameters[2]) : 3f;

            Character c = CharacterManager.instance.GetCharacter(character);
            Sprite sprite = c.GetSprite(expression);

            c.TransitionExpression(sprite, speed, false);
        }

        void Command_SetBody(string bodyData)
        {
            string[] parameters = bodyData.Split(',');
            string character = parameters[0];
            string expression = parameters[1];
            float speed = parameters.Length == 3 ? float.Parse(parameters[2]) : 3f;

            Character c = CharacterManager.instance.GetCharacter(character);
            Sprite sprite = c.GetSprite(expression);

            c.TransitionBody(sprite, speed, false);
        }

        void Command_Flip(string flipData)
        {
            string[] characters = flipData.Split(';');

            foreach (string s in characters)
            {
                Character c = CharacterManager.instance.GetCharacter(s);
                c.Flip();
            }
        }

        void Command_FaceLeft(string faceLeftData)
        {
            string[] characters = faceLeftData.Split(';');

            foreach (string s in characters)
            {
                Character c = CharacterManager.instance.GetCharacter(s);
                c.FaceLeft();
            }
        }

        void Command_FaceRight(string faceRightData)
        {
            string[] characters = faceRightData.Split(';');

            foreach (string s in characters)
            {
                Character c = CharacterManager.instance.GetCharacter(s);
                c.FaceRight();
            }
        }

        void Command_Exit(string exitData)
        {
            string[] parameters = exitData.Split(',');
            string[] characters = parameters[0].Split(';');
            float speed = 3;
            bool smooth = false;
            for (int i = 1; i < parameters.Length; i++)
            {
                float fVal = 0; bool bVal = false;
                if (float.TryParse(parameters[i], out fVal))
                { speed = fVal; continue; }
                if (bool.TryParse(parameters[i], out bVal))
                { smooth = bVal; continue; }
            }

            foreach (string s in characters)
            {
                Character c = CharacterManager.instance.GetCharacter(s);
                c.FadeOut(speed, smooth);
            }
        }
        void Command_Enter(string enterData)
        {
            string[] parameters = enterData.Split(',');
            string[] characters = parameters[0].Split(';');
            float speed = 3;
            bool smooth = false;
            for (int i = 1; i < parameters.Length; i++)
            {
                float fVal = 0; bool bVal = false;
                if (float.TryParse(parameters[i], out fVal))
                { speed = fVal; continue; }
                if (bool.TryParse(parameters[i], out bVal))
                { smooth = bVal; continue; }
            }

            foreach (string s in characters)
            {
                Character c = CharacterManager.instance.GetCharacter(s, true, false);
                c.enabled = true;
                c.FadeIn(speed, smooth);
            }
        }
        void Command_TransLayer(BCFC.LAYER layer, string transLayerData)
        {
            string[] parameters = transLayerData.Split(',');

            string texName = parameters[0];
            string transTexName = parameters[1];
            Texture2D tex = texName == "null" ? null : Resources.Load("Images/UI/Backdrops/" + texName) as Texture2D;
            Texture2D transTex = Resources.Load("Images/TransitionEffects/" + transTexName) as Texture2D;

            float spd = 2f;
            bool smooth = false;

            for (int i = 2; i < parameters.Length; i++)
            {
                string p = parameters[i];
                float fVal = 0;
                bool bVal = false;
                if (float.TryParse(p, out fVal))
                { spd = fVal; continue; }
                if (bool.TryParse(p, out bVal))
                { smooth = bVal; continue; }
            }

            TransitionMaster.TransitionLayer(layer, tex, transTex, spd, smooth);
        }
        
        void Command_ShowScene(string showSceneData)
        {
            string[] parameters = showSceneData.Split(',');
            bool show = bool.Parse(parameters[0]);
            string texName = parameters[1];
            Texture2D transTex = Resources.Load("Images/TransitionEffects/" + texName) as Texture2D;
            float spd = 2f;
            bool smooth = false;

            for (int i = 2; i < parameters.Length; i++)
            {
                string p = parameters[i];
                float fVal = 0;
                bool bVal = false;
                if (float.TryParse(p, out fVal))
                { spd = fVal; continue; }
                if (bool.TryParse(p, out bVal))
                { smooth = bVal; continue; }
            }

            TransitionMaster.ShowScene(show, spd, smooth, transTex);
        }


    }
}



 