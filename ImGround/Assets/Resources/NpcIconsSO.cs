using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� Ŭ������ ScriptableObject��, �������� �����Ͽ� ����ؾ� �մϴ�!
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NpcIconsSO")]
public class NpcIconsSO : ScriptableObject
{
    /*==================================================
     *                 Static ������ 
     *==================================================*/

    private static NpcIconsSO instance = null;

    private static NpcIconsSO getInstance()
    {
        if (instance == null)
        {
            instance = Resources.Load<NpcIconsSO>("NpcIconsSO");
            if (instance == null)
            {
                Debug.LogError("Resources ������ " + nameof(NpcIconsSO) + " ScriptableObject�� ���� ������ ��ϵ��� �ʾҽ��ϴ�!");
            }
        }
        return instance;
    }

    public static NPCIconBehavior getNPCIcon(int IconNumber)
    {
        GameObject newIconObject;
        switch (IconNumber) {
            case DEFAULT:
                newIconObject = getInstance().Icon_Default_Prefab;
                break;
            case QUEST:
                newIconObject = getInstance().Icon_Quest_Prefab;
                break;
            case REWARD:
                newIconObject = getInstance().Icon_Reward_Prefab;
                break;
            default:
                return null;
        }
        return Instantiate(newIconObject).GetComponent<NPCIconBehavior>();
    }

    /*==================================================
     *              NPC Icon Selecter 
     *==================================================*/

    public const int DEFAULT = 0;
    public const int QUEST = 1;
    public const int REWARD = 2;

    /*==================================================
     *              NPC Icon ������ ������ 
     *==================================================*/

    public GameObject Icon_Default_Prefab;
    public GameObject Icon_Quest_Prefab;
    public GameObject Icon_Reward_Prefab;
}
