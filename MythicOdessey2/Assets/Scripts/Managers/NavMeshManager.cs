using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


public class NavMeshManager : MonoBehaviour {
    [SerializeField] private NavMeshSurface _meshSurface;
    [SerializeField] private NavMeshData _navMeshData;
    private void OnEnable(){
        //_navMeshData = _meshSurface.navMeshData;
        EventManager.instance.AddAction("OnUpdateNavMesh",(objects => {
            UpdateNavMesh();
        } ));
       // EventManager.instance.TriggerEvent("OnUpdateNavMesh");
    }

    // private void Update(){
    //     if(Input.GetKeyDown(KeyCode.N))
    //         UpdateNavMesh();
    // }

    void UpdateNavMesh(){
        _meshSurface.BuildNavMesh();
    }

    // private void OnDisable(){
    //     EventManager.instance.RemoveAction("OnUpdateNavMesh",(objects => {
    //         NavMeshBuilder.BuildNavMesh(); //UpdateNavMeshData(_navMeshData, NavMesh.GetSettingsByID(0), _navMeshBuildSource);
    //     } ));
    // }
}

