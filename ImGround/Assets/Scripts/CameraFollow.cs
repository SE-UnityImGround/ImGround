/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 focusOffset = new Vector3(0,2,0); // ī�޶� �ָ��� Ÿ���� ���� ������
    public float cameraDistance = 5; // ī�޶��� �Ÿ�
    [SerializeField]
    private float accel = 120.0f; // ȸ�� ���ӷ�

    // xȸ������ �ּ�/�ִ� ����
    private const float MIN_X_ROTATION = 0.0f; 
    private const float MAX_X_ROTATION = 80.0f;

    private const float DECAY_MULTIPLY = 25.0f; // ȸ���ӵ� ���� ���ӷ�
    private float xVelocity; // xȸ���� ȸ���ӵ�(���Ʒ�)
    private float yVelocity; // yȸ���� ȸ���ӵ�(�¿�)

    void Update()
    {
        yVelocity += Input.GetAxisRaw("Mouse X"); // ���콺 X ������ �¿� ȸ���� ���
        xVelocity -= Input.GetAxisRaw("Mouse Y"); // ���콺 Y ���п� ���� ���Ͽ� ���� ���� (���� �ø��� ������ ����, �Ʒ��� ������ ������ �Ʒ���)

        // �¿� ȸ�� (Y�� ȸ��)
        transform.Rotate(0.0f, yVelocity * accel * Time.deltaTime, 0.0f, Space.World);

        // ���Ʒ� ȸ�� (X�� ȸ��) - �ּ�/�ִ� ȸ�� �Ѱ踦 �˻�
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

        // �÷��̾�κ��� ī�޶��� ���� ��ġ ���
        transform.position = target.position + focusOffset - (transform.rotation * Vector3.forward * cameraDistance);

        // ���� �������� �ڿ������� �ӵ� ���� ����
        xVelocity = Mathf.Lerp(xVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
        yVelocity = Mathf.Lerp(yVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
    }

}
*/

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 focusOffset = new Vector3(0, 2, 0); // ī�޶� �ָ��� Ÿ���� ���� ������
    public float cameraDistance = 5; // ī�޶��� �Ÿ�
    [SerializeField]
    private float accel = 120.0f; // ȸ�� ���ӷ�

    // xȸ������ �ּ�/�ִ� ����
    private const float MIN_X_ROTATION = 0.0f;
    private const float MAX_X_ROTATION = 80.0f;

    private const float DECAY_MULTIPLY = 25.0f; // ȸ���ӵ� ���� ���ӷ�
    private float xVelocity; // xȸ���� ȸ���ӵ�(���Ʒ�)
    private float yVelocity; // yȸ���� ȸ���ӵ�(�¿�)

    private static CameraFollow instance;

    void Awake()
    {
        // ī�޶� �ߺ� ���� �� DontDestroyOnLoad ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʰ� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� ī�޶� �ı�
        }

        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // �� �ε� �̺�Ʈ ���� (�޸� ���� ����)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerTarget();
    }

    void FindPlayerTarget()
    {
        // �÷��̾� ������Ʈ�� ���̾�� ã�Ƽ� Ÿ�� ����
        GameObject[] players = FindObjectsOfType<GameObject>();

        foreach (GameObject player in players)
        {
            if (player.layer == LayerMask.NameToLayer("Player"))
            {
                target = player.transform;
                return;
            }
        }

        Debug.LogWarning("Player object not found!");
    }

    void Update()
    {
        if (target != null)
        {
            yVelocity += Input.GetAxisRaw("Mouse X"); // ���콺 X ������ �¿� ȸ���� ���
            xVelocity -= Input.GetAxisRaw("Mouse Y"); // ���콺 Y ���п� ���� ���Ͽ� ���� ���� (���� �ø��� ������ ����, �Ʒ��� ������ ������ �Ʒ���)

            // �¿� ȸ�� (Y�� ȸ��)
            transform.Rotate(0.0f, yVelocity * accel * Time.deltaTime, 0.0f, Space.World);

            // ���Ʒ� ȸ�� (X�� ȸ��) - �ּ�/�ִ� ȸ�� �Ѱ踦 �˻�
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

            // �÷��̾�κ��� ī�޶��� ���� ��ġ ���
            transform.position = target.position + focusOffset - (transform.rotation * Vector3.forward * cameraDistance);

            // ���� �������� �ڿ������� �ӵ� ���� ����
            xVelocity = Mathf.Lerp(xVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
            yVelocity = Mathf.Lerp(yVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
        }
    }
}
*/




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 focusOffset = new Vector3(0, 2, 0); // ī�޶� �ָ��� Ÿ���� ���� ������
    public float cameraDistance = 5; // ī�޶��� �Ÿ�
    [SerializeField]
    private float accel = 120.0f; // ȸ�� ���ӷ�

    // xȸ������ �ּ�/�ִ� ����
    private const float MIN_X_ROTATION = 0.0f;
    private const float MAX_X_ROTATION = 80.0f;

    private const float DECAY_MULTIPLY = 25.0f; // ȸ���ӵ� ���� ���ӷ�
    private float xVelocity; // xȸ���� ȸ���ӵ�(���Ʒ�)
    private float yVelocity; // yȸ���� ȸ���ӵ�(�¿�)

    private Player player;

    void Awake()
    {
        // �� �ε� �̺�Ʈ ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
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
        player = Player.GetInstance();
        if (player != null)
        {
            target = player.transform;
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
            yVelocity += Input.GetAxisRaw("Mouse X"); // ���콺 X ������ �¿� ȸ���� ���
            xVelocity -= Input.GetAxisRaw("Mouse Y"); // ���콺 Y ���п� ���� ���Ͽ� ���� ���� (���� �ø��� ������ ����, �Ʒ��� ������ ������ �Ʒ���)

            // �¿� ȸ�� (Y�� ȸ��)
            transform.Rotate(0.0f, yVelocity * accel * Time.deltaTime, 0.0f, Space.World);

            // ���Ʒ� ȸ�� (X�� ȸ��) - �ּ�/�ִ� ȸ�� �Ѱ踦 �˻�
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

            // �÷��̾�κ��� ī�޶��� ���� ��ġ ���
            transform.position = target.position + focusOffset - (transform.rotation * Vector3.forward * cameraDistance);

            // ���� �������� �ڿ������� �ӵ� ���� ����
            xVelocity = Mathf.Lerp(xVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
            yVelocity = Mathf.Lerp(yVelocity, 0.0f, Time.deltaTime * DECAY_MULTIPLY);
        }
    }
}
