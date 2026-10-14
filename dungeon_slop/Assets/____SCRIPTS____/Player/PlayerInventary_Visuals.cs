using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventary_Visuals : MonoBehaviour
{
    [SerializeField] List<Image> imageList = new List<Image>();

    public void ShowItem(ItemController item)
    {
        foreach (Image image in imageList)
        {
            if(image != null && !image.gameObject.activeSelf)
            {
                image.sprite = item.visuals.spriteUI;
                image.gameObject.SetActive(true);
                return;
            }
        }
    }

    internal void HideItem(ItemController item)
    {
        foreach (Image image in imageList)
        {
            if (image != null && image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(false);
                return;
            }
        }
    }
}
