using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpsSystem : MonoBehaviour
{
    public Transform motoNavMeshAgentPrefab;
    Transform player;
    GpsInstance gps;

    private void Start()
    {
        GameObject playerGameObject=GameObject.FindGameObjectWithTag("Motorcycle");
        player = playerGameObject.transform;
        StartCoroutine(Navigate());
    }
    IEnumerator Navigate()
    {
        yield return new WaitForSeconds(1f);
        BeginNavigating(new Vector3(87f,0f,91f));
    }
    void BeginNavigating(Vector3 destination)
    {
        Transform navMeshAgent = player.Find("Moto Navmesh Agent");
        if(navMeshAgent!=null)
        {
            Destroy(navMeshAgent.gameObject);
        }
        navMeshAgent=Instantiate(motoNavMeshAgentPrefab, player);
        gps = navMeshAgent.GetComponent<GpsInstance>();
        gps.SetDestination(destination);
    }

}
