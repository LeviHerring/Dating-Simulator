using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTesting : MonoBehaviour
{
    public float volume, pitch;
    public AudioClip[] clips; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.PlaySFX(clips[Random.Range(0, clips.Length)], volume, pitch);
        }
    }
}
