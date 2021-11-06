using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour
{

    [SerializeField] private string type;
    [SerializeField] private string color;

    public string GetFigureType()
    {
        return type;
    }

    public string GetColor()
    {
        return color;
    }
}
