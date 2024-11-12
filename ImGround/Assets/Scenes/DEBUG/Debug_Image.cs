using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debug_Image : MonoBehaviour
{
    private Sprite img;
    [SerializeField]
    private TextMeshPro text;
    [SerializeField]
    private SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        sp.sprite = img;
    }

    public void setImage(Vector3 position, Sprite image, string description)
    {
        transform.position = position;
        this.img = image;
        text.text = description;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        sp.gameObject.transform.Rotate(0, 90 * Time.deltaTime, 0);
    }
}
