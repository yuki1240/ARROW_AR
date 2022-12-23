using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneDetection : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    [SerializeField] GameObject sphere;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        if (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Ended || sphere == null)
        {
            return;
        }

        var hits = new List<ARRaycastHit>();
        // TrackableType.PlaneWithinPolygonを指定することによって検出した平面を対象にできる
        if (raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            // インスタンス化
            Instantiate(sphere, hitPose.position, hitPose.rotation);
        }
    }
}