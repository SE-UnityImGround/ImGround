using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTreeManager : MonoBehaviour
{
    private Dictionary<GameObject, bool> respawnInProgress = new Dictionary<GameObject, bool>(); // 리스폰 진행 여부 확인
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
        // 매 프레임마다 적의 활성화 상태를 확인
        foreach (GameObject fruit in fruits)
        {
            if (fruit != null && !fruit.activeSelf && !respawnInProgress[fruit])
            {
                Fruit fr = fruit.GetComponent<Fruit>();
                float respawnTime = 10f;
                respawnInProgress[fruit] = true; // 리스폰이 진행 중임을 표시
                StartCoroutine(RespawnFruit(fruit, respawnTime));
                fr.Respawn();
            }
        }
    }

    IEnumerator RespawnFruit(GameObject fruit, float delay)
    {
        // 일정 시간 대기
        yield return new WaitForSeconds(delay);

        // 적 다시 활성화
        fruit.SetActive(true);
        respawnInProgress[fruit] = false; // 리스폰이 완료되었음을 표시
    }
}