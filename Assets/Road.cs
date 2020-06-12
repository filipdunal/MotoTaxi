using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Road : MonoBehaviour
{
    public LinesType linesType;
    LinesType temp;
    Transform linesLocalFolder;

    private void Awake()
    {
    }
    public void CreateLines(LinesType type)
    {
        EmptyRoad();
        switch(type)
        {
            case LinesType.None:
                break;
            case LinesType.Dotted:
                Instantiate(transform.parent.GetComponent<RoadLinesFolder>().dottedLineStraight,transform.GetChild(0));
                break;
            case LinesType.OneContinous:
                break;
            case LinesType.DoubleContinous:
                break;
            default:
                break;
        }
    }

    void EmptyRoad()
    {
       foreach(Transform children in transform.GetChild(0))
       {
            DestroyImmediate(children.gameObject);
       }
    }
    private void Update()
    {
        if(temp!=linesType)
        {
            temp = linesType;
            CreateLines(linesType);
        }
    }
}
