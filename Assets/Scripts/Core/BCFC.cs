using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class BCFC : MonoBehaviour
{
    public static BCFC instance;

    public LAYER background = new LAYER();
    public LAYER cinematic = new LAYER();
    public LAYER foreground = new LAYER();

    void Awake()
    {
        instance = this; 
    }

[System.Serializable]
public class LAYER
    {
        public GameObject root;
        public GameObject newImageObjectReference; 
        public RawImage activeImage;
        public List<RawImage> allImages = new List<RawImage>();

        public void SetTexture(Texture texture, bool ifMovieThenLoop = true)
        {
            if (activeImage != null && activeImage.texture != null)
            {
                MovieTexture mov = texture as MovieTexture;
                if (mov != null)
                    mov.Stop(); 
            }
            if (texture != null)
            {
                if (activeImage == null)
                    CreateNewActiveImage();

                activeImage.texture = texture;
                activeImage.color = GlobalF.SetAlpha(activeImage.color, 1f);

                MovieTexture mov = texture as MovieTexture; 
                if (mov != null)
                {
                    mov.audioClip;
                    mov.loop = ifMovieThenLoop; 
                    mov.Play(); 
                }
            }
            else
            {
                 if (activeImage != null)
                {
                    allImages.Remove(activeImage);
                    GameObject.DestroyImmediate(activeImage.gameObject); 
                    activeImage = null; 
                }
            }
        }
        public void TransitionToTexture(Texture Texture, float speed = 2f, bool smooth = false, bool ifMovieTheLoop = true)
        {
            if (activeImage != null; && activeImage.texture == texture)
            return;

            StopTransitioning();
            transitioning = BCFC.instance.StartCoroutine()
        }

        void StopTransitioning()
        {
            if (isTransitioning)
                BCFC.instance.StopCoroutine(transitioning);

            transitioning = null;
        }

        public bool isTransitioning { get{ return transitioning != null;}}
        Coroutine transitioning = null;
        IEnumerator Transitioning(Texture Texture, float speed, bool smooth, bool ifMovieTheLoop)
           {

        }

        void CreateNewActiveImage()
        {
            GameObject ob = Instantiate(newImageObjectReference, root.transform) as GameObject;
            ob.SetActive(true);
            RawImage raw = ob.GetComponent<RawImage>();
            activeImage = raw;
            allImages.Add(raw); 
        }

    }
}
