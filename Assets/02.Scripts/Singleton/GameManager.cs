using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player { get; set; }

    public PlayerController CreatePlayer(float x = 0f, float y = 0f, float z = 0f)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        GameObject prefabs = ResourceManager.Instance.GetResource<GameObject>("Player");
        GameObject objPlayer = Instantiate(prefabs, newPosition, Quaternion.identity);
        objPlayer.transform.position = newPosition;
        return objPlayer.AddComponent<PlayerController>();
    }

    public PlayerController ChangePlayerOwner(PlayerController playerObject)
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
