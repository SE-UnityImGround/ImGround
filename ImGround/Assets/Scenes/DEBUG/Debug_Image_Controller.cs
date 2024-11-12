using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_Image_Controller : MonoBehaviour
{
    [SerializeField]
    private const float distance = 5.0f; // horizontal
    [SerializeField]
    private const float height = 5.75f; // vertical
    private const int maxColLen = 15;
    [SerializeField]
    private Debug_Image displayPrefab;
    [SerializeField]
    private showType type;

    private enum showType
    {
        IMAGE,
        ITEM
    }

    private Type getType()
    {
        switch (type)
        {
            case showType.IMAGE:
                return typeof(ImageIdEnum);
            case showType.ITEM:
                return typeof(ItemIdEnum);
            default:
                return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Array ids = Enum.GetValues(getType());
        int idx = 0;
        Vector3 initPos = 
            transform.position
            + new Vector3(
                - maxColLen / 2.0f * distance,
                (ids.Length / maxColLen + 1) * height,
                0);
        Vector3 currentPos = initPos;

        foreach (object id in ids)
        {
            string description;
            Sprite img;
            getData(out img, out description, type, id);

            currentPos.x = initPos.x + (idx % maxColLen) * distance;
            currentPos.y = initPos.y - (idx / maxColLen) * height;
            Instantiate(displayPrefab.gameObject, gameObject.transform).GetComponent<Debug_Image>()
                .setImage(currentPos, img, description);
            idx++;
        }
    }

    private void getData(out Sprite img, out string description, showType type, object id)
    {
        switch (type)
        {
            case showType.IMAGE:
                getImageData(out img, out description, id);
                break;
            case showType.ITEM:
                getItemData(out img, out description, id);
                break;
            default:
                throw new Exception("처리되지 않은 코드!");
                break;
        }
    }

    private void getImageData(out Sprite img, out string description, object dataId)
    {
        ImageIdEnum id = (ImageIdEnum)dataId;

        try
        {
            img = ImageManager.getImage(id);
            description = "id : " + id.ToString();
        }
        catch (Exception e)
        {
            img = null;
            description = "이미지 미등록 오류\nid: " + id.ToString();
        }
    }

    private void getItemData(out Sprite img, out string description, object dataId)
    {
        ItemIdEnum id = (ItemIdEnum)dataId;

        try
        {
            description = ItemInfoManager.getItemName(id) + "\nid : " + id.ToString();
            try
            {
                img = ItemInfoManager.getItemImage(id);
            }
            catch (Exception e)
            {
                img = null;
                description = description + "\n아이템 이미지 미등록 오류\nid: " + id.ToString();
            }
        }
        catch (Exception e)
        {
            img = null;
            description = "아이템 정보 미등록 오류\nid: " + id.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
