using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;

    private MeshRenderer meshRenderer;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        //시작하자마자 매시 랜더러를 지워 색깔 안보이게 하기 
        Hide();
    }
    private void OnTriggerEnter(Collider other)
    {
        //   Debug.Log(other.GetComponentInParent<VehicleControl>().name + "");
        if (other.GetComponentInParent<VehicleControl>()){
            trackCheckpoints.CarThroughCheckpoint(this, other.GetComponentInParent<VehicleControl>().transform);
        }        
        else
            Debug.LogWarning("Warning: Collider detecting Fail");        
    }

    public void SetTrackCheckpoints(TrackCheckpoints trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }


    public void Show()
    {
        meshRenderer.enabled = true;
    }
    public void Hide()
    {
        meshRenderer.enabled = false;
    }
    

    
}
