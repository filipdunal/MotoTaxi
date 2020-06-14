using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Road : MonoBehaviour
{
    [Header("Don't modify on instance level")]
    public RoadType roadType;
    
    [Space(20)]
    public LinesType linesType;
    LinesType temp;
    Transform linesLocalFolder;
    
    public void CreateLines(LinesType type)
    {
        EmptyRoad();
        switch(type)
        {
            case LinesType.None:
                break;
            case LinesType.Dotted:
                CreateDottedLines();
                break;
            case LinesType.OneContinous:
                CreateOneContinousLine();
                break;
            case LinesType.DoubleContinous:
                CreateDoubleContinousLine();
                break;
            default:
                break;
        }
    }

    void CreateDottedLines()
    {
        switch(roadType)
        {
            case RoadType.Straight:
                Instantiate(transform.parent.GetComponent<RoadLinesFolder>().dottedLineStraight, transform.GetChild(0));
                break;
            case RoadType.Turn:
                Instantiate(transform.parent.GetComponent<RoadLinesFolder>().dottedLineTurn, transform.GetChild(0));
                break;
            default:
                break;
        }
    }

    void CreateOneContinousLine()
    {
        switch(roadType)
        {
            case RoadType.Straight:
                Instantiate(transform.parent.GetComponent<RoadLinesFolder>().oneContinousStraight, transform.GetChild(0));
                break;
            default:
                break;
        }
    }

    void CreateDoubleContinousLine()
    {
        switch (roadType)
        {
            case RoadType.Straight:
                Instantiate(transform.parent.GetComponent<RoadLinesFolder>().doubleContinousStraight, transform.GetChild(0));
                break;
            default:
                break;
        }
    }

    public void CreateLines()
    {
        CreateLines(linesType);
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
