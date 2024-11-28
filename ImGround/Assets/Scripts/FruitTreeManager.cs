using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTreeManager : MonoBehaviour
{
    private Dictionary<GameObject, bool> respawnInProgress = new Dictionary<GameObject, bool>(); // ������ ���� ���� Ȯ��
    private GameObject[] fruits;

    private void Start()
    {
        fruits = GameObject.FindGameObjectsWithTag("fruit");
        foreach (GameObject fruit in fruits)
        {
            if (fruit != null)
            {
                respawnInProgress[fruit] = false;
            }
        }
    }

    private void Update()
    {
        // �� �����Ӹ��� ���� Ȱ��ȭ ���¸� Ȯ��
        foreach (GameObject fruit in fruits)
        {
            if (fruit != null && !fruit.activeSelf && !respawnInProgress[fruit])
            {
                Fruit fr = fruit.GetComponent<Fruit>();
                float respawnTime = 10f;
                respawnInProgress[fruit] = true; // �������� ���� ������ ǥ��
                StartCoroutine(RespawnFruit(fruit, respawnTime));
                fr.Respawn();
            }
        }
    }

    IEnumerator RespawnFruit(GameObject fruit, float delay)
    {
        // ���� �ð� ���
        yield return new WaitForSeconds(delay);

        // �� �ٽ� Ȱ��ȭ
        fruit.SetActive(true);
        respawnInProgress[fruit] = false; // �������� �Ϸ�Ǿ����� ǥ��
    }
}