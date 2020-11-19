﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerTesting : MonoBehaviour
{
    BCFC controller;

    public Texture tex;
    public MovieTexture mov;
    void Start()
    {
        BCFC.instance;  
    }

    // Update is called once per frame
    void Update()
    {
        BCFC.LAYER layer = null;
        if (Input.GetKey(KeyCode.Q))
            layer = controller.background;
        if (Input.GetKey(KeyCode.W))
            layer = controller.cinematic;
        if (Input.GetKey(KeyCode.E))
            layer = controller.foreground; 

        if (Input.GetKey(KeyCode.T))
        {

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
                layer.SetTexture(tex);
            else if (Input.GetKeyDown(KeyCode.S))
                layer.SetTexture(mov);
        }
    }
}
