#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Road)),CanEditMultipleObjects]
public class RoadEditor : Editor
{

}
#endif