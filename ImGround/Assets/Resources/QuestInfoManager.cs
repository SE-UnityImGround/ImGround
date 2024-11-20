﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class QuestInfoManager
{
    /*==================================================
     *                 싱글톤 관리자
     *==================================================*/

    /// <summary>
    /// 절대 이 멤버로 접근하지 말 것. <see cref="getInstance"/>를 사용하세요.
    /// </summary>
    private static QuestInfoManager instance = null;

    private QuestInfoManager()
    {

    }

    private static QuestInfoManager getInstance()
    {
        if (instance == null)
        {
            instance = new QuestInfoManager();
            checkQuestInfo();
        }
        return instance;
    }

    /// <summary>
    /// 모든 퀘스트 정보가 포함되어 있는지를 검사합니다.
    /// </summary>
    private static void checkQuestInfo()
    {
        string omittedData = "";
        string cantLoaded = "";
        foreach (QuestIdEnum id in Enum.GetValues(typeof(QuestIdEnum)))
        {
            try
            {
                if (findQuestInfo(id).UIPrefab == null)
                {
                    cantLoaded += id.ToString() + "\n";
                }
            }
            catch
            {
                omittedData += id.ToString() + "\n";
            }
        }
        if (omittedData.Length > 0)
            Debug.LogError("다음의 퀘스트 정보가 아직 등록되지 않았습니다! :\n" + omittedData);
        if (cantLoaded.Length > 0)
            Debug.LogError("다음의 퀘스트 UI 프리팹 로딩에 실패했습니다! :\n" + cantLoaded);
    }

    /*==================================================
     *               Quest Info 데이터 
     *==================================================*/

    private QuestInfo[] questInfo =
    {
        new QuestInfo(
            QuestIdEnum.Q_1,
            new Quest(
                QuestIdEnum.Q_1,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.GOLD_NECKLACE, 1, false) },
                new ItemBundle[] { },
                5000),
            (GameObject)Resources.Load("QuestPrefabs/Quest_1")),

        new QuestInfo(
            QuestIdEnum.Q_2,
            new Quest(
                QuestIdEnum.Q_2,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.PIZZA, 1, false),
                    new ItemBundle(ItemIdEnum.HAMBURGER, 1, false)},
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.SILVER_ORE, 1, false)},
                100000),
            (GameObject)Resources.Load("QuestPrefabs/Quest_2")),

        new QuestInfo(
            QuestIdEnum.Q_3,
            new Quest(
                QuestIdEnum.Q_3,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.FROUT_SALAD, 1, false)},
                new ItemBundle[] { },
                5000),
            (GameObject)Resources.Load("QuestPrefabs/Quest_3")),

        new QuestInfo(
            QuestIdEnum.Q_4,
            new Quest(
                QuestIdEnum.Q_4,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.CHEESE, 3, false),
                    new ItemBundle(ItemIdEnum.BREAD, 1, false)},
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.SILVER_ORE, 1, false)},
                100000),
            (GameObject)Resources.Load("QuestPrefabs/Quest_4")),

        new QuestInfo(
            QuestIdEnum.Q_5,
            new Quest(
                QuestIdEnum.Q_5,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.APPLE_JUICE, 3, false)},
                new ItemBundle[] { },
                5000),
            (GameObject)Resources.Load("QuestPrefabs/Quest_5")),

        new QuestInfo(
            QuestIdEnum.Q_6,
            new Quest(
                QuestIdEnum.Q_6,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.BANANA_MILK, 5, false)},
                new ItemBundle[] { },
                5000),
            (GameObject)Resources.Load("QuestPrefabs/Quest_6")),

        new QuestInfo(
            QuestIdEnum.Q_7,
            new Quest(
                QuestIdEnum.Q_7,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.WATERMELON, 5, false), 
                    new ItemBundle(ItemIdEnum.WATERMELON_JUICE, 2, false)},
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.SILVER_ORE, 1, false)},
                100000),
            (GameObject)Resources.Load("QuestPrefabs/Quest_7")),

        new QuestInfo(
            QuestIdEnum.Q_8,
            new Quest(
                QuestIdEnum.Q_8,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.APPLE_JUICE, 2, false),
                    new ItemBundle(ItemIdEnum.LEMON_JUICE, 3, false)},
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.SILVER_ORE, 1, false)},
                100000),
            (GameObject)Resources.Load("QuestPrefabs/Quest_8")),
        
        new QuestInfo(
            QuestIdEnum.Q_9,
            new Quest(
                QuestIdEnum.Q_9,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.CHICKEN_SALAD, 2, false)},
                new ItemBundle[] { },
                5000),
            (GameObject)Resources.Load("QuestPrefabs/Quest_9")),

        new QuestInfo(
            QuestIdEnum.Q_10,
            new Quest(
                QuestIdEnum.Q_10,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.FISH, 2, false),
                    new ItemBundle(ItemIdEnum.SALMON, 1, false)},
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.SILVER_INGOT, 1, false)},
                100000),
            (GameObject)Resources.Load("QuestPrefabs/Quest_10")),

        new QuestInfo(
            QuestIdEnum.Q_11,
            new Quest(
                QuestIdEnum.Q_11,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.COOKED_LAMB, 4, false)},
                new ItemBundle[] { },
                5000),
            (GameObject)Resources.Load("QuestPrefabs/Quest_11")),

        new QuestInfo(
            QuestIdEnum.Q_12,
            new Quest(
                QuestIdEnum.Q_12,
                new ItemBundle[] {
                    new ItemBundle(ItemIdEnum.STEAK, 1, false),
                    new ItemBundle(ItemIdEnum.FROUT_SALAD, 1, false),
                    new ItemBundle(ItemIdEnum.HAMBURGER, 1, false),
                    new ItemBundle(ItemIdEnum.FISH_AND_CHIPS, 1, false),
                    new ItemBundle(ItemIdEnum.WATERMELON_JUICE, 1, false),
                    new ItemBundle(ItemIdEnum.APPLE_JUICE, 1, false),
                    new ItemBundle(ItemIdEnum.LEMON_JUICE, 1, false)},
                new ItemBundle[] { }, // 뭐줄까 스페셜 퀘스트
                0), // 얼마줄까 스페셜 퀘스트
            (GameObject)Resources.Load("QuestPrefabs/Quest_12")),
    };

    /*==================================================
     *                 Util Method
     *==================================================*/

    private static QuestInfo findQuestInfo(QuestIdEnum questId)
    {
        // 현재 O(N) 알고리즘 -> 리팩터링 요소
        foreach (QuestInfo info in getInstance().questInfo)
            if (info.questId == questId)
                return info;
        throw new Exception("오류 : " + questId.ToString() + "에 대한 퀘스트 정보가 등록되지 않았습니다!");
    }

    /*==================================================
     *                외부 지원 메소드
     *==================================================*/

    public static Quest getQuestInfo(QuestIdEnum questId)
    {
        return findQuestInfo(questId).quest;
    }

    public static GameObject getQuestUIPrefab(QuestIdEnum questId)
    {
        return findQuestInfo(questId).UIPrefab;
    }
}