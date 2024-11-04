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
    
    private float heightPos = 0.0f;

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
            heightPos = HEIGHT_AMPLITUDE * Mathf.Sin(Time.time / HEIGHT_PERIOD * 2 * Mathf.PI);

            transform.position = origin.transform.position;
            foreach (GameObject obj in icons)
                obj.transform.localPosition = iconOffset + new Vector3(0, heightPos, 0);
            transform.Rotate(0, Time.deltaTime * ROTATE_VELOCITY, 0);
        }
    }

    public void show(Transform target, Vector3 offset)
    {
        if (target == null)
        {
            throw new System.Exception(nameof(target) + "이 null입니다!");
        }

        this.origin = target;
        this.iconOffset = offset;

        isVisible = true;
        gameObject.SetActive(true);
    }

    public void hide()
    {
        this.origin = null;

        isVisible = false;
        gameObject.SetActive(false);
    }

    public void setSelected(bool isSelected)
    {
        selectedMark.SetActive(isSelected);
    }
}
