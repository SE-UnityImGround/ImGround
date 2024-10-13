using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] enemies;
    public float enemyRespawnTime = 10.0f; // ���� �ٽ� Ȱ��ȭ�� �ð�
    public float bossRespawnTime = 30.0f;
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        // �� �����Ӹ��� ���� Ȱ��ȭ ���¸� Ȯ��
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && !enemy.activeSelf)
            {
                // ���� ��Ȱ��ȭ�� ��� ������ �ð� ����
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    float respawnTime = (enemyComponent.type == Enemy.Type.Boss) ? bossRespawnTime : enemyRespawnTime;
                    StartCoroutine(RespawnEnemy(enemy, respawnTime));
                    enemyComponent.Respawn();
                }
            }
        }
    }

    // ���� ��Ȱ��ȭ�� �� ���� �ð� �ڿ� �ٽ� Ȱ��ȭ�ϴ� �Լ�
    IEnumerator RespawnEnemy(GameObject enemy, float delay)
    {
        // ���� �ð� ���
        yield return new WaitForSeconds(delay);

        // �� �ٽ� Ȱ��ȭ
        enemy.SetActive(true);
    }
}
