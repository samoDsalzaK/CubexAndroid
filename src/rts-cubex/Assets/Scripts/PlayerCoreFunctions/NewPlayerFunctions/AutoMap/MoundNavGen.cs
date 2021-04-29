using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoundNavGen : MonoBehaviour
{
    [SerializeField] List<NavMeshSurface> moundWalks;

    private bool buildNavMesh = false;
    public bool BuildNavMesh { set {buildNavMesh = value;} get {return buildNavMesh; }}
    void Start()
    {
        if (buildNavMesh)
        {
            foreach(var m in moundWalks) 
            {
                m.BuildNavMesh();
            }
        }
    }

    
}
