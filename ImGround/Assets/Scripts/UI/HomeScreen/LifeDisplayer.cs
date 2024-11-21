using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplayer : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text lifeText;
    [SerializeField]
    private Image lifeImage;

    public void initialize()
    {
        if (lifeText == null)
        {
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, nameof(lifeText));
            return;
        }
        if (lifeImage == null)
        {
            Debug.LogErrorFormat("{0}에 {1}가 등록되지 않았습니다!", this.GetType().Name, nameof(lifeImage));
            return;
        }
    }

    public void update(float lifeRatio)
    {
        lifeText.text = string.Format("{0:#}%", lifeRatio * 100);
        lifeImage.sprite = getLifeImage(lifeRatio);
    }

    private Sprite getLifeImage(float ratioValue)
    {
        if (ratioValue > 0.90f)
        {
            return ImageManager.getImage(ImageIdEnum.UI_LIFE_100);
        }
        if (ratioValue > 0.65f)
        {
            return ImageManager.getImage(ImageIdEnum.UI_LIFE_80);
        }
        if (ratioValue > 0.40f)
        {
            return ImageManager.getImage(ImageIdEnum.UI_LIFE_50);
        }
        if (ratioValue > 0.15f)
        {
            return ImageManager.getImage(ImageIdEnum.UI_LIFE_30);
        }
        return ImageManager.getImage(ImageIdEnum.UI_LIFE_0);
    }
}
