using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    int _points;

    // Use this for initialization
    void Start()
    {
        points = 10;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int points
    {
        get
        {
            Debug.Log("Points returned " + _points);
            return _points;
        }
        set { _points = value; }
    }
}

