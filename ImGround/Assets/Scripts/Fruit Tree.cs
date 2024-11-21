using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitTree : MonoBehaviour
{
    [SerializeField]
    private Transform[] fruitPos;
    [SerializeField]
    private GameObject fruit;

    private GameObject[] currentFruit;
    private bool[] isRespawning;

    void Awake()
    {
        currentFruit = new GameObject[fruitPos.Length];
        isRespawning = new bool[fruitPos.Length];

        for (int i = 0; i < fruitPos.Length; i++)
        {
            if (fruitPos[i] != null)
            {
                currentFruit[i] = Instantiate(fruit, fruitPos[i].position, fruitPos[i].rotation);
                currentFruit[i].transform.SetParent(fruitPos[i]);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < fruitPos.Length; i++)
        {
            if (fruitPos[i] == null) continue;

            if (fruitPos[i].childCount == 0 && !isRespawning[i])
            {
                StartCoroutine(RespawnFruit(fruitPos[i], i));
            }
        }
    }

    IEnumerator RespawnFruit(Transform pos, int index)
    {
        isRespawning[index] = true;
        yield return new WaitForSeconds(5f);
        GameObject newFruit = Instantiate(fruit, pos.position, pos.rotation);
        newFruit.transform.SetParent(pos);
        isRespawning[index] = false;
    }
}
