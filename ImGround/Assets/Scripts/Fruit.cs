using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public Vector3 respawnPosition; // ������ ��ġ ����
    private Quaternion respawnRotation;
    void Start()
    {
        // ������ �� �÷��̾��� �⺻ ��ġ�� ���� ��ġ�� ���� (�߰����� ������ ���� ���)
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
