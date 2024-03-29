using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


using System;

public class TapToPlaceObject : MonoBehaviour
{
   
    public GameObject objectToPlace;
    public GameObject placementIndicator;
    private ARRaycastManager arRaycastManager;
    public Pose placementPose;
    private bool placementPoseisValid = false;
    public bool isObjectPlaced = false;
    ARPlaneManager m_ARPlaneManager;
    ARAnchorManager m_ARAnchorManager;

    void Start()
    {
        m_ARPlaneManager = GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>();
        arRaycastManager = FindObjectOfType<ARRaycastManager>();

    }


    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if(placementPoseisValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !isObjectPlaced)
        {
            PlaceObject();
        }
  
    }

    private void PlaceObject()
    {
        isObjectPlaced = true;
        GameObject placedObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation * Quaternion.Euler(-90f, 0f, 0f));
        

      //  ARAnchor anchor =  m_ARAnchorManager.AddAnchor(placementPose);
     //   placedObject.transform.parent = anchor.transform;

        m_ARPlaneManager.enabled = false;
        foreach (ARPlane plane in m_ARPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        placementIndicator.SetActive(false);
        gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseisValid)
        {
            placementIndicator.SetActive(true); // setting visibility of Game Object to true
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation); // setting rotation of Game Object
        }
        else
        {
            placementIndicator.SetActive(false); // setting visibility of Game Object to false
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);

        if(hits.Count > 0) placementPoseisValid = true;
         
        if (placementPoseisValid)
        {
            placementPose = hits[0].pose;
        }

    }

}
