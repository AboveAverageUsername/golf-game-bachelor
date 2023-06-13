using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;


public class ScreenShotController : MonoBehaviour {

    public RenderTexture myTexture;
    public GameObject screenshotCamera;

    public void captureSceenshot()
    {
        StartCoroutine("TakeSnapshot");
    }

    IEnumerator TakeSnapshot()
    {
        yield return new WaitForEndOfFrame();



        //Vector2 size = Vector2.Scale(gameObject.GetComponent<RectTransform>().rect.size, gameObject.GetComponent<RectTransform>().lossyScale);
        //Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        //Rect rect = new Rect(0, 0, Screen.width / 3, Screen.height / 3);

        //screenshotTexture.ReadPixels(rect, 0, 0, false);
        //screenshotTexture.Apply();


        //byte[] byteArray = screenshotTexture.EncodeToPNG();
        //System.IO.File.WriteAllBytes(Application.dataPath + "/Screenshot.png", byteArray);



        //Texture2D renderResult = new Texture2D(myTexture.width, myTexture.height, TextureFormat.ARGB32, false);
        //Rect rect = new Rect(0, 0, myTexture.width, myTexture.height);
        //renderResult.ReadPixels(rect, 0, 0);
        //byte[] byteArray = renderResult.EncodeToPNG();
        //System.IO.File.WriteAllBytes(Application.dataPath + "/Screenshot.png", byteArray);



        screenshotCamera.SetActive(true);
        int width = Mathf.FloorToInt(Screen.height * 1.23f);

        RenderTexture rt = new RenderTexture(width, Screen.height, 24);
        screenshotCamera.GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, Screen.height, TextureFormat.RGB24, false);
        screenshotCamera.GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, Screen.height), 0, 0);
        screenshotCamera.GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        GameDataController.controller.chosenStage.stagePreview = Convert.ToBase64String(bytes);

        screenshotCamera.SetActive(false);
    }
}

