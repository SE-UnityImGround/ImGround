using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] enemies;
    public float enemyRespawnTime = 10.0f; // 적이 다시 활성화될 시간
    public float bossRespawnTime = 30.0f;
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Update()
    {
        // 매 프레임마다 적의 활성화 상태를 확인
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && !enemy.activeSelf)
            {
                // 적이 비활성화된 경우 리스폰 시간 결정
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

    // 적이 비활성화된 후 일정 시간 뒤에 다시 활성화하는 함수
    IEnumerator RespawnEnemy(GameObject enemy, float delay)
    {
        // 일정 시간 대기
        yield return new WaitForSeconds(delay);

        // 적 다시 활성화
        enemy.SetActive(true);
    }
}
