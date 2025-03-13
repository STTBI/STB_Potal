using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject player { get; private set; }

    public GameObject CreatePlayer(Quaternion quater,Vector3 originVelocity, float x = 0f, float y = 0f, float z = 0f)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        GameObject prefabs = ResourceManager.Instance.GetResource<GameObject>("player");
        GameObject objPlayer = Instantiate(prefabs, newPosition, quater);
        objPlayer.transform.position = newPosition;
        //objPlayer.GetComponent<PlayerController>().MoveMent.playerVelocity = originVelocity;
        return objPlayer;
    }

    public GameObject ChangePlayerOwner(GameObject playerObject)
    {
        if (playerObject == null || player == playerObject)
        {
            Debug.LogError("[GameManager] Player owner change failed");
            return null;
        }

        player = playerObject;
        return player;
    }
}
