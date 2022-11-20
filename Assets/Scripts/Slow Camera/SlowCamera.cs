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
    public int raycastsBetweenBorders = 10;

    private void Start()
    {
        cam = Camera.main;
        starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (starterAssetsInputs.fire)
        {
            starterAssetsInputs.FireInput(false);
            GetVisiblePercentage();
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

    /// <summary>
    /// This calculates the cover percentage using RayCasts
    /// </summary>
    /// <param name="camera">The camera viewing the object</param>
    /// <param name="Target">This is the object (obviously it needs a collider)</param>
    /// <returns>0.0=No Visibility, 1.0 = Full Visibility</returns>
    public float GetVisiblePercentage()
    {

        if (murdererCol == null)
            return 0;

        if (Vector3.Distance(murdererCol.transform.position, cam.transform.position) > 3.5f)
        {
            return 0;
        }


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
            for (float x = minX + 1; x < maxX; x += (maxX - minX) / raycastsBetweenBorders)
                for (float y = minY + 1; y < maxY; y += (maxY - minY) / raycastsBetweenBorders)
                {
                    if (y >= maxY || x >= maxX) continue;
                    count++;
                    //Get a Ray from the Screen to to the locaiton in world Space
                    Ray ray = cam.ScreenPointToRay(new Vector3(x, y, 0));
                    if (x < 0 || x > Screen.width || y < 0 || y > Screen.height)
                    {
                        Debug.DrawRay(ray.origin, ray.direction, Color.green, 1f);
                        continue;
                    }

                    //Check if it is a clear him without cover
                    if (Physics.Raycast(ray, out hit, Distance, ~(1 << LayerMask.NameToLayer("Character"))))
                    {
                        if (hit.transform.gameObject == murdererCol.gameObject)
                        {
                            Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);
                            Debug.Log("Hit!");
                            //We hit the target
                            TargetHits++;//Increase hits
                        }
                        else
                        {
                            Debug.DrawRay(ray.origin, ray.direction, Color.green, 1f);
                        }
                    }
                    else
                    {
                        Debug.DrawRay(ray.origin, ray.direction, Color.green, 1f);
                    }
                }



            if (TargetHits > 0)
            {
                Debug.Log((TargetHits / count));
                float percentage = TargetHits / count;
                if (percentage > 0.8f && count > 80) Debug.Log("Valid picture");
                return percentage;
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