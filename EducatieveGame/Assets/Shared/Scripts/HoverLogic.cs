using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverLogic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 1);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 0.4f);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }
}
