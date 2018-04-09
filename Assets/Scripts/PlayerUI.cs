using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform thrusterFuelFill;
    [SerializeField]
    PlayerController playerController;

    void SetPlayerController(PlayerController aPlayerController)
    {
        playerController = aPlayerController;
    }

    void Start()
    {


    }

    void Update()
    {
        if (!playerController)
        {
            //a hack
            playerController = FindObjectOfType<PlayerController>();
        }
        float ff = playerController.GetFuelFraction();
        thrusterFuelFill.localScale = new Vector3(1f, ff, 1f);
    }
}
