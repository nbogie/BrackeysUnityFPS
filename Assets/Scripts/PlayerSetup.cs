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
    Behaviour[] componentsToDisableForRemotes;
    [SerializeField]
    private string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;
    GameObject playerUIInstance; 

    #region Unity Callbacks
    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponentsForRemotePlayerObjects();
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

            //Disable obstructive player graphics for local player
            //TODO: consider: how bullet impacts still hit this layer.
            int layer = LayerMask.NameToLayer(dontDrawLayerName);
            SetLayerRecursively(playerGraphics.transform, layer);

            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;
        }
        GetComponent<Player>().Setup();
    }
    private void OnDisable()
    {
        Destroy(playerUIInstance);
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

    void SetLayerRecursively(Transform t, int layer)
    {
        Debug.Log("setting layer for " + t.gameObject.name + " to " + layer);
        t.gameObject.layer = layer;
        foreach (Transform otherT in t.transform)
        {
            SetLayerRecursively(otherT, layer);
        }

    }

    private void AssignToRemoteLayer()
    {
        string layerName = "RemotePlayers";
        if ((gameObject.layer = LayerMask.NameToLayer(layerName)) == -1)
        {
            Debug.LogError("Couldn't find layer: " + layerName);
        }

    }

    private void DisableComponentsForRemotePlayerObjects()
    {
        foreach (Behaviour b in componentsToDisableForRemotes)
        {
            Debug.Log("Intentionally disabling: " + b.name + " on remote's player obj");
            b.enabled = false;
        }
    }


}