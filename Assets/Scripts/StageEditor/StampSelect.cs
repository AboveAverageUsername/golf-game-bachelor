using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StampSelect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public StampController controller;
    private bool btnClicked = false;
    private Vector2 clickPos = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(selectStamp);
    }


    public void selectStamp()
    {
        //if(controller.currentHold == null && controller.currentStamp == null)
        //    controller.selectStamp(name);

        if (controller.currentHold == null && !btnClicked)
        {
            if (controller.singleStamp != null) controller.deselectSingle();
            if (controller.selectedListElement == gameObject)
            {
                if (controller.currentStamp != null) controller.deselectStamp();
            }
            else
            {
                controller.selectStamp(name, false);
                if (controller.selectedListElement != null)
                    controller.selectedListElement.GetComponent<Image>().color = Color.white;
                controller.selectedListElement = gameObject;
                gameObject.GetComponent<Image>().color = Color.black;
            }
        }
        btnClicked = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        btnClicked = true;
        if (controller.currentHold == null)
        {
            controller.selectStamp(name, true);
            if (controller.selectedListElement != null)
                controller.selectedListElement.GetComponent<Image>().color = Color.white;
            controller.selectedListElement = gameObject;
            gameObject.GetComponent<Image>().color = Color.black;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        btnClicked = false;
    }
}
