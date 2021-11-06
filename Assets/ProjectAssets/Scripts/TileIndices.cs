using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileIndices : MonoBehaviour
{
    
    private int x;
    private int y;

    private void Start()
    {
        //y = Convert.ToInt32(name[0]);
        //x = Convert.ToInt32(name[2]);
        y = name[0] - '0';
        x = name[2] - '0';
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

}
