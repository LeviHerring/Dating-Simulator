using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emotions : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite GetSprite(int index = 0)
    {
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
    bool isTransitioningBody {get { transitioningBody != null; } }
    Coroutine transitioningBody = null; 

    public void TransitionBody(Sprite Sprite, float speed, bool smooth)
    {
        if (renderers.bodyRenderer.sprite == sprite)
            return;
        StopTransitioningBody();
        transitioningBody = CharacterManager.instance.StartCoroutine(TransitionBody(sprite, speed, smooth  ));
    }

    void StopTransitioningBody()
    {
        if (isTransitioningBody)
            CharacterManager.instance.StopCoroutine (transitioningBody);
        transitioningBody = null; 
    }

    public IEnumerator TransitioningBody(Sprite sprite, float speed, bool smooth)
    {
        for (int i = 0; i < renderers.allBodyRenderers.Count; i++)
        {
            Image image = renderers.allBodyRenderers [i];
            if (image.sprite == sprite )
            {
                renderers.bodyRenderer = Image;
                break; 
            }
        }
    }

    public Character (string _name, bool enableOnStart = true)
    {
        CharacterManager cm = CharacterManager.instance;
        GameObject prefab = Resources.Load("Characters/Character[" + _name + "]") as GameObject;
        GameObject ob = GameObject.Instantiate(prefa, cm.characterPanel);

        root = ob.GetComponent<RectTransform>();
        characterName = _name;

        renderers.bodyRenderer = ob.transform.Find("bodyLayer").GetComponent<Image>();
        renderers.expressionRenderer = ob.transform.Find("expressionLayer").GetComponent<Image>();
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
}
