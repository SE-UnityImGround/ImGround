using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPCController : MonoBehaviour
{
    private static readonly float MAX_INTERACTION_DISTANCE = 5.0f;
    private static readonly float MAX_INTERACTION_ANGLE = 70.0f;

    private GameObject selected;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject selectedNPC = findInteractableNPC();
        if (selected != selectedNPC)
        {
            if (selected != null)
            {
                // 이전 선택된 NPC 처리
                selected.GetComponent<NPCBehavior>().setSelected(false);
                Debug.Log("취소됨 : " + selected.name);
            }
            if (selectedNPC != null)
            {
                // 새로 선택된 NPC 처리
                selectedNPC.GetComponent<NPCBehavior>().setSelected(true);
                Debug.Log("새로 선택됨 : " + selectedNPC.name);
            }
            selected = selectedNPC;
        }
    }

    private GameObject findInteractableNPC()
    {
        float minDistance = float.MaxValue;
        GameObject selectedNpc = null;
        foreach (NPCBehavior npc in FindObjectsByType<NPCBehavior>(FindObjectsSortMode.None))
        {
            Vector3 deltaAngle = Quaternion.LookRotation(npc.transform.position - transform.position).eulerAngles
                                - transform.rotation.eulerAngles;
            float distanceSquare = (npc.transform.position - transform.position).sqrMagnitude;

            if (Mathf.Abs(deltaAngle.y) < MAX_INTERACTION_ANGLE
                && distanceSquare < MAX_INTERACTION_DISTANCE * MAX_INTERACTION_DISTANCE
                && distanceSquare < minDistance)
            {
                selectedNpc = npc.gameObject;
                minDistance = distanceSquare;
            }
        }
        return selectedNpc;
    }
}
