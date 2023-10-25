using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FurniturePlacementManager : MonoBehaviour
{
    [FormerlySerializedAs("SpawnableFurniture")] [SerializeField]
    private GameObject spawnableFurniture;
    
    [SerializeField]
    private ARSessionOrigin sessionOrigin;
    
    [SerializeField]
    private ARRaycastManager raycastManager;
    
    [SerializeField]
    private ARPlaneManager planeManager;

    private List<ARRaycastHit> _raycastHits;

    private void Awake()
    {
        _raycastHits = new List<ARRaycastHit>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
        {
            if (raycastManager.Raycast(Input.GetTouch(0).position, _raycastHits, TrackableType.PlaneWithinPolygon) && !IsButtonPressed()) 
            {
                Transform _object = Instantiate(spawnableFurniture).transform;
                Pose pose = _raycastHits[0].pose;
                _object.position = pose.position;
                _object.rotation = pose.rotation;
            }

            foreach (ARPlane planesPlane in planeManager.trackables)
            {
                planesPlane.gameObject.SetActive(false);
            }

            planeManager.enabled = false;
        }
    }

    public void SwitchFurniture(GameObject furniture)
    {
        spawnableFurniture = furniture;
    }

    public bool IsButtonPressed()
    {
        return EventSystem.current.currentSelectedGameObject?.GetComponent<Button>() != null;
    }
}
