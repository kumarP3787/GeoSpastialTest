using Google.XR.ARCoreExtensions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GeoTest : MonoBehaviour
{
    [SerializeField]
    private AREarthManager manager;
    [SerializeField]
    private ARRaycastManager raycastManager;
    [SerializeField]
    private GameObject indicator;
    [SerializeField]
    private GameObject indicator2;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text text2;
    [SerializeField]
    private ARAnchorManager anchorManager;
    private GeospatialPose pose;
    //in case of adding to list to remove or change
    private GameObject selected;

    [SerializeField]
    private float latitude,longitude,altitude;
    [SerializeField]
    private InputField _latitude, _longitude, _altitude;
    [SerializeField]
    private Quaternion quaternion;

    // Start is called before the first frame update
    private void Start()
    {
       
    }

    public void InputValues()
    {
        latitude = float.Parse(_latitude.text);
        longitude = float.Parse(_longitude.text);
        altitude= float.Parse(_altitude.text);
    }
    public void placement()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(new Vector2(Screen.width / 2f, Screen.height / 2.5f), hits, TrackableType.PlaneWithinPolygon);
        if (hits.Count > 0)
        {
            if (raycastManager.enabled)
            {
                indicator.transform.SetPositionAndRotation(hits[0].pose.position, hits[0].pose.rotation);
                indicator.SetActive(true);
                pose =  manager.Convert(hits[0].pose);
            }
        }
    }
    public void CheckTouchAndSpawnPrefeb()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            selected = Instantiate(indicator,indicator.transform.position, indicator.transform.rotation) as GameObject;
            raycastManager.enabled = false;   
        }
    }

    public void geoAnchor()
    {
        if (manager.EarthTrackingState == TrackingState.Tracking)
        {
            var anchor =
                anchorManager.AddAnchor(
                    latitude,
                    longitude,
                    altitude,
                    quaternion);
            var anchoredAsset = Instantiate(indicator2, anchor.transform);
        }
    }


    public void Restart()
    {
        SceneManager.LoadScene(0);    
    }

    // Update is called once per frame
    void Update()
    {      
        var earthTrackingState = manager.EarthTrackingState;
        if (earthTrackingState == TrackingState.Tracking)
        {
            var pose2 = manager.CameraGeospatialPose;
            placement();
            CheckTouchAndSpawnPrefeb();
            text2.text = pose2.Latitude + " " + pose2.Longitude + " " + pose2.Altitude;
            text.text = pose.Latitude + " " + pose.Longitude + " " + pose.Altitude;
        }
        else if (earthTrackingState == TrackingState.None)
        {
            Debug.Log("None");
        }
    }
}
