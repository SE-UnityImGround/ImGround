using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    public bool isColliding = false;

    void OnTriggerEnter(Collider other)
    {
        // �±� ���� ���� ��� �浹ü���� �浹 ����
        isColliding = true;
    }

    void OnTriggerExit(Collider other)
    {
        isColliding = false; // �浹 ����
    }
}