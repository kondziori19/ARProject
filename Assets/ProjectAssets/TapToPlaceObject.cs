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
    private Pose placementPose;
    private bool placementPoseisValid = false;

    void Start()
    {

        arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }


    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if(placementPoseisValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation * Quaternion.Euler(90f, 0f, 0f));
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

        placementPoseisValid = hits.Count > 0;
        if (placementPoseisValid)
        {
            placementPose = hits[0].pose;
        }

    }
}
