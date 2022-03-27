using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{
    [SerializeField] Sprite altImg;
    Image spr;

    void Awake()
    {
        spr = GetComponent<Image>();
    }

    public void SwapImage()
    {
        spr.sprite = altImg;
    }

}
