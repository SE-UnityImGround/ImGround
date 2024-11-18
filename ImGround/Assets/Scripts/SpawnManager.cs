using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] enemies;
    public float enemyRespawnTime = 10.0f; // ���� �ٽ� Ȱ��ȭ�� �ð�
    public float bossRespawnTime = 10.0f;
    private Dictionary<GameObject, bool> respawnInProgress = new Dictionary<GameObject, bool>(); // ������ ���� ���� Ȯ��

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // ��� ���� ���� ������ ���� ���� �ʱ�ȭ
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                respawnInProgress[enemy] = false;
            }
        }
    }

    void Update()
    {
        // �� �����Ӹ��� ���� Ȱ��ȭ ���¸� Ȯ��
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && !enemy.activeSelf && !respawnInProgress[enemy])
            {
                // ���� ��Ȱ��ȭ�� ��� ������ �ð� ����
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    float respawnTime = (enemyComponent.type == Enemy.Type.Boss) ? bossRespawnTime : enemyRespawnTime;
                    respawnInProgress[enemy] = true; // �������� ���� ������ ǥ��
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
        respawnInProgress[enemy] = false; // �������� �Ϸ�Ǿ����� ǥ��
    }
}
