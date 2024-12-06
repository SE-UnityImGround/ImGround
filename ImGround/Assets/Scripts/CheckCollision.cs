using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    public bool isColliding = false;

    void OnTriggerEnter(Collider other)
    {
        // 태그 조건 없이 모든 충돌체와의 충돌 감지
        isColliding = true;
    }

    void OnTriggerExit(Collider other)
    {
        isColliding = false; // 충돌 종료
    }
}