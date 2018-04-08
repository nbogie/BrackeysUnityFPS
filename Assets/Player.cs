using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{

    [SerializeField]
    private int maxHealth = 100;
    [SyncVar]
    private int currentHealth;

	void Awake()
	{
        SetDefaults();
    }

    private void SetDefaults()
    {
        currentHealth = maxHealth;
    }

	internal void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log("TakeDamage() " + transform.name + " now has " + currentHealth + " health.");
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(400, 200, 200, 200));
        GUILayout.BeginVertical();
        GUILayout.Label("Health: " + currentHealth);
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

}
