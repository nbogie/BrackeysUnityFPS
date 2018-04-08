using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    private Camera sceneCamera;

    [SerializeField]
    Behaviour[] componentsToDisable;

    #region Unity Callbacks
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignToRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera)
            {
                Debug.Log("disable scene camera");
                sceneCamera.gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        if (sceneCamera)
        {
            Debug.Log("reactivate scene camera");
            sceneCamera.gameObject.SetActive(true);
        }
        GameManager.UnRegisterPlayer(transform.name);


    }
    public override void OnStartClient()
    {

        base.OnStartClient();

        GameManager.RegisterPlayer(GetComponent<NetworkIdentity>().netId,
                                   GetComponent<Player>());
    }
    #endregion

    private void AssignToRemoteLayer()
    {
        string layerName = "RemotePlayers";
        if ((gameObject.layer = LayerMask.NameToLayer(layerName)) == -1)
        {
            Debug.LogError("Couldn't find layer: " + layerName);
        }

    }

    private void DisableComponents()
    {
        foreach (Behaviour b in componentsToDisable)
        {
            Debug.Log("Intentionally disabling: " + b.name);
            b.enabled = false;
        }
    }


}