using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] enemies;
    private GameObject[] animals;
    public float enemyRespawnTime = 10.0f; // 적이 다시 활성화될 시간
    public float bossRespawnTime = 10.0f;
    private Dictionary<GameObject, bool> respawnInProgress = new Dictionary<GameObject, bool>(); // 리스폰 진행 여부 확인

    void Start()
    {
        animals = GameObject.FindGameObjectsWithTag("Animal");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // 모든 적에 대해 리스폰 진행 여부 초기화
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                respawnInProgress[enemy] = false;
            }
        }
        foreach (GameObject animal in animals)
        {
            if (animal != null)
            {
                respawnInProgress[animal] = false;
            }
        }
    }

    void Update()
    {
        // 매 프레임마다 적의 활성화 상태를 확인
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && !enemy.activeSelf && !respawnInProgress[enemy])
            {
                // 적이 비활성화된 경우 리스폰 시간 결정
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    float respawnTime = (enemyComponent.type == Enemy.Type.Boss) ? bossRespawnTime : enemyRespawnTime;
                    respawnInProgress[enemy] = true; // 리스폰이 진행 중임을 표시
                    StartCoroutine(RespawnEnemy(enemy, respawnTime));
                    enemyComponent.Respawn();
                }
            }
        }
        foreach (GameObject animal in animals)
        {
            if (animal != null && !animal.activeSelf && !respawnInProgress[animal])
            {
                // 적이 비활성화된 경우 리스폰 시간 결정
                /* Animal animalComponent = animal.GetComponent<Animal>();
                 if (animalComponent != null)
                 {
                     float respawnTime = 30f;
                     respawnInProgress[animal] = true; // 리스폰이 진행 중임을 표시
                     StartCoroutine(RespawnAnimal(animal, respawnTime));
                     animalComponent.Respawn();
                 }
                 else
                 {
                     Chicken chicken = animal.GetComponent<Chicken>();
                     float respawnTime = 15f;
                     respawnInProgress[animal] = true; // 리스폰이 진행 중임을 표시
                     StartCoroutine(RespawnAnimal(animal, respawnTime));
                     chicken.Respawn();
                 }*/
                Chicken chicken = animal.GetComponent<Chicken>();
                if (chicken != null)
                {   
                    float respawnTime = 15f;
                    respawnInProgress[animal] = true; // 리스폰이 진행 중임을 표시
                    StartCoroutine(RespawnAnimal(animal, respawnTime));
                    chicken.Respawn();
                    
                }
                else
                {
                    Animal animalComponent = animal.GetComponent<Animal>();
                    float respawnTime = 30f;
                    respawnInProgress[animal] = true; // 리스폰이 진행 중임을 표시
                    StartCoroutine(RespawnAnimal(animal, respawnTime));
                    animalComponent.Respawn();
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
        respawnInProgress[enemy] = false; // 리스폰이 완료되었음을 표시
    }
    IEnumerator RespawnAnimal(GameObject animal, float delay)
    {
        // 일정 시간 대기
        yield return new WaitForSeconds(delay);

        // 적 다시 활성화
        animal.SetActive(true);
        respawnInProgress[animal] = false; // 리스폰이 완료되었음을 표시
    }
}
