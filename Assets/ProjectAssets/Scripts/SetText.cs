using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetText : MonoBehaviour
{
    public string textVal;
    public Text textElem;

    // Start is called before the first frame update
    void Start()
    {
        textElem.text = textVal;
    }

    // Update is called once per frame
    void Update()
    {
        textElem.text = textVal;
    }
}
