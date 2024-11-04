using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBehavior : MonoBehaviour
{
    Animator anim;
    private Player player;
    public GameObject[] tools;

    bool dDown;
    bool fDown;
    //bool eDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool sDown4;
    bool sDown5;
    bool sDown6;
    bool sDown7;
    bool sDown0;

    bool isDigReady;
    bool isPickReady;
    bool isHarvestReady;
    bool isEating = false;
    bool isPickingUp = false;
    bool isDigging = false;
    bool isPicking = false;
    bool isHarvest = false;
    bool isDie = false;

    public Transform handPoint; // 아이템을 줍기 위한 손의 위치
    public Transform pointH;
    private GameObject pickedItem; // 현재 주운 아이템
    public bool IsEating {  get { return isEating; } }
    public bool IsPickingUp { get {  return isPickingUp; } }
    public bool IsDigging { get {  return isDigging; } }
    public bool IsPicking { get { return isPicking; } }
    public bool IsHarvest { get { return isHarvest; } }
    public int ToolIndex { get { return toolIndex; } }
    public bool IsDie { get { return isDie; } set { isDie = value; } }

    float digDelay;
    float pickDelay;
    float harvestDelay;
    int toolIndex = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }
    public void getInput()
    {
        dDown = Input.GetButton("Fire2");
        fDown = Input.GetKeyDown(KeyCode.F);
        //eDown = Input.GetKeyDown(KeyCode.E);
        sDown1 = Input.GetKeyDown(KeyCode.Alpha1); // 1번 키
        sDown2 = Input.GetKeyDown(KeyCode.Alpha2); // 2번 키
        sDown3 = Input.GetKeyDown(KeyCode.Alpha3); // 3번 키
        sDown4 = Input.GetKeyDown(KeyCode.Alpha4); // 4번 키
        sDown5 = Input.GetKeyDown(KeyCode.Alpha5); // 5번 키
        sDown6 = Input.GetKeyDown(KeyCode.Alpha6);
        sDown7 = Input.GetKeyDown(KeyCode.Alpha7);
        sDown0 = Input.GetKeyDown(KeyCode.Alpha0);
    }

    // 도구 사용 및 행동 로직
    public void Use()
    {
        digDelay += Time.deltaTime;
        pickDelay += Time.deltaTime;
        harvestDelay = Time.deltaTime;
        isDigReady = 1.5f < digDelay;
        isPickReady = 1.2f < pickDelay;
        isHarvestReady = 0.4f < harvestDelay;

        if(toolIndex == 0 && dDown && !player.pMove.IsJumping && !player.pAttack.IsAttacking)
        {// 음식 먹기
            isEating = true;
            anim.SetTrigger("doEat");
            StartCoroutine(ResetEat());
        }
        else if (toolIndex == 0 && fDown && !isPickingUp && !isHarvest && !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking)
        {
            // 원형 범위로 아이템 감지 (OverlapSphere 사용)
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.3f); // 플레이어 주변 1미터 범위

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("fruit") || hitCollider.CompareTag("crop")) // 과일 태그가 있는지 확인 (있다면 pickedItem에 해당 프리펩 저장)
                {
                    pickedItem = hitCollider.gameObject;
                    isPickingUp = true;
                    player.pMove.IsTired = true;
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
        //else if (toolIndex == 0 && eDown && !isPickingUp && !player.pMove.IsJumping && !player.pAttack.IsAttacking && !player.pMove.IsWalking)
        //{
        //    // E 키를 눌렀을 때 문 열기 시도
        //    Ray ray = new Ray(transform.position, transform.forward);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, 3.0f)) // 플레이어 앞 3미터 거리 체크
        //    {
        //        if (hit.collider.CompareTag("Door"))
        //        {
        //            StartCoroutine(OpenAndCloseDoor(hit.collider.gameObject)); // 문 여닫기 애니메이션 실행
        //        }
        //    }
        //}
        else if ((toolIndex == 1 || toolIndex == 3) && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {// 경작하기
            anim.SetTrigger("doDigDown");
            isDigging = true;
            digDelay = 0f;
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
            StartCoroutine(ResetDig());
        }
        else if(toolIndex == 5 && dDown && isDigReady && !isHarvest && !player.pAttack.IsAttacking && !player.pMove.IsJumping && !isPicking)
        {
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
        int currentIndex = toolIndex;
        if (sDown1) // 주먹
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 0;
        }
        if (sDown2) // 괭이
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 1;
        }
        if (sDown3) // 삼지창(과일 수확용)
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 2;
        }
        if (sDown4) // 곡괭이
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 3;
        }
        if (sDown5) // 삽
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 4;
        }
        if (sDown6) // 낫
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 5;
        }
        if (sDown7) // 검
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 6;
        }
        if (sDown0) // 이스터에그
        {
            tools[currentIndex].gameObject.SetActive(false);
            toolIndex = 7;
        }

        tools[toolIndex].gameObject.SetActive(true);
    }
    IEnumerator ResetDig()
    {
        yield return new WaitForSeconds(1.5f); 
        isDigging = false;
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
            Destroy(pickedItem);
            pickedItem = null; // Reset the reference to the item
        }
    }

    IEnumerator ResetEat()
    {
        yield return new WaitForSeconds(1.5f);
        isEating = false;
    }

    IEnumerator ResetHarvest()
    {
        yield return new WaitForSeconds(1f);
        pointH.gameObject.SetActive(false);
        isHarvest = false;
    }
    IEnumerator Picking()
    {
        // 아이템을 손 위치로 이동
        yield return new WaitForSeconds(0.5f);
        pickedItem.transform.position = handPoint.position;
        pickedItem.transform.rotation = handPoint.rotation; // 손의 회전과 맞춤
        pickedItem.transform.parent = handPoint; // 아이템을 손에 붙임
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
                player.health -= 3;
            }
            else if (other.tag == "BossRock")
            {
                anim.SetTrigger("doHit");
                player.health -= 5;
            }
            else if(other.tag == "EnemyAttack")
            {
                anim.SetTrigger("doHit");
                player.health -= 1;
            }
            if (player.health <= 0)
            {
                Die();
            }
        }
    }
}
