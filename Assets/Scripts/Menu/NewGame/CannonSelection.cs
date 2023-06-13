using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class CannonSelection : MonoBehaviour
{

    public TextAsset cannonJSON;
    public TextMeshProUGUI cannonName;
    public GameObject powerBar;
    public GameObject chargeBar;
    public Image cannonImage;

    [System.Serializable]
    public class Cannon
    {
        public string name;
        public float maxSpeed;
        public float maxHold;
        public string imageName;
    }

    [System.Serializable]
    public class CannonList
    {
        public Cannon[] cannon;
    }

    private CannonList myCannonList = new CannonList();

    private float maxSpeed;
    private float maxHold;
    private int cannonIndex;

    // Start is called before the first frame update
    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Cannons.json");
        StreamReader reader = new StreamReader(path);
        myCannonList = JsonUtility.FromJson<CannonList>(reader.ReadToEnd());
        //myCannonList = JsonUtility.FromJson<CannonList>(cannonJSON.text);
        maxSpeed = myCannonList.cannon[0].maxSpeed;
        maxHold = myCannonList.cannon[0].maxHold;
        cannonName.text = myCannonList.cannon[1].name;
        powerBar.GetComponent<Image>().fillAmount = myCannonList.cannon[1].maxSpeed / maxSpeed;
        chargeBar.GetComponent<Image>().fillAmount = 1 - myCannonList.cannon[1].maxHold / maxHold;
        cannonImage.sprite = Resources.Load<Sprite>("Images/" + myCannonList.cannon[1].imageName);
        cannonIndex = 1;
    }



    public void arrowLeftClick()
    {
        clickArrowSound();
        cannonIndex = cannonIndex - 1;
        if (cannonIndex == 0) cannonIndex = myCannonList.cannon.Length - 1;

        cannonName.text = myCannonList.cannon[cannonIndex].name;
        powerBar.GetComponent<Image>().fillAmount = myCannonList.cannon[cannonIndex].maxSpeed / maxSpeed;
        chargeBar.GetComponent<Image>().fillAmount = 1 - myCannonList.cannon[cannonIndex].maxHold / maxHold;
        cannonImage.sprite = Resources.Load<Sprite>("Images/" + myCannonList.cannon[cannonIndex].imageName);
    }

    public void arrowRightClick()
    {
        clickArrowSound();
        cannonIndex = (cannonIndex + 1) % myCannonList.cannon.Length;
        if (cannonIndex == 0) cannonIndex = 1;

        cannonName.text = myCannonList.cannon[cannonIndex].name;
        powerBar.GetComponent<Image>().fillAmount = myCannonList.cannon[cannonIndex].maxSpeed / maxSpeed;
        chargeBar.GetComponent<Image>().fillAmount = 1 - myCannonList.cannon[cannonIndex].maxHold / maxHold;
        cannonImage.sprite = Resources.Load<Sprite>("Images/" + myCannonList.cannon[cannonIndex].imageName);
    }

    public void startGame()
    {
        PlayerPrefs.SetFloat("maxSpeed", myCannonList.cannon[cannonIndex].maxSpeed);
        PlayerPrefs.SetFloat("maxHold", myCannonList.cannon[cannonIndex].maxHold);
        PlayerPrefs.SetString("spriteName", "Images/" + myCannonList.cannon[cannonIndex].imageName);
    }

    public void clickArrowSound()
    {
        AudioManager.Instance.Play("BtnClk");
    }
}
