using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// NPC의 시선처리를 담당하는 클래스입니다.
/// </summary>
public class NpcGazer
{
    private Transform npcTransform;
    private Transform head;
    private Quaternion lastHeadRotation;
    private Quaternion deltaAngle;

    /// <summary>
    /// NPC의 시선처리를 담당하는 클래스입니다.
    /// <br/>초기 상태를 기준으로 시선을 처리하므로 NPC는 정면을 향한 상태여야 합니다.
    /// </summary>
    /// <param name="npcTransform"></param>
    public NpcGazer(Transform npcTransform)
    {
        if (npcTransform == null)
            throw new Exception("주어진 npc(Transform)가 null입니다.");
        this.npcTransform = npcTransform;

        if ((head = findChildRecursively(npcTransform, "Head")) == null)
            throw new Exception(string.Format("NPC 게임오브젝트 {0}의 Head를 찾을 수 없습니다!!", npcTransform.gameObject.name));
        else
        {
            deltaAngle = Quaternion.Euler(head.eulerAngles - npcTransform.eulerAngles);
            lastHeadRotation = head.rotation;
        }
    }

    /* ===========================
     *        Util 메소드
     * ===========================*/

    /// <summary>
    /// 주어진 이름의 자식 오브젝트(Transform)를 찾아 반환합니다.
    /// <br/> 없거나 찾지 못한 경우 null을 반환합니다.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private Transform findChildRecursively(Transform root, string name)
    {
        Queue<Transform> childs = new Queue<Transform>();
        childs.Enqueue(root);
        Transform child;
        Transform parent;

        do
        {
            for (int cnt = childs.Count; cnt > 0; cnt--)
            {
                parent = childs.Dequeue();
                for (int i = 0; i < parent.childCount; i++)
                {
                    child = parent.GetChild(i);
                    if (child.name.CompareTo(name) == 0)
                    {
                        childs.Clear();
                        return child;
                    }
                    else
                        childs.Enqueue(child);
                }
            }
        } while (childs.Count > 0);

        return null;
    }

    /// <summary>
    /// NPC의 머리를 duration 시간동안 lookAngle 방향으로 회전합니다.
    /// <br/>Lerp 선형보간이 적용됩니다.
    /// </summary>
    /// <param name="lookAngle"></param>
    /// <param name="duration"></param>
    private void lookAt(Quaternion lookAngle, float duration)
    {
        lastHeadRotation = Quaternion.Lerp(
               lastHeadRotation,
               lookAngle * deltaAngle,
               1.0f / duration * Time.deltaTime); // duration : a->b로 가는데 걸리는 총 시간(초)
        head.rotation = lastHeadRotation;
    }

    /* ===========================
     *        기능 메소드
     * ===========================*/

    /// <summary>
    /// NPC의 시선을 조정합니다.
    /// <br/>좌우 70도, 아래로 60도 위로 45도 범위 내에서만 동작하며, 그렇지 않으면 정면을 향합니다.
    /// </summary>
    /// <param name="lookPosition">시선을 향할 위치입니다.</param>
    public void lookAtPos(Vector3 lookPosition)
    {
        Quaternion lookAngle = Quaternion.LookRotation(lookPosition - head.transform.position);

        Vector3 angleAmount = lookAngle.eulerAngles - npcTransform.eulerAngles;
        float x = angleAmount.x % 360.0f;
        x = (x >= 180.0f ? x - 360.0f :
            (x <= -180.0f ? x + 360.0f : x)); // -180.0 ~ 180.0 범위로 정규화
        float y = angleAmount.y % 360.0f;
        y = (y >= 180.0f ? y - 360.0f :
            (y <= -180.0f ? y + 360.0f : y)); // -180.0 ~ 180.0 범위로 정규화

        if (Mathf.Abs(y) <= 70.0f
            && x <= 60.0f && x >= -45.0f) // 좌우 70도, 아래로 60도 위로 45도 까지의 시선만 적용
        {
            lookAt(lookAngle, 0.15f); // 플레이어를 향함
        }
        else
        {
            lookAtFront();
        }
    }

    /// <summary>
    /// 정면을 향합니다.
    /// </summary>
    public void lookAtFront()
    {
        lookAt(npcTransform.rotation, 0.5f);
    }
}
