using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{
    Animator anim;
    private Player player;
    public GameObject[] tools;

    public bool dDown;
    bool fDown;
    bool eDown;
    bool[] sDown; // 0~7번까지의 도구 인덱스 번호 모음

    bool isDigReady, isPickReady, isHarvestReady, isPlantReady;
    bool isEating = false;
    bool isPickingUp = false;
    bool isDigging = false;
    bool isPicking = false;
    bool isHarvest = false;
    bool isPlant = false;
    bool isDie = false;

    public Transform handPoint; // 아이템을 줍기 위한 손의 위치
    public Transform pickPoint; // 아이템을 줍기 위한 손의 위치
    public Transform pointH;  // 낫의 콜라이더 위치
    public Transform[] curtivatePoint; // 괭이와 삽의 콜라이더 (0번 인덱스 : 괭이, 1번 인덱스 : 삽)
    public Transform ItemPoint; // 음식을 먹는 손의 위치
    private GameObject pickedItem; // 현재 주운 아이템
    public bool IsEating {  get { return isEating; } }
    public bool IsPickingUp { get {  return isPickingUp; } }
    public bool IsDigging { get {  return isDigging; } }
    public bool IsPicking { get { return isPicking; } }
    public bool IsHarvest { get { return isHarvest; } }
    public int ToolIndex { get { return toolIndex; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }

    float digDelay, pickDelay, harvestDelay, plantDelay;

    int toolIndex = 0; // 현재 플레이어가 손에 든 도구의 인덱스

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        sDown = new bool[8];
    }
    public void getInput()
    {
        dDown = Input.GetButton("Fire2"); // 도구 동작 키
        fDown = Input.GetKeyDown(KeyCode.F); // 줍기 키
        eDown = Input.GetKeyDown(KeyCode.E);
        sDown[1] = Input.GetKeyDown(KeyCode.Alpha1); // 1번 키
        sDown[2] = Input.GetKeyDown(KeyCode.Alpha2); // 2번 키
        sDown[3] = Input.GetKeyDown(KeyCode.Alpha3); // 3번 키
        sDown[4] = Input.GetKeyDown(KeyCode.Alpha4); // 4번 키
        sDown[5] = Input.GetKeyDown(KeyCode.Alpha5); // 5번 키
        sDown[6] = Input.GetKeyDown(KeyCode.Alpha6); // 6번 키
        sDown[7] = Input.GetKeyDown(KeyCode.Alpha7); // 7번 키
        sDown[0] = Input.GetKeyDown(KeyCode.Alpha0); // 0번 키
    }

    // 도구 사용 및 행동 로직
    public void Use()
    {
        digDelay += Time.deltaTime;
        pickDelay += Time.deltaTime;
        harvestDelay += Time.deltaTime;
        plantDelay += Time.deltaTime;

        isDigReady = 1.8f < digDelay;
        isPickReady = 1.2f < pickDelay;
        isHarvestReady = 0.4f < harvestDelay;
        isPlantReady = 2f < plantDelay;

        if(toolIndex == 0 && dDown && !player.pMove.IsJumping && !player.pAttack.IsAttacking)
        {// 음식 먹기
            if (ItemPoint.childCount == 0)
                return;
            isEating = true;
            anim.SetTrigger("doEat");
            StartCoroutine(ResetEat());
        }
        else if (toolIndex == 0 && fDown && !isPickingUp && !isDigging && !isHarvest && !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking)
        {
            // 원형 범위로 아이템 감지 (OverlapSphere 사용)
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.3f); // 플레이어 주변 1미터 범위

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("fruit") || hitCollider.CompareTag("crop") || hitCollider.CompareTag("item")) // 특정 태그가 있는지 확인 (있다면 pickedItem에 해당 프리펩 저장)
                {
                    pickedItem = hitCollider.gameObject;
                    isPickingUp = true;
                    //player.pMove.IsTired = true;
                    anim.SetTrigger("doPickUp");

                    // 아이템 손으로 줍기 동작
                    StartCoroutine(Picking());

                    // 물리 효과 제거 (아이템이 손에서 날뛰는 문제 해결)
                    if (pickedItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        rb.isKinematic = true; // 물리 시뮬레이션 중지하여 고정
                    }

                    // 과일의 Collider 비활성화 (플레이어와의 충돌로 플레이어가 날아가는 문제 방지)
                    if (pickedItem.TryGetComponent<Collider>(out Collider itemCollider))
                    {
                        itemCollider.enabled = false; // 충돌 비활성화
                    }

                    // 아이템 줍기 동작 종료
                    StartCoroutine(ResetPickUp());
                    break; // 한 번 아이템을 잡으면 루프 중지(범위 내 아이템이 여러개 동시에 줍는 것 방지)
                }
            }
        }
        else if (toolIndex == 0 && eDown && isPlantReady && !isPickingUp && !isHarvest && !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking)
        {// 씨앗 심기
            anim.SetTrigger("doPlant");
            isPlant = true;
            plantDelay = 0f;
            StartCoroutine(ResetPlant());
        }
        else if ((toolIndex == 1 || toolIndex == 3) && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// 경작하기 + 채광하기
            anim.SetTrigger("doDigDown");
            isDigging = true;
            digDelay = 0f;
            if(toolIndex == 1)
            {
                curtivatePoint[0].gameObject.SetActive(true);
            }
            StartCoroutine(ResetDig());
        }
        else if (toolIndex == 2 && dDown && isPickReady && !isHarvest && !player.pAttack.IsAttacking)
        {// 과일 수확
            player.rigid.AddForce(Vector3.up * 4f, ForceMode.Impulse);
            anim.SetTrigger("doPick");
            isPicking = true;
            pickDelay = 0f;
            StartCoroutine(ResetPick());
        }
        else if (toolIndex == 4 && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// 땅파기
            anim.SetTrigger("doDigUp");
            isDigging = true;
            digDelay = 0f;
            curtivatePoint[1].gameObject.SetActive(true);
            StartCoroutine(ResetDigUp());
        }
        else if(toolIndex == 5 && dDown && isHarvestReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// 작물 수확하기
            anim.SetTrigger("doHarvest");
            isHarvest = true;
            pointH.gameObject.SetActive(true);
            harvestDelay = 0f;
            StartCoroutine(ResetHarvest());
        }
    }
   
    // 아이템 줍기 범위 확인용(추후 삭제 예정)
    private void OnDrawGizmosSelected()
    {
        if (handPoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(handPoint.position, 1f);
    }
    // 사망 동작
    public void Die()
    {
        anim.SetTrigger("doDie");
        isDie = true;
    }
    public void Swap()
    {
        if (!isEating && !isDigging && !isPicking && !isPickingUp && !isHarvest) {
            int currentIndex = toolIndex;
            if (sDown[1]) // 주먹
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 0;
            }
            if (sDown[2]) // 괭이
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 1;
            }
            if (sDown[3]) // 삼지창(과일 수확용)
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 2;
            }
            if (sDown[4]) // 곡괭이
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 3;
            }
            if (sDown[5]) // 삽
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 4;
            }
            if (sDown[6]) // 낫
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 5;
            }
            if (sDown[7]) // 검
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 6;
            }
            if (sDown[0]) // 이스터에그
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 7;
            }

            tools[toolIndex].gameObject.SetActive(true);
        }
    }
    IEnumerator ResetDig()
    {
        yield return new WaitForSeconds(1.6f);
        isDigging = false;
        curtivatePoint[0].gameObject.SetActive(false);
    }

    IEnumerator ResetDigUp()
    {
        yield return new WaitForSeconds(1.5f);
        isDigging = false;
        curtivatePoint[1].gameObject.SetActive(false);
    }

    IEnumerator ResetPick()
    {
        yield return new WaitForSeconds(1.2f); 
        isPicking = false;
    }

    IEnumerator ResetPickUp()
    {
        yield return new WaitForSeconds(2.4f); // Wait for the pick-up animation to finish
        player.pMove.IsTired = false;
        isPickingUp = false;

        // Destroy the picked-up item
        if (pickedItem != null)
        {
            getItem(pickedItem.GetComponent<ItemPrefabID>()); // 주운 아이템의 인벤토리 처리
            pickedItem.transform.SetParent(null);
            pickedItem.gameObject.SetActive(false);
            //Destroy(pickedItem);
            pickedItem = null; // Reset the reference to the item
        }
    }

    private void getItem(ItemPrefabID itemPrefabId)
    {
        if (itemPrefabId == null)
        {
            Debug.LogError("아이템을 추가할 수 없음 : 아이템 프리팹/오브젝트에 " + nameof(ItemPrefabID) + " 스크립트가 없습니다!");
        }
        else
        {
            ItemBundle takenItem = itemPrefabId.getItem();
            InventoryManager.addItems(takenItem);
            if (takenItem.count > 0)
            {
                ItemThrowManager.throwItem(takenItem);
            }
        }
    }

    IEnumerator ResetEat()
    {
        yield return new WaitForSeconds(1.5f);
        isEating = false;
    }

    IEnumerator ResetHarvest()
    {
        yield return new WaitForSeconds(0.7f);
        pointH.gameObject.SetActive(false);
        isHarvest = false;
    }

    IEnumerator ResetPlant()
    {
        yield return new WaitForSeconds(2f);
        isPlant = false;
    }
    IEnumerator Picking()
    {
        // 아이템을 손 위치로 이동
        yield return new WaitForSeconds(0.5f);
        // 현재의 월드 스케일 저장
        Vector3 originalScale = pickedItem.transform.lossyScale;

        // 부모 설정
        pickedItem.transform.SetParent(handPoint, worldPositionStays: false);

        // 스케일을 기존 월드 스케일로 고정
        pickedItem.transform.localScale = new Vector3(
            originalScale.x / handPoint.lossyScale.x,
            originalScale.y / handPoint.lossyScale.y,
            originalScale.z / handPoint.lossyScale.z
        );
        pickedItem.transform.localPosition = Vector3.zero;
        pickedItem.transform.localRotation = Quaternion.identity; // 손의 회전과 맞춤  

        Debug.Log(pickedItem.transform.position);
    }
    // 과일 수확 로직
    private void OnTriggerEnter(Collider other)
    {
        if (!isDie)
        {
            if (other.tag == "fruit" && toolIndex == 2 && isPicking)
            {
                Rigidbody fruitRb = other.GetComponent<Rigidbody>();
                Collider fruitCollider = other.GetComponent<Collider>();
                if (fruitRb != null)
                {
                    fruitRb.useGravity = true;
                    fruitCollider.isTrigger = false;
                }
            }
            else if (other.tag == "BossAttack")
            {
                anim.SetTrigger("doHit");
                player.health -= 6;
            }
            else if (other.tag == "BossRock")
            {
                anim.SetTrigger("doHit");
                player.health -= 10;
            }
            else if(other.tag == "EnemyAttack")
            {
                anim.SetTrigger("doHit");
                player.health -= 2;
            }
            if (player.health <= 0)
            {
                Die();
            }
        }
    }
}
