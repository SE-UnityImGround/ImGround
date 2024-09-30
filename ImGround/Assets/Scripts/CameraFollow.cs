using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 focusOffset = new Vector3(0,2,0); // 카메라가 주목할 타겟의 원점 오프셋
    public float cameraDistance = 5; // 카메라의 거리
    [SerializeField]
    private float accel = 120.0f; // 회전 가속량

    // x회전축의 최소/최대 각도
    private const float MIN_X_ROTATION = 0.0f; 
    private const float MAX_X_ROTATION = 80.0f;

    private const float DECAY_MULTIPLY = 25.0f; // 회전속도 보간 가속량
    private float xVelocity; // x회전축 회전속도(위아래)
    private float yVelocity; // y회전축 회전속도(좌우)

    void Update()
    {
        yVelocity += Input.GetAxisRaw("Mouse X"); // 마우스 X 성분은 좌우 회전을 담당
        xVelocity -= Input.GetAxisRaw("Mouse Y"); // 마우스 Y 성분에 음수 곱하여 방향 반전 (위로 올리면 시점이 위로, 아래로 내리면 시점이 아래로)

        // 좌우 회전 (Y축 회전)
        transform.Rotate(0.0f, yVelocity * accel * Time.deltaTime, 0.0f, Space.World);

        // 위아래 회전 (X축 회전) - 최소/최대 회전 한계를 검사
        if (MIN_X_ROTATION > transform.localRotation.eulerAngles.x + xVelocity * accel * Time.deltaTime)
        {
            transform.rotation = Quaternion.Euler(MIN_X_ROTATION, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            xVelocity = 0.0f;
        }
        else if (MAX_X_ROTATION < transform.localRotation.eulerAngles.x + xVelocity * accel * Time.deltaTime)
        {
            transform.rotation = Quaternion.Euler(MAX_X_ROTATION, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            xVelocity = 0.0f;
        }
        else
        {
            transform.Rotate(xVelocity * accel * Time.deltaTime, 0.0f, 0.0f, Space.Self);
        }

        // 플레이어로부터 카메라의 최종 위치 계산
        transform.position = target.position + focusOffset - (transform.rotation * Vector3.forward * cameraDistance);

        // 선형 보간으로 자연스러운 속도 감소 적용
        xVelocity = Mathf.Lerp(xVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
        yVelocity = Mathf.Lerp(yVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
    }

}
