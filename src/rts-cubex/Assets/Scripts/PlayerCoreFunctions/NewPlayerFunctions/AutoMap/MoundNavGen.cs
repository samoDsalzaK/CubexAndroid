using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoundNavGen : MonoBehaviour
{
    [SerializeField] List<NavMeshSurface> moundWalks;
    void Start()
    {
       foreach(var m in moundWalks) 
       {
           m.BuildNavMesh();
       }
    }

    
}
