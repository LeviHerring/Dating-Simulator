using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Dynamic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{
    public string CharacterName;
    [HideInInspector] public RectTransform root;
    public bool isMultiLayerCharacter { get { return renderers.renderer == null; } }

    public bool enabled{get { return root.GameObject.activeInHierarchy; } get { return root.gameObject.GetActive(value); } }

    DialogueSystem dialogue; 

    public void Say(string speech, bool add = false)
    {
        if (!enabled)
            enabled = true;
        if (!add)
            dialogue.Say(speech, CharacterName);
        else
            dialogue.SayAdd(speech, characterName);
    }

    public Vector2 anchorPadding { get { return root.anchorMax - root.anchorMin; } }

    Vector2 targetPosition;
    Coroutine moving;
    bool isMoving { get { return moving != null; } }
    /// <summary>
    /// Move to a specific point relative to the canvas space. (1,1) = far top right, (0,0) = far bottom left, (0.5,0.5) = Middle.
    /// </summary>
    /// <param name="Target">Target.</param>
    /// <param name="speed">Speed.</param>
    /// <param name="smooth">If set to <c>true</c> smooth.</param>
    public void MoveTo(Vector2 Target, float speed, bool smooth = true)
    {
        //if we are moving, stop moving
        StopMoving();
        //start moving coroutine.
        moving = CharacterManager.instance.StartCoroutine(Moving(Target, speed, smooth));
    }

    /// <summary>
    /// Stops the character in its tracks, either setting it immediately at the target position or not.
    /// </summary>
    /// <param name="arriveAtTargetPositionImmediately">If set to <c>true</c> arrive at target position immediately.</param>
    public void StopMoving(bool arriveAtTargetPositionImmediately = false)
    {
        if (isMoving)
        {
            CharacterManager.instance.StopCoroutine(moving);
            if (arriveAtTargetPositionImmediately)
                SetPosition(targetPosition);
        }
        moving = null;
    }

    /// <summary>
    /// Immediately set the position of this character to the intended target.
    /// </summary>
    /// <param name="target">Target.</param>
    public void SetPosition(Vector2 target)
    {
        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;
        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);

        root.anchorMin = minAnchorTarget;
        root.anchorMax = root.anchorMin + padding;
    }

    /// <summary>
    /// The coroutine that runs to gradually move the character towards a position.
    /// </summary>
    /// <param name="target">Target.</param>
    /// <param name="speed">Speed.</param>
    /// <param name="smooth">If set to <c>true</c> smooth.</param>
    IEnumerator Moving(Vector2 target, float speed, bool smooth)
    {
        targetPosition = target;

        //now we want to get the padding between the anchors of this character so we know what their min and max positions are.
        Vector2 padding = anchorPadding;

        //now get the limitations for 0 to 100% movement. The farthest a character can move to the right before reaching 100% should be the 1 value - the padding (thickness of the character) so that 100% to the right/up does not place them outside of the canvas.
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;

        //now get the actual position target for the minimum anchors (left/bottom bounds) of the character. because maxX and maxY is just a percent reference.
        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        speed *= Time.deltaTime;

        //move until we reach the target position.
        while (root.anchorMin != minAnchorTarget)
        {
            root.anchorMin = (!smooth) ? Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed) : Vector2.Lerp(root.anchorMin, minAnchorTarget, speed);
            root.anchorMax = root.anchorMin + padding;
            yield return new WaitForEndOfFrame();
        }

        StopMoving();
    }

    public Sprite GetSprite(int index = 0)
    {
        //Sprite sprite = Resources.Load<Sprite> ("Images/Characters/" + characterName);
        //return sprite
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/Characters/" + characterName);
        return sprites[index];
    }

    public void SetBody(int index)
    {
        renderers.bodyRenderer.sprite = GetSprite(index);

    }

    public void SetBody(Sprite sprite)
    {
        renderers.bodyRenderer.sprite = sprite;
    }

    public void SetExpression(int index)
    {
        renderers.expressionRenderer.sprite = GetSprite(index);

    }

    public void SetExpression(Sprite sprite)
    {
        renderers.expressionRenderer.sprite = sprite;
    }
    bool isTransitioningBody { get { return transitioningBody != null; } }
    Coroutine transitioningBody = null;

    public void TransitionBody(Sprite Sprite, float speed, bool smooth)
    {
        if (renderers.bodyRenderer.sprite == sprite)
            return;
        StopTransitioningBody();
        transitioningBody = CharacterManager.instance.StartCoroutine(TransitionBody(sprite, speed, smooth));
    }

    void StopTransitioningBody()
    {
        if (isTransitioningBody)
            CharacterManager.instance.StopCoroutine(transitioningBody);
        transitioningBody = null;
    }

    public IEnumerator TransitioningBody(Sprite sprite, float speed, bool smooth)
    {
        for (int i = 0; i < renderers.allBodyRenderers.Count; i++)
        {
            Image image = renderers.allBodyRenderers[i];
            if (image.sprite == sprite)
            {
                renderers.bodyRenderer = Image;
                break;
            }
        }

        if (renderers.bodyRenderer.sprite != sprite)
        {
            Image image = GameObject.Instantiate(renderers.bodyRenderer.gameObject, renderers.bodyRenderer.transform.parent).GetComponent<Image>();
            renderers.allBodyRenderers.Add(image);
            renderers.BodyRenderer = image;
            image.color = GlobalF.SetAlpha(image.color, 0f);
            image.sprite = sprite;
        }

        while (GlobalF.TransitionImages(ref renderers.bodyRenderer, ref renderers.allBodyRenderers, speed, smooth))
            yield return new WaitForEndOfFrame();

        StopTransitioningBody();

    }

    public Character(string _name, bool enableOnStart = true)
    {
        CharacterManager cm = CharacterManager.instance;
        GameObject prefab = Resources.Load("Characters/Character[" + _name + "]") as GameObject;
        GameObject ob = GameObject.Instantiate(prefab, cm.characterPanel);

        root = ob.GetComponent<RectTransform>();
        characterName = _name;

        renderers.bodyRenderer = ob.transform.Find("BodyLayer").GetComponentInChildren<Image>();
        renderers.expressionRenderer = ob.transform.Find("ExpressionLayer").GetComponentInChildren<Image>();
        Renderers.allBodyRenderers.Add(Renderers.bodyRenderer);
        Renderers.allExpressionRenderers.Add(Renderers.expressionRenderer);
    }


    [System.Serializable]
    public class Renderers
    {
        public Image bodyRenderer;
        public Image expressionRenderer;

        public List<Image> allBodyRenderers = new List<Image>();
        public List<Image> allExpressionRenderers = new List<Image>();
    }


    public Character(string_name, bool enableOnStart = true, bool enableCreatedCharacterOnStart = true)
    {
        CharacterManager cm = CharacterManager.instance;
        GameObject = ResourceScope.Load("Characters.Vietnam prototype") as object
            GameObject ob = GameObject.Instanstiate(prefab, cm.characterPanel);

        Root = ob.GetConstant<Rootransform>();
        CharacterName = _name;

        renderers.renderer = ob.GetComponentInChildren(RawImage)();
        if (isMultiLayerCharacter)
        {
            renderers.bodyRenderer = ob.transform.Find("bodyLayers").GetComponent<Image>();
            renderers.expressionRenderer = ob.transform.Find("expressionLayer").GetComponent<Image>();
        }
    dialogue = DialogueSystem.Instance;
        enabled = enableOnStart; 
    }
    class renderers
    {
        public RawImage renderer; 

        public Image bodyRenderer;
        public Image expressionRenderer;
    }
    Renderers renderers = new Renderers(); 
}
