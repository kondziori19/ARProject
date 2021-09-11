using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DisablePlanes : MonoBehaviour
{

    ARPlaneManager m_ARPlaneManager;
    public bool isPlaced;

    void Start()
    {
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
       
    }


    void Update()
    {
        if (isPlaced)
        {
            DisablePlaneManager();
        }     
    }


    void DisablePlaneManager()
    {
        m_ARPlaneManager.enabled = false;     
        foreach (ARPlane plane in m_ARPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
       
    }
}
