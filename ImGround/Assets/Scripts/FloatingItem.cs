using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public float floatAmplitude = 0.1f;  // 아이템이 떠오르는 높이 (위아래 이동 범위)
    public float floatSpeed = 1f;        // 떠오르는 속도
    public float rotationSpeed = 50f;    // 회전 속도

    private Vector3 startPosition;

    // 생성된 위치를 기준으로 초기화하는 메서드
    public void Initialize(Vector3 position)
    {
        startPosition = position;
    }

    void Update()
    {
        // 아이템이 처음 생성된 y위치를 기준으로 위아래로 천천히 떠다니는 애니메이션
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // 천천히 회전하는 애니메이션
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
