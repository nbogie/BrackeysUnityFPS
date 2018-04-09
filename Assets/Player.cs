using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private float respawnTime = 4f;

    [SerializeField]
    private int maxHealth = 100;
    [SyncVar]
    private int currentHealth;
    [SyncVar]
    private bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }
    protected void SetIsDead(bool v)
    {
        isDead = v;
    }

    public void Setup()
    {
        RecordComponentStates();

        ResetVariablesForRespawn();

        //ResetComponentStatesOnRespawn();
    }

    private void ResetVariablesForRespawn()
    {
        currentHealth = maxHealth;
        isDead = false;
    }

    private void RecordComponentStates()
    {
        wasEnabled = new bool[componentsToDisableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = componentsToDisableOnDeath[i].enabled;
        }
    }

    private void ResetComponentStatesForRespawn()
    {
        for (int i = 0; i < componentsToDisableOnDeath.Length; i++)
        {
            componentsToDisableOnDeath[i].enabled = wasEnabled[i];
        }
        //special for colliders as they're not behaviours
        Collider _col = GetComponent<Collider>();
        if (_col)
        {
            _col.enabled = true;
        }
    }

    [ClientRpc]
    internal void RpcTakeDamage(int dmg)
    {
        if (IsDead())
        {
            return;
        }

        currentHealth -= dmg;

        Debug.Log("TakeDamage() " + transform.name + " now has " + currentHealth + " health.");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;

        //Disable components (controls, collisions)
        DisableComponentsOnDeath();

        Debug.Log(transform.name + " is DEAD!");

        //Call respawn method
        Invoke("Respawn", GameManager.instance.matchSettings.respawnTime);
    }

    private void Respawn()
    {
        ResetVariablesForRespawn();
        ResetComponentStatesForRespawn();
        Transform  spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
    private void DisableComponentsOnDeath()
    {
        foreach (var c in componentsToDisableOnDeath)
        {
            c.enabled = false;
        }
        Collider coll = GetComponent<Collider>();
        if (coll)
        {
            coll.enabled = false;
        }
    }

    private void OnGUI()
    {
        if (isLocalPlayer)
        {

            GUILayout.BeginArea(new Rect(400, 200, 200, 200));
            GUILayout.BeginVertical();
            GUILayout.Label("Health: " + currentHealth);
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }

}
