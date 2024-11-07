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
            throw new System.Exception("NPC ICON : 기본 아이콘이 없습니다!");
        }
        if (selectedMark == null)
        {
            throw new System.Exception("NPC ICON : 선택 상태에서 표시할 아이콘이 없습니다!");
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
    /// 아이콘 위치를 목표 위치로 이동합니다.
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
    /// NPC 아이콘의 위치를 설정합니다.
    /// </summary>
    /// <param name="target">대상 오브젝트의 <see cref="Transform"/></param>
    /// <param name="offset">아이콘이 표시될 위치 오프셋</param>
    public void setPosition(Transform target, Vector3 offset)
    {
        if (target == null)
        {
            throw new System.Exception(nameof(target) + "이 null입니다!");
        }

        this.origin = target;
        this.iconOffset = offset;

    }

    public void show()
    {
        if (origin == null)
        {
            throw new System.Exception("기본 위치가 설정되지 않았습니다!");
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
