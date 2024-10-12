using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstpersonCamera : MonoBehaviour
{
    public Transform target; // ī�޶��� Ÿ��(�÷��̾��� Face)
    public Vector3 focusOffset = new Vector3(0, 0, 0); // ī�޶� Ÿ���� ������ ��ġ�ϵ��� ������ ����
    public float mouseSensitivity = 100f; // ���콺 ����
    public Transform playerBody; // �÷��̾� ��ü (ȸ�� ó����)

    private float xRotation = 0f; // ī�޶��� x�� ȸ�� �� (��/�Ʒ� ����)
    public float moveSpeed = 5f; // �÷��̾��� �̵� �ӵ�

    private void Awake()
    {
        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // ���콺 Ŀ���� ȭ�� �߾ӿ� �����ϰ� ���� ó��
        Cursor.lockState = CursorLockMode.Locked;

        // ���� �ε�� ������ �÷��̾ ã�� Ÿ���� ����
        FindPlayerTarget();
    }

    void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ���� (�޸� ���� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �� �ε� �ø��� �÷��̾ ã�Ƽ� Ÿ�� ����
        FindPlayerTarget();
    }

    void FindPlayerTarget()
    {
        // DontDestroy�� �÷��̾ ã�� Ÿ������ ����
        Player player = Player.GetInstance();
        if (player != null)
        {
            // Mesh �ȿ� �ִ� Face�� Ÿ������ ����
            Transform faceTransform = player.transform.Find("Mesh/Face");
            if (faceTransform != null)
            {
                target = faceTransform;
                playerBody = player.transform; // �÷��̾� ��ü ��ü�� ����
            }
            else
            {
                Debug.LogWarning("Face object not found inside the Player's Mesh!");
            }
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }
    }

    void Update()
    {
        if (target != null)
        {
            // ���콺 �Է��� ����
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // ���Ʒ� ī�޶� ȸ�� (x�� ����)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ��/�Ʒ� ȸ�� ���� ����

            // ī�޶� ȸ�� ���� (ī�޶� ���Ʒ� ȸ��)
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // �÷��̾� ��ü�� �¿� ȸ�� (y�� ����)
            playerBody.Rotate(Vector3.up * mouseX);

            // ī�޶� ��ġ�� �� ��ġ�� ����
            transform.position = target.position + focusOffset;

            // �÷��̾��� �̵� ó��
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        // �÷��̾��� �̵� ó��: WASD Ű�� �յ�, �¿� �̵� ó��
        float moveX = Input.GetAxis("Horizontal"); // A, D �Ǵ� ��, �� ���� �̵�
        float moveZ = Input.GetAxis("Vertical");   // W, S �Ǵ� ����, ����

        // �̵� ���� ���� (��/��, ��/��)
        Vector3 moveDirection = playerBody.transform.right * moveX + playerBody.transform.forward * moveZ;

        // �̵� ó�� (Time.deltaTime ����Ͽ� ������ �������� �̵� ó��)
        playerBody.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
