using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject[] enemies;
    private GameObject[] animals;
    public float enemyRespawnTime = 10.0f; // ���� �ٽ� Ȱ��ȭ�� �ð�
    public float bossRespawnTime = 10.0f;
    private Dictionary<GameObject, bool> respawnInProgress = new Dictionary<GameObject, bool>(); // ������ ���� ���� Ȯ��

    void Start()
    {
        animals = GameObject.FindGameObjectsWithTag("Animal");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // ��� ���� ���� ������ ���� ���� �ʱ�ȭ
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
        foreach (GameObject animal in animals)
        {
            if (animal != null && !animal.activeSelf && !respawnInProgress[animal])
            {
                // ���� ��Ȱ��ȭ�� ��� ������ �ð� ����
                /* Animal animalComponent = animal.GetComponent<Animal>();
                 if (animalComponent != null)
                 {
                     float respawnTime = 30f;
                     respawnInProgress[animal] = true; // �������� ���� ������ ǥ��
                     StartCoroutine(RespawnAnimal(animal, respawnTime));
                     animalComponent.Respawn();
                 }
                 else
                 {
                     Chicken chicken = animal.GetComponent<Chicken>();
                     float respawnTime = 15f;
                     respawnInProgress[animal] = true; // �������� ���� ������ ǥ��
                     StartCoroutine(RespawnAnimal(animal, respawnTime));
                     chicken.Respawn();
                 }*/
                Chicken chicken = animal.GetComponent<Chicken>();
                if (chicken != null)
                {   
                    float respawnTime = 15f;
                    respawnInProgress[animal] = true; // �������� ���� ������ ǥ��
                    StartCoroutine(RespawnAnimal(animal, respawnTime));
                    chicken.Respawn();
                    
                }
                else
                {
                    Animal animalComponent = animal.GetComponent<Animal>();
                    float respawnTime = 30f;
                    respawnInProgress[animal] = true; // �������� ���� ������ ǥ��
                    StartCoroutine(RespawnAnimal(animal, respawnTime));
                    animalComponent.Respawn();
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
    IEnumerator RespawnAnimal(GameObject animal, float delay)
    {
        // ���� �ð� ���
        yield return new WaitForSeconds(delay);

        // �� �ٽ� Ȱ��ȭ
        animal.SetActive(true);
        respawnInProgress[animal] = false; // �������� �Ϸ�Ǿ����� ǥ��
    }
}
