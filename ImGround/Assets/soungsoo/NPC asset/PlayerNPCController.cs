using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPCController : MonoBehaviour
{
    private static readonly float MAX_INTERACTION_DISTANCE = 5.0f;
    private static readonly float MAX_INTERACTION_ANGLE = 70.0f;

    private GameObject selectedNPC;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        selectInteractableNPC();
        if (Input.GetKeyDown(KeyCode.F) && selectedNPC != null)
        {
            Debug.Log("선택된 npc : " + selectedNPC.name + " 타입 : " + selectedNPC.GetComponent<NPCBehavior>().type);
        }
    }

    /// <summary>
    /// 상호작용 가능한 NPC를 탐색합니다.
    /// 속성 <see cref="selectedNPC"/>에 저장됩니다.
    /// </summary>
    private void selectInteractableNPC()
    {
        GameObject selectedNPC = findInteractableNPC();
        if (this.selectedNPC != selectedNPC)
        {
            if (this.selectedNPC != null)
            {
                // 이전 선택된 NPC 처리
                this.selectedNPC.GetComponent<NPCBehavior>().setSelected(false);
            }
            if (selectedNPC != null)
            {
                // 새로 선택된 NPC 처리
                selectedNPC.GetComponent<NPCBehavior>().setSelected(true);
            }
            this.selectedNPC = selectedNPC;
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
