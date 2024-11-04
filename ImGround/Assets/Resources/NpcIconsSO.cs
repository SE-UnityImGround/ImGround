using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이 클래스는 ScriptableObject로, 에셋으로 생성하여 사용해야 합니다!
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NpcIconsSO")]
public class NpcIconsSO : ScriptableObject
{
    /*==================================================
     *                 Static 접근자 
     *==================================================*/

    private static NpcIconsSO instance = null;

    private static NpcIconsSO getInstance()
    {
        if (instance == null)
        {
            instance = Resources.Load<NpcIconsSO>("NpcIconsSO");
            if (instance == null)
            {
                Debug.LogError("Resources 폴더에 " + nameof(NpcIconsSO) + " ScriptableObject에 대한 에셋이 등록되지 않았습니다!");
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
     *              NPC Icon 프리팹 데이터 
     *==================================================*/

    public GameObject Icon_Default_Prefab;
    public GameObject Icon_Quest_Prefab;
    public GameObject Icon_Reward_Prefab;
}
