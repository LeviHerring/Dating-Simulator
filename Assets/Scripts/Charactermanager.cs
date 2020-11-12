using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactermanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public class CHARACTERPOSITIONS
    {
        public Vector2 bottomleft = new Vector2(0, 0);
        public Vector2 topRight = new Vector2(1f, 1f);
        public Vector2 centre = new Vector2(0.5f, 0.5f);
        public Vector2 bottomRight = new Vector2(1f, 0);
        public Vector2 topLeft = new Vector2(0, 1f); 
    }
    public static CHARACTERPOSITIONS characterPositions = new CHARACTERPOSITIONS();    
}
