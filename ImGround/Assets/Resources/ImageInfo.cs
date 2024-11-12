using UnityEngine;

/// <summary>
/// 이미지 정보입니다.
/// </summary>
public class ImageInfo
{
    public ImageIdEnum id;
    public Sprite img;
    public string resourcePath;

    /// <summary>
    /// 이미지 정보를 생성합니다. 아직 이미지는 로드되지 않았습니다.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="resourcePath"></param>
    public ImageInfo(ImageIdEnum id, string resourcePath)
    {
        this.id = id;
        this.img = null;
        this.resourcePath = resourcePath;
    }

    /// <summary>
    /// 이미지를 로딩합니다. 만약 실패하면 false를 반환합니다.
    /// </summary>
    /// <returns></returns>
    public bool load()
    {
        if (resourcePath != null)
        {
            this.img = Resources.Load<Sprite>(resourcePath);
            if (img == null)
            {
                return false;
            }
        }
        return true;
    }
}
