#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Road)),CanEditMultipleObjects]
public class RoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Refresh lines"))
        {
            foreach(GameObject go in Selection.gameObjects)
            {
                go.GetComponent<Road>().CreateLines();
            }
        }
    }
}
#endif