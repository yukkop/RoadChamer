using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickButtonEvent : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    public Controler.ActionsNames action;
    public ClickEvent clickButton;

    Image image;
    public Sprite upHandler, downHandler;


    public void OnPointerClick(PointerEventData eventData)
    {
        clickButton.Invoke(action);
    }

    public void Start()
    {
        image = this.gameObject.GetComponent<Image>();
    }

    public void PressOut()
    {
        image.sprite = upHandler;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = (image.sprite == upHandler) ? downHandler : upHandler;
    }
}

[System.Serializable]
public class ClickEvent : UnityEvent<Controler.ActionsNames> { }
