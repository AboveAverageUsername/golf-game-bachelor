using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class ListElement : MonoBehaviour
{
    public GameData.CustomStage myStage;
    public MenuController menu;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(stageSelect);
    }

    public void stageSelect()
    {
        GameDataController.controller.chosenStage = myStage;
        menu.stageListSelected = gameObject;
        menu.stageInfo.SetActive(true);
        menu.stageInfoText.text = myStage.stageName;
        Texture2D preview = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        preview.LoadImage(Convert.FromBase64String(GameDataController.controller.chosenStage.stagePreview));
        menu.stageInfoImage.GetComponent<RawImage>().texture =  preview;
    }

}
