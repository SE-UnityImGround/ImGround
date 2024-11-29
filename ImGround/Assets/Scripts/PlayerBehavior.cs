using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{
    public AudioSource[] effectSound;
    Animator anim;
    private Player player;
    private Crops crop;
    public GameObject[] tools;
    private GameObject grabbedItem = null;
    private int grabbingSlotIdx = -1;

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
    bool isGrabbing = false;
    bool canFarming = false;
    bool canPlant = false;

    public Transform handPoint; // 아이템을 줍기 위한 손의 위치
    public Transform pickPoint; // 아이템을 줍기 위한 손의 위치
    public Transform pointH;  // 낫의 콜라이더 위치
    public Transform[] curtivatePoint; // 괭이와 삽의 콜라이더 (0번 인덱스 : 괭이, 1번 인덱스 : 삽)
    public Transform ItemPoint; // 음식을 먹는 손의 위치
    private GameObject pickedItem; // 현재 주운 아이템
    public int GrabSlotID { get { return grabbingSlotIdx; } }
    public bool IsEating {  get { return isEating; } }
    public bool IsPickingUp { get {  return isPickingUp; } }
    public bool IsDigging { get {  return isDigging; } set { isDigging = value; } }
    public bool IsPicking { get { return isPicking; } }
    public bool IsHarvest { get { return isHarvest; } }
    public bool IsPlant {  get { return isPlant; } }

    public bool IsGrabbing { set { isGrabbing = value; } }
    public int ToolIndex { get { return toolIndex; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }

    float digDelay, pickDelay, harvestDelay, plantDelay;

    int toolIndex = 0; // 현재 플레이어가 손에 든 도구의 인덱스

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        sDown = new bool[8];

        InventoryManager.onSelectionChangedHandler += OnItemSelectionChanged;
    }
    public void GetInput()
    {
        dDown = InputManager.GetButton("Fire2"); // 도구 동작 키
        fDown = InputManager.GetKeyDown(KeyCode.F); // 줍기 키
        eDown = InputManager.GetKeyDown(KeyCode.E);
        sDown[1] = InputManager.GetKeyDown(KeyCode.Alpha1); // 1번 키
        sDown[2] = InputManager.GetKeyDown(KeyCode.Alpha2); // 2번 키
        sDown[3] = InputManager.GetKeyDown(KeyCode.Alpha3); // 3번 키
        sDown[4] = InputManager.GetKeyDown(KeyCode.Alpha4); // 4번 키
        sDown[5] = InputManager.GetKeyDown(KeyCode.Alpha5); // 5번 키
        sDown[6] = InputManager.GetKeyDown(KeyCode.Alpha6); // 6번 키
        sDown[7] = InputManager.GetKeyDown(KeyCode.Alpha7); // 7번 키
        sDown[0] = InputManager.GetKeyDown(KeyCode.Alpha0); // 0번 키
    }

    // 도구 사용 및 행동 로직
    public void Use()
    {
        digDelay += Time.deltaTime;
        pickDelay += Time.deltaTime;
        harvestDelay += Time.deltaTime;
        plantDelay += Time.deltaTime;

        isDigReady = 1.5f < digDelay;
        isPickReady = 0.8f < pickDelay;
        isHarvestReady = 0.4f < harvestDelay;
        isPlantReady = 2f < plantDelay;

        if(toolIndex == 0 && dDown && !player.pMove.IsJumping && !player.pAttack.IsAttacking)
        {// 음식 먹기
            if (ItemPoint.childCount == 0
                || IsEating // 먹는 도중 또 먹는 로직을 실행하는 현상 방지
                || !isGrabbing // 아무 아이템도 잡고 있지 않다면 처리하지 않음
                || !ItemInfoManager.getItemInfo(grabbedItem.GetComponent<ItemPrefabID>().itemType).isFood) // 음식을 들고 있는게 아니면 처리하지 않음
                return;

            // 체력 회복 처리
            float healAmount = ItemInfoManager.getItemInfo(grabbedItem.GetComponent<ItemPrefabID>().itemType).healAmount;
            player.health = Mathf.Min(player.health + (int)(healAmount * player.MaxHealth), player.MaxHealth);

            isEating = true;
            anim.SetTrigger("doEat");
            StartCoroutine(ResetEat());
            effectSound[4].Play();
        }
        else if (toolIndex == 0 && fDown && !isPickingUp && !isDigging && !isHarvest && !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking && !isGrabbing)
        {
            Collider nearestCollider = null;
            float nearestDistance = Mathf.Infinity;         
            // 원형 범위로 아이템 감지 (OverlapSphere 사용)
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // 플레이어 주변 1미터 범위

            foreach (Collider hitCollider in hitColliders)
            {
                // 특정 태그가 있는지 확인(있다면 pickedItem에 해당 프리펩 저장)
                if (hitCollider.CompareTag("fruit") || hitCollider.CompareTag("crop") || hitCollider.CompareTag("item"))
                {
                    // 각 콜라이더와의 거리 계산
                    float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestCollider = hitCollider;
                    }
                }
            }
            // 플레이어와 가장 가까이 있는 물체를 줍기
            pickedItem = nearestCollider.gameObject;
            isPickingUp = true;
            anim.SetTrigger("doPickUp");

            // 아이템 손으로 줍기 동작
            StartCoroutine(Picking());
            effectSound[5].Play();

            // 물리 효과 제거 (아이템이 손에서 날뛰는 문제 해결)
            if (pickedItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true; // 물리 시뮬레이션 중지하여 고정
            }

            // 과일의 Collider 비활성화 (플레이어와의 충돌로 플레이어가 날아가는 문제 방지)
            if (pickedItem.TryGetComponent<Collider>(out Collider itemCollider) && !pickedItem.CompareTag("crop"))
            {
                itemCollider.enabled = false; // 충돌 비활성화
            }

            // 아이템 줍기 동작 종료
            StartCoroutine(ResetPickUp());
        }
        else if (toolIndex == 0 && ItemPoint.childCount > 0 && eDown && canFarming && canPlant && isPlantReady && !isPickingUp && !isHarvest &&
                !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking)
        {// 씨앗 심기
            StartCoroutine(Planting());
            anim.SetTrigger("doPlant");
            isPlant = true;
            plantDelay = 0f;
            effectSound[6].Play();
            StartCoroutine(ResetPlant());
        }

        else if ((toolIndex == 1 || toolIndex == 3) && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {
            if (toolIndex == 1 && canFarming) // 경작
            {
                anim.SetTrigger("doDigDown");
                isDigging = true;
                digDelay = 0f;
                curtivatePoint[0].gameObject.SetActive(true);
                effectSound[7].Play();
                StartCoroutine(ResetDig());
            }
            else if (toolIndex == 3) // 채광
            {
                // 원형 범위로 아이템 감지 (OverlapSphere 사용)
                Collider[] checkOre = Physics.OverlapSphere(transform.position, MiningOre.allowedDistance);

                foreach (Collider hitOre in checkOre)
                {
                    if (hitOre.CompareTag("Ore")) // 'Ore' 태그가 있는지 확인 (있다면 채광 가능)
                    {
                        anim.SetTrigger("doDigDown");
                        isDigging = true;
                        digDelay = 0f;
                        effectSound[8].Play();
                        StartCoroutine(ResetDig());
                    }
                }
            }
        }

        else if (toolIndex == 2 && dDown && isPickReady && !isHarvest && !player.pAttack.IsAttacking)
        {// 과일 수확
            player.rigid.AddForce(Vector3.up * 4f, ForceMode.Impulse);
            anim.SetTrigger("doPick");
            isPicking = true;
            pickDelay = 0f;
            if (isPicking && !effectSound[11].isPlaying)
            {
                effectSound[11].Play();
            }
            else if ((!isPicking && effectSound[11].isPlaying))
            {
                effectSound[11].Stop();
            }
            
            StartCoroutine(ResetPick());
        }
        else if (toolIndex == 4 && canFarming && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// 땅파기
            anim.SetTrigger("doDigUp");
            isDigging = true;
            digDelay = 0f;
            curtivatePoint[1].gameObject.SetActive(true);
            
            if (isDigging && !effectSound[9].isPlaying)
            {
                effectSound[9].Play();
            }
            else if ((!isDigging && effectSound[9].isPlaying))
            {
                effectSound[9].Stop();
            }
            StartCoroutine(ResetDigUp());
        }
        else if(toolIndex == 5 && canFarming && dDown && isHarvestReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// 작물 수확하기
            anim.SetTrigger("doHarvest");
            isHarvest = true;
            pointH.gameObject.SetActive(true);
            harvestDelay = 0f;
            
            if (isHarvest && !effectSound[10].isPlaying)
            {
                effectSound[10].Play();
            }
            else if ((!isHarvest && effectSound[10].isPlaying))
            {
                effectSound[10].Stop();
            }
            StartCoroutine(ResetHarvest());
        }
    }

    /// <summary>
    /// 인벤토리로부터 아이템 선택 값이 변경되었을때 처리되는 이벤트 처리기
    /// </summary>
    /// <param name="slotIdx"></param>
    private void OnItemSelectionChanged(int slotIdx)
    {
        // 이전 들고있던 아이템 숨기기
        if (grabbedItem != null)
            Destroy(grabbedItem.gameObject);

        ItemIdEnum item = InventoryManager.getItemId(slotIdx);
        if (item == ItemIdEnum.TEST_NULL_ITEM)
        {
            // 아무것도 선택하지 않음 상태라면
            grabbedItem = null;
            isGrabbing = false;
            grabbingSlotIdx = -1;
            return;
        }

        // 무언가를 선택했다면 손에 쥐어줘
        grabbedItem = ItemPrefabSO.getItemPrefab(new ItemBundle(item, 1, false)).gameObject;
        grabbedItem.SetActive(false);
        isGrabbing = true;
        grabbingSlotIdx = slotIdx;
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
        ItemBundle[] inventoryItems = InventoryManager.popAllItems();
        if (inventoryItems.Length > 0)
            ItemThrowManager.throwItem(new ItemBundle(new ItemPackage(inventoryItems), 1, false));
        anim.SetTrigger("doDie");
        isDie = true;
        player.pMove.IsWalking = false;
        player.pMove.IsRunning = false;
    }
    public void Swap()
    {
        if (!isEating && !isDigging && !isPicking && !isPickingUp && !isHarvest && !isPlant) {
            int currentIndex = toolIndex;
            if (sDown[1]) // 주먹
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 0;
                isGrabbing = false;
            }
            if (sDown[2]) // 괭이
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 1;
                isGrabbing = false;
            }
            if (sDown[3]) // 삼지창(과일 수확용)
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 2;
                isGrabbing = false;
            }
            if (sDown[4]) // 곡괭이
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 3;
                isGrabbing = false;
            }
            if (sDown[5]) // 삽
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 4;
                isGrabbing = false;
            }
            if (sDown[6]) // 낫
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 5;
                isGrabbing = false;
            }
            if (sDown[7]) // 검
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 6;
                isGrabbing = false;
            }
            if (sDown[0]) // 이스터에그
            {
                tools[currentIndex].gameObject.SetActive(false);
                toolIndex = 7;
                isGrabbing = false;
            }

            // 들고 있는 아이템 처리
            if (isGrabbing)
            {
                toolIndex = 0;

                tools[currentIndex].gameObject.SetActive(false);
                grabbedItem.transform.SetParent(ItemPoint, true);
                grabbedItem.transform.localPosition = Vector3.zero;
                grabbedItem.SetActive(true);
            }
            else
            {
                if (grabbedItem != null)
                    Destroy(grabbedItem.gameObject);
                grabbedItem = null;
            }

            if (!isGrabbing)
            {
                tools[toolIndex].gameObject.SetActive(true);
            }
        }
    }
    IEnumerator ResetDig()
    {
        if(toolIndex == 3)
            yield return new WaitForSeconds(1f);
        else
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
        yield return new WaitForSeconds(1f); 
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
            GetItem(pickedItem.GetComponent<ItemPrefabID>()); // 주운 아이템의 인벤토리 처리
            pickedItem.transform.SetParent(null);
            pickedItem.gameObject.SetActive(false);
            pickedItem = null; // Reset the reference to the item
        }
    }

    private void GetItem(ItemPrefabID itemPrefabId)
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
                WarningManager.startWarning();
            }
        }
    }

    IEnumerator ResetEat()
    {
        yield return new WaitForSeconds(1.5f);
        isEating = false;
        InventoryManager.takeItem(grabbingSlotIdx, 1);
    }

    IEnumerator ResetHarvest()
    {
        yield return new WaitForSeconds(0.7f);
        pointH.gameObject.SetActive(false);
        isHarvest = false;
    }

    IEnumerator ResetPlant()
    {
        yield return new WaitForSeconds(4f);
        isPlant = false;
        InventoryManager.takeItem(grabbingSlotIdx, 1);
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
    }

    IEnumerator Planting()
    {
        yield return new WaitForSeconds(1f);
        ItemPrefabID seed = ItemPoint.GetComponentInChildren<ItemPrefabID>();
        bool isSeed = seed.itemType == ItemIdEnum.CARROT_SEED || seed.itemType == ItemIdEnum.RICE_SEED || seed.itemType == ItemIdEnum.TOMATO_SEED ||
                      seed.itemType == ItemIdEnum.LEMMON_SEED || seed.itemType == ItemIdEnum.WATERMELON_SEED;
        // 원형 범위로 아이템 감지 (OverlapSphere 사용)
        Collider nearestCollider = null;
        float nearestDistance = Mathf.Infinity;
        Collider[] checkGround = Physics.OverlapSphere(transform.position, 1f); // 플레이어 주변 1미터 범위
        foreach (Collider collider in checkGround)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Harvest")) // 'Harvest' 태그가 있는지 확인
            {
                // 각 콜라이더와의 거리 계산
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestCollider = collider;
                }
            }
        }
        crop = nearestCollider.GetComponent<Crops>();
        if (crop == null)
        {
            yield break;
        }
        yield return new WaitForSeconds(6f);
        // 현재 들고 있는 아이템이 씨앗인지 확인
        if (isSeed)
        {
            switch(seed.itemType)
            {
                case ItemIdEnum.CARROT_SEED:
                    crop.CropIndex = 0;
                    break;
                case ItemIdEnum.RICE_SEED:
                    crop.CropIndex = 1;
                    break;
                case ItemIdEnum.TOMATO_SEED:
                    crop.CropIndex = 2;
                    break;
                case ItemIdEnum.LEMMON_SEED:
                    crop.CropIndex = 3;
                    break;
                case ItemIdEnum.WATERMELON_SEED:
                    crop.CropIndex = 4;
                    break;
            }

        }
    }
    // 과일 수확 로직 + 피격 로직
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
                    effectSound[0].Play(); // 0번 효과음 재생
                    StartCoroutine(PlaySoundWithDelay(effectSound[3], 1.0f)); // 1초 후 1번 효과음 재생
                }
            }
            else if (other.tag == "BossAttack")
            {
                anim.SetTrigger("doHit");
                player.health -= 6;
                effectSound[1].Play();
            }
            else if (other.tag == "BossRock")
            {
                anim.SetTrigger("doHit");
                player.health -= 10;
                effectSound[1].Play();
            }
            else if(other.tag == "EnemyAttack")
            {
                anim.SetTrigger("doHit");
                player.health -= 2;
                effectSound[1].Play();
            }
            if (player.health <= 0)
            {
                Die();
                effectSound[2].Play();
            }
        }
    }
    void OnCollisionStay(Collision collision)
    {
        // 현재 플레이어가 밟고 있는 바닥이 농작지인지 확인
        if (collision.gameObject.layer == LayerMask.NameToLayer("Harvest"))
        {
            canFarming = true;
        }
        // 현재 플레이어가 밟고 있는 바닥이 경작된 땅인지 확인
        if (collision.gameObject.CompareTag("Plant"))
        {
            canPlant = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 현재 플레이어가 밟고 있는 바닥이 농작지인지 확인
        if (collision.gameObject.layer == LayerMask.NameToLayer("Harvest"))
        {
            canFarming = false;
        }
        // 현재 플레이어가 밟고 있는 바닥이 경작된 땅인지 확인
        if (collision.gameObject.CompareTag("Plant"))
        {
            canPlant = false;
        }
    }
    private IEnumerator PlaySoundWithDelay(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간만큼 대기
        audioSource.Play(); // 지정된 효과음 재생
    }
    private IEnumerator StopSoundWithDelay(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간만큼 대기
        audioSource.Stop(); // 지정된 효과음 재생
    }
}
