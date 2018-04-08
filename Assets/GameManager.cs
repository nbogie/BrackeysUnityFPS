using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    private static Dictionary<string, Player> playerDict = new Dictionary<string, Player>();

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();
        foreach (string playerID in playerDict.Keys)
        {
            GUILayout.Label(playerID + " - " + playerDict[playerID].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

    }
    public static Player GetPlayer(string playerID)
    {
        return playerDict[playerID];
    }

    public static void RegisterPlayer(NetworkInstanceId _netID, Player _player)
    {
        string _playerID = "Player " + _netID;
        playerDict.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    internal static void UnRegisterPlayer(string playerName)
    {
        playerDict.Remove(playerName);
    }

}
