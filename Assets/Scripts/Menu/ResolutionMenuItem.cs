using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ResolutionMenuItem : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        var dropDownList = GetComponentInChildren<Canvas>();
        if (!dropDownList) return;

        // If the dropdown was opened find the options toggles
        var toggles = dropDownList.GetComponentsInChildren<Toggle>(true);
        // Debug.Log("Resolution options: " + toggles.Length);
        // Debug.Log("System width: " + Display.main.systemWidth + ", system height: " + Display.main.systemHeight);
        for (var i = 1; i < toggles.Length; i++)
        {
            // Debug.Log("Resolution width: " + GameDataController.controller.resolutions[i - 1, 0] + ", resolution height: " + GameDataController.controller.resolutions[i - 1, 1]);
            if (GameDataController.controller.resolutions[i - 1, 0] > Display.main.systemWidth || GameDataController.controller.resolutions[i - 1, 1] > Display.main.systemHeight)
            {
                toggles[i].interactable = false;
                 // Debug.Log(toggles[i].interactable ? "true" : "false");
            }
        }
    }

}
