using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCIconBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject[] icons;
    [SerializeField]
    private GameObject selectedMark;

    private static readonly float HEIGHT_AMPLITUDE = 0.2f;
    private static readonly float HEIGHT_PERIOD = 1.0f;
    private static readonly float ROTATE_VELOCITY = 360.0f;

    private Transform origin;
    private Vector3 iconOffset;

    private bool isVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        if (icons == null)
        {
            throw new System.Exception("NPC ICON : �⺻ �������� �����ϴ�!");
        }
        if (selectedMark == null)
        {
            throw new System.Exception("NPC ICON : ���� ���¿��� ǥ���� �������� �����ϴ�!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isVisible)
        {
            setPosition();
        }
    }

    /// <summary>
    /// ������ ��ġ�� ��ǥ ��ġ�� �̵��մϴ�.
    /// </summary>
    private void setPosition()
    {
        transform.position = origin.transform.position;
        float heightPos = HEIGHT_AMPLITUDE * Mathf.Sin(Time.time / HEIGHT_PERIOD * 2 * Mathf.PI);
        foreach (GameObject obj in icons)
            obj.transform.localPosition = iconOffset + new Vector3(0, heightPos, 0);
        transform.Rotate(0, Time.deltaTime * ROTATE_VELOCITY, 0);
    }

    /// <summary>
    /// NPC �������� ��ġ�� �����մϴ�.
    /// </summary>
    /// <param name="target">��� ������Ʈ�� <see cref="Transform"/></param>
    /// <param name="offset">�������� ǥ�õ� ��ġ ������</param>
    public void setPosition(Transform target, Vector3 offset)
    {
        if (target == null)
        {
            throw new System.Exception(nameof(target) + "�� null�Դϴ�!");
        }

        this.origin = target;
        this.iconOffset = offset;

    }

    public void show()
    {
        if (origin == null)
        {
            throw new System.Exception("�⺻ ��ġ�� �������� �ʾҽ��ϴ�!");
        }

        isVisible = true;
        setPosition();
        gameObject.SetActive(isVisible);
    }

    public void hide()
    {
        isVisible = false;
        gameObject.SetActive(isVisible);
    }

    public void setSelected(bool isSelected)
    {
        selectedMark.SetActive(isSelected);
    }
}
