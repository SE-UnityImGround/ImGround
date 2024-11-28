using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSounds : MonoBehaviour
{
    public AudioSource[] effectSound;

    public GameObject player; // 플레이어 오브젝트
    public static Vector3 minBounds = new Vector3(-78, -10, -120); // x, y, z 최소값
    public static Vector3 maxBounds = new Vector3(183, 20, 148);

    // Update is called once per frame
    void Update()
    {
        bool isWithinBounds = IsPlayerWithinBounds();

        if (!isWithinBounds)
        {
            if (!effectSound[0].isPlaying)
            {
                effectSound[0].Play();
            }            
        }
        else
        {
            if (effectSound[0].isPlaying)
            {
                effectSound[0].Stop();
            }
        }

    }
    private bool IsPlayerWithinBounds()
    {
        if (player == null) return false;

        Vector3 playerPosition = player.transform.position;
        return playerPosition.x >= minBounds.x && playerPosition.x <= maxBounds.x &&
               playerPosition.y >= minBounds.y && playerPosition.y <= maxBounds.y &&
               playerPosition.z >= minBounds.z && playerPosition.z <= maxBounds.z;
    }
}
