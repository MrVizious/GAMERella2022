using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SlowCamera : MonoBehaviour
{
    public CinemachineVirtualCamera vCamera;
    public Volume volume;

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
}

#if UNITY_EDITOR
[CustomEditor(typeof(SlowCamera))]
public class ObjectBuilderEditor : Editor
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
    }
}
#endif