using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cursor : MonoBehaviour
{
    private Image cursorImage;
    [SerializeField]
    private Sprite defaultSprite;

    void Start()
    {
        cursorImage = GetComponent<Image>();
        cursorImage.sprite = defaultSprite;
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetSprite(Sprite inSprite)
    {
        cursorImage.sprite = inSprite;
    }

    public void ResetSprite()
    {
        cursorImage.sprite = defaultSprite;
    }
}
