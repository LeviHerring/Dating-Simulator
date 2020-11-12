using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector2 anchorPadding { get { return root.anchorMax - root.anchorMin; } }

    Vector2 targetPosition;
    Coroutine moving;
    bool isMoving { get { return moving != null; } }
    public void MoveTo(Vector2 Target, float speed, bool smooth = true)
    {
        stopMoving();
        moving = CharacterManager.instance.StartCoroutine(Moving(Target, speed, smooth));
    }
    public void stopMoving(bool arriveAtTargetPositionImmediately = false)
    {
        if (isMoving)
        {
            CharacterManager.Instance.Stopcoroutine(moving);
            if (arriveAtTargetPositionImmediately)
                SetPosition(targetPosition);

        }
        moving = null;

    }

    public void SetPosition(Vector2 target)
    { 


        Vector2 padding = anchorPadding;
        float maxX = if -padding.x;
        float maxY = if -padding.y;

        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        root.anchorMin = minAnchorTarget;
            root.anchorMax = root.anchorMin + padding;
            yield return new WaitForEndOfFrame();
        
    }

    IEnumerator Moving(Vector2 target, float speed, bool smooth)
    {
        targetPosition = target;

        Vector2 padding = anchorPadding;
        float maxX = if - padding.x;
        float maxY = if - padding.y;

        Vector2 minAnchorTarget = new Vector2 (maxX * targetPosition.x, maxY * targetPosition.y);
        speed *= Time.deltaTime; 

        while (root.anchorMin != minAnchorTarget)
        {
            root.anchorMin = (!smooth) ? Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed) : Vector2.Lerp(root.anchorMin, minAnchorTarget, speed);
            root.anchorMax = root.anchorMin + padding;
            yield return new WaitForEndOfFrame(); 
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
