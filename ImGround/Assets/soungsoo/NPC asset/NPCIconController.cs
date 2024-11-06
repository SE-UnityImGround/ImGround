using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// NPC의 아이콘을 제어하는 클래스입니다.
/// </summary>
public class NPCIconController
{
    private NPCIconBehavior defaultIcon;
    private NPCIconBehavior questIcon;
    private NPCIconBehavior RewardIcon;

    private NPCIconBehavior selectedIcon = null;
    private bool isSelected;

    /// <summary>
    /// NPC 아이콘의 위치를 설정합니다.
    /// <br/>
    /// <br/><paramref name="npcTransform"/> : 대상 오브젝트의 <see cref="Transform"/>
    /// <br/><paramref name="offset"/> : 아이콘이 표시될 위치 오프셋
    /// </summary>
    public NPCIconController(Transform npcTransform, Vector3 offset)
    {
        defaultIcon = NpcIconsSO.getNPCIcon(NpcIconsSO.DEFAULT);
        questIcon = NpcIconsSO.getNPCIcon(NpcIconsSO.QUEST);
        RewardIcon = NpcIconsSO.getNPCIcon(NpcIconsSO.REWARD);

        defaultIcon.setPosition(npcTransform, offset);
        questIcon.setPosition(npcTransform, offset);
        RewardIcon.setPosition(npcTransform, offset);

        setIconType(NpcIconsSO.DEFAULT);
    }

    private NPCIconBehavior selectIcon(int iconType)
    {
        switch (iconType)
        {
            case NpcIconsSO.DEFAULT:
                return defaultIcon;
            case NpcIconsSO.QUEST:
                return questIcon;
            case NpcIconsSO.REWARD:
                return RewardIcon;
            default:
                throw new Exception(iconType + " : 알 수 없는 아이콘 타입입니다!");
        }
    }

    /// <summary>
    /// 아이콘을 변경합니다.
    /// <br/>
    /// <br/><paramref name="iconType"/> : <see cref="NpcIconsSO"/>의 아이콘 타입 상수
    /// </summary>
    public void setIconType(int iconType)
    {
        NPCIconBehavior selection = selectIcon(iconType);
        if (selectedIcon != selection)
        {
            if (selectedIcon != null)
            {
                selectedIcon.hide();
            }
            selection.show();
            selection.setSelected(isSelected);
            selectedIcon = selection;
        }
    }

    public void show()
    {
        selectedIcon.show();
    }

    public void hide()
    {
        selectedIcon.hide();
    }

    public void setSelected(bool isSelected)
    {
        this.isSelected = isSelected;
        selectedIcon.setSelected(isSelected);
    }
}
