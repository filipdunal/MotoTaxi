using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DitzelGames.FastIK
{
    public class DelayFastIK : MonoBehaviour
    {
        void Start()
        {
            FastIKFabric ik=GetComponent<FastIKFabric>();
            if(ik)
            {
                ik.enabled = true;
            }
        }
    }
}

