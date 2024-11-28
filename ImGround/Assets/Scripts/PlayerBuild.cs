using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerBuild : MonoBehaviour
{
    public GameObject go_Prefab; // 실제 설치될 프리펩
    private GameObject go_Preview;
    public GameObject go_PreviewPrefab; // 미리보기 프리펩

    private bool isPreviewActivated = false;
    private Player player;
    public Transform tf_Player;
    // Raycast 필요 변수 선언
    private RaycastHit hitInfo;
    private LayerMask layerMask;
    private float range = 2f;
    void Awake()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.pBehavior.ItemPoint.childCount > 0 && !isPreviewActivated)
        {
            ItemPrefabID bed = player.pBehavior.ItemPoint.GetComponentInChildren<ItemPrefabID>();
            bool isBed = bed.itemType == ItemIdEnum.BED;
            if (isBed)
            {
                go_Preview = Instantiate(go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
                isPreviewActivated = true;
            }
        }
        if (isPreviewActivated)
        {
            PreviewPositionUpdate();

            // R 키 입력 감지
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotatePreview();
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Build();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }
    void Build()
    {
        if (isPreviewActivated)
        {
            Instantiate(go_Prefab, go_Preview.transform.position, go_Preview.transform.rotation);
            Destroy(go_Preview);
            isPreviewActivated = false;
            go_Preview = null;
            InventoryManager.takeItem(player.pBehavior.GrabSlotID, 1);
        }
    }
    void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isPreviewActivated = false;
        go_Preview = null;
    }
    void PreviewPositionUpdate()
    {
        Vector3 previewPosition = tf_Player.position + tf_Player.forward * range * 1.5f;
        Vector3 correctedPosition = new Vector3(previewPosition.x, tf_Player.position.y, previewPosition.z);
        go_Preview.transform.position = correctedPosition;

        // 충돌 여부 확인
        CheckCollision collisionChecker = go_Preview.GetComponentInChildren<CheckCollision>();
        if (collisionChecker != null)
        {
            SetPreviewColor(collisionChecker.isColliding ? Color.red : Color.green);
        }
    }

    void SetPreviewColor(Color color)
    {
        Renderer renderer = go_Preview.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            foreach (var mat in renderer.materials)
            {
                mat.color = color;
            }
        }
    }


    void RotatePreview()
    {
        if (go_Preview != null)
        {
            // 기존 각도에 90도 추가
            go_Preview.transform.Rotate(Vector3.up, 90f);
        }
    }
}