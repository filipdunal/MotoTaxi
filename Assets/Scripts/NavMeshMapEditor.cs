#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NavMeshMap))]
public class NavMeshMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        NavMeshMap script = (NavMeshMap)target;
        if (GUILayout.Button("Build map based on Nav Mesh"))
        {
            script.RenderMap();
        }
        if (GUILayout.Button("Erase map"))
        {
            script.EraseMap();
        }
    }
}
#endif
