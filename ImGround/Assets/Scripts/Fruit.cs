using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Vector3 respawnPosition; // 리스폰 위치 저장
    private Quaternion respawnRotation;
    void Start()
    {
        // 시작할 때 플레이어의 기본 위치를 현재 위치로 설정 (추가적인 로직이 없는 경우)
        respawnPosition = transform.position;
        respawnRotation = transform.rotation;
    }

    // Update is called once per frame
    public void Respawn()
    {
        transform.position = respawnPosition;
        transform.rotation = respawnRotation;
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();
        rb.useGravity = false;
        rb.isKinematic = false;
        col.enabled = true;
        col.isTrigger = true;
    }
}
