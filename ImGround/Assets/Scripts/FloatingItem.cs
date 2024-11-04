using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public float floatAmplitude = 0.1f;  // �������� �������� ���� (���Ʒ� �̵� ����)
    public float floatSpeed = 1f;        // �������� �ӵ�
    public float rotationSpeed = 50f;    // ȸ�� �ӵ�

    private Vector3 startPosition;

    // ������ ��ġ�� �������� �ʱ�ȭ�ϴ� �޼���
    public void Initialize(Vector3 position)
    {
        startPosition = position;
    }

    void Update()
    {
        // �������� ó�� ������ y��ġ�� �������� ���Ʒ��� õõ�� ���ٴϴ� �ִϸ��̼�
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // õõ�� ȸ���ϴ� �ִϸ��̼�
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
