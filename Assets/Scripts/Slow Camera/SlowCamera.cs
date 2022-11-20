using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using StarterAssets;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SlowCamera : MonoBehaviour
{
    private Camera cam;
    private StarterAssetsInputs starterAssetsInputs;

    public CinemachineVirtualCamera vCamera;
    public Volume volume;
    public Collider murdererCol;
    public int pixelsPerRaycast = 80;

    private void Start()
    {
        cam = Camera.main;
        starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (starterAssetsInputs.fire)
        {
            Debug.Log("Hi");
            starterAssetsInputs.FireInput(false);
            LogVisiblePercentage();
        }
    }


    public void ActivateOldCameraFilters()
    {
        volume.enabled = true;
        vCamera.m_Lens.FieldOfView = 50;
    }

    public void DeactivateOldCameraFilters()
    {
        volume.enabled = false;
        vCamera.m_Lens.FieldOfView = 90;
    }
    public void LogVisiblePercentage()
    {
        Debug.Log("Visible percentage is: " + GetVisiblePercentage());
    }
    /// <summary>
    /// This calculates the cover percentage using RayCasts
    /// </summary>
    /// <param name="camera">The camera viewing the object</param>
    /// <param name="Target">This is the object (obviously it needs a collider)</param>
    /// <returns>0.0=No Visibility, 1.0 = Full Visibility</returns>
    public float GetVisiblePercentage()
    {
        Debug.Log("GetVisiblePercentage");
        if (murdererCol == null)
            return 0;


        float TargetHits = 0;
        float Distance = 1000;

        //Get Max Bounds for an collider
        Vector3 Max = murdererCol.bounds.max;
        Vector3 Min = murdererCol.bounds.min;

        //Project to screen Space
        Vector3 minScreen = cam.WorldToScreenPoint(Min);
        Vector3 maxScreen = cam.WorldToScreenPoint(Max);
        Debug.Log("MinScreen: " + minScreen + ", MaxScreen: " + maxScreen);
        float minX = Mathf.Min(minScreen.x, maxScreen.x);
        float maxX = Mathf.Max(minScreen.x, maxScreen.x);
        float minY = Mathf.Min(minScreen.y, maxScreen.y);
        float maxY = Mathf.Max(minScreen.y, maxScreen.y);

        RaycastHit hit;
        if (murdererCol != null)
        {
            int count = 0;
            //Loop through the screen space coords
            for (float x = minX; x <= maxX; x += pixelsPerRaycast)
                for (float y = minY; y <= maxY; y += pixelsPerRaycast)
                {
                    count++;
                    //Get a Ray from the Screen to to the locaiton in world Space
                    Ray ray = cam.ScreenPointToRay(new Vector3(x, y, 0));

                    //Check if it is a clear him without cover
                    Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);
                    if (Physics.Raycast(ray, out hit, Distance))
                    {
                        if (hit.transform.gameObject == murdererCol.gameObject)
                        {
                            Debug.Log("Hit!");
                            //We hit the target
                            TargetHits++;//Increase hits
                        }
                    }
                }



            if (TargetHits > 0)
            {
                Debug.Log((TargetHits / count));
                return TargetHits / count;
            }
            else
                return 0;
        }

        return 0;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SlowCamera))]
public class SlowCameraEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SlowCamera myScript = (SlowCamera)target;
        if (GUILayout.Button("Activate Old Camera"))
        {
            myScript.ActivateOldCameraFilters();
        }
        if (GUILayout.Button("Deactivate Old Camera"))
        {
            myScript.DeactivateOldCameraFilters();
        }
        if (GUILayout.Button("Percentage"))
        {
            myScript.GetVisiblePercentage();
        }
    }
}
#endif