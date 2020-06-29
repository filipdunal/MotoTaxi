using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GpsInstance : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    LineRenderer line;
    public float lineHeight=0f;
    public Vector3 agentPosition;
    public float destinationReachMargin;
    bool keepDestinationPoint;

    bool canDestroyNow;

    public void SetDestination(Vector3 destination, bool keepDestinationPointNearIt=false)
    {
        keepDestinationPoint = keepDestinationPointNearIt;
        navMeshAgent = GetComponent<NavMeshAgent>();
        line = GetComponentInChildren<LineRenderer>();
        navMeshAgent.SetDestination(destination);
        StartCoroutine(DelayDestinationDetection());
    }

    private void OnDrawGizmos()
    {
        if (navMeshAgent == null || navMeshAgent.path == null)
            return;
        
        var path = navMeshAgent.path;

        line.positionCount = path.corners.Length;

        for (int i = 0; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]+new Vector3(0f,lineHeight,0f));
        }
    }

    IEnumerator DelayDestinationDetection()
    {
        yield return new WaitForSeconds(1f);
        canDestroyNow = true;

    }
    private void FixedUpdate()
    {
        transform.localPosition = agentPosition;
        if (!keepDestinationPoint && destinationReached)
        {
            Destroy(gameObject);
        }
    }

    bool destinationReached
    {
        get { return canDestroyNow && Vector3.Distance(transform.position, navMeshAgent.destination) < destinationReachMargin; }
    }

}
