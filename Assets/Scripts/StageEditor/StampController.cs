using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StampController : MonoBehaviour
{

    public ScreenShotController screenshotController;
    public GameObject saveBtn;
    public GameObject backBtn;
    public TextMeshProUGUI nameText;
    public GameObject nameInputField;
    public GameObject coinInputField;
    public GameObject exitMenu;
    public GameObject overwriteWindow;
    public GameData.CustomStage overwrittenStage = null;

    public GameObject singleStamp = null;
    private GameObject selectedStamp = null;
    public GameObject selectedListElement = null;
    public GameObject currentStamp = null;
    public GameObject currentHold = null;
    private Vector3 holdPosition = Vector3.zero;
    private Vector3 cursorClickDelta = Vector3.zero;
    public GameObject trashCan;

    private int holeNum;
    private GameObject firstTeleporter;

    // All stamps
    public GameObject Tree_1;
    public GameObject Tree_2;
    public GameObject Tree_3;
    public GameObject Tree_4;
    public GameObject Stump;
    public GameObject Log;
    public GameObject Bush_1;
    public GameObject Bush_2;
    public GameObject Bush_3;
    public GameObject Hole_Yellow;
    public GameObject Hole_Orange;
    public GameObject Hole_Red;
    public GameObject Ice_Small_1;
    public GameObject Ice_Medium_1;
    public GameObject Ice_Big_1;
    public GameObject Mud_Small_1;
    public GameObject Mud_Medium_1;
    public GameObject Mud_Big_1;
    public GameObject Teleporter;
    public GameObject Bird_Flying;
    public GameObject Lake_Small_1;
    public GameObject Lake_Small_2;
    public GameObject Lake_Small_3;
    public GameObject Lake_Medium_1;
    public GameObject Lake_Medium_2;
    public GameObject Lake_Big_1;
    //public GameObject CoinSpawner;
    private int coinNum;

    private List<GameObject> allElements;

    ContactFilter2D colFltr = new ContactFilter2D();
    [SerializeField] private LayerMask colMask;

    private bool changeMade = false;


    private GameObject clickedOnElem = null;
    private float clickStart;
    private const float clickTime = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        allElements = new List<GameObject>();
        exitMenu.SetActive(false);
        overwriteWindow.SetActive(false);
        holeNum = 0;

        if (GameDataController.controller.chosenStage != null)
        {
            nameInputField.GetComponent<TMP_InputField>().text = GameDataController.controller.chosenStage.stageName;
            coinInputField.GetComponent<TMP_InputField>().text = GameDataController.controller.chosenStage.numOfCoins.ToString();
            foreach (GameData.StageObstacle elem in GameDataController.controller.chosenStage.allElements)
            {
                GameObject obstacle = null;
                switch (elem.name)
                {
                    case "Tree_1":
                        obstacle = Tree_1;
                        break;
                    case "Tree_2":
                        obstacle = Tree_2;
                        break;
                    case "Tree_3":
                        obstacle = Tree_3;
                        break;
                    case "Tree_4":
                        obstacle = Tree_4;
                        break;
                    case "Stump":
                        obstacle = Stump;
                        break;
                    case "Log":
                        obstacle = Log;
                        break;
                    case "Bush_1":
                        obstacle = Bush_1;
                        break;
                    case "Bush_2":
                        obstacle = Bush_2;
                        break;
                    case "Bush_3":
                        obstacle = Bush_3;
                        break;
                    case "Hole_Yellow":
                        obstacle = Hole_Yellow;
                        holeNum++;
                        break;
                    case "Hole_Orange":
                        obstacle = Hole_Orange;
                        holeNum++;
                        break;
                    case "Hole_Red":
                        obstacle = Hole_Red;
                        holeNum++;
                        break;
                    case "Ice_Small_1":
                        obstacle = Ice_Small_1;
                        break;
                    case "Ice_Medium_1":
                        obstacle = Ice_Medium_1;
                        break;
                    case "Ice_Big_1":
                        obstacle = Ice_Big_1;
                        break;
                    case "Mud_Small_1":
                        obstacle = Mud_Small_1;
                        break;
                    case "Mud_Medium_1":
                        obstacle = Mud_Medium_1;
                        break;
                    case "Mud_Big_1":
                        obstacle = Mud_Big_1;
                        break;
                    case "Teleporter":
                        obstacle = Teleporter;
                        break;
                    case "Bird_Flying":
                        obstacle = Bird_Flying;
                        break;
                    case "Lake_Small_1":
                        obstacle = Lake_Small_1;
                        break;
                    case "Lake_Small_2":
                        obstacle = Lake_Small_2;
                        break;
                    case "Lake_Small_3":
                        obstacle = Lake_Small_3;
                        break;
                    case "Lake_Medium_1":
                        obstacle = Lake_Medium_1;
                        break;
                    case "Lake_Medium_2":
                        obstacle = Lake_Medium_2;
                        break;
                    case "Lake_Big_1":
                        obstacle = Lake_Big_1;
                        break;
                    default:
                        break;
                }
                GameObject addedElement = Instantiate(obstacle, elem.position, Quaternion.Euler(Vector3.zero));
                int index = addedElement.name.IndexOf("_Stamp");
                if (index >= 0)
                    addedElement.name = addedElement.name.Substring(0, index);
                if (addedElement.tag.Contains("Tree") || addedElement.tag.Contains("Obstacle"))
                {
                    addedElement.GetComponent<SpriteRenderer>().sortingOrder = setTreeLayerOrder(addedElement.transform);
                }
                allElements.Add(addedElement);
            }
        }
        changeMade = false;
        colFltr.NoFilter();
        colFltr.SetLayerMask(colMask);
        colFltr.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentStamp != null)
        {
            currentStamp.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

            if (currentStamp.tag.Contains("Tree") || currentStamp.tag.Contains("Obstacle"))
            {
                currentStamp.GetComponent<SpriteRenderer>().sortingOrder = setTreeLayerOrder(currentStamp.transform);
                //Debug.Log(currentStamp.GetComponent<SpriteRenderer>().sortingOrder);
            }


            List<Collider2D> colRes = new List<Collider2D>();
            Collider2D col = currentStamp.GetComponents<Collider2D>()[1];
            //Debug.Log(col.OverlapCollider(colFltr, colRes));
            if (col.OverlapCollider(colFltr, colRes) > 0)
            {
                bool hasCollision = false;
                foreach (Collider2D element in colRes)
                {
                    //Debug.Log(element.gameObject.name);
                    if(!checkOverlap(currentStamp, element))
                    {
                        currentStamp.GetComponent<SpriteRenderer>().color = Color.red;
                        hasCollision = true;
                        break;
                    }
                    
                }
                if (!hasCollision)
                {
                    currentStamp.GetComponent<SpriteRenderer>().color = Color.white;
                    if (Input.GetMouseButtonUp(0))
                    {
                        successfulPlace();
                        GameObject newElem = Instantiate(selectedStamp, currentStamp.transform.position, currentStamp.transform.rotation);
                        if (selectedStamp.name.Contains("Hole"))
                            holeNum++;
                        int index = newElem.name.IndexOf("_Stamp");
                        if (index >= 0)
                            newElem.name = newElem.name.Substring(0, index);
                        allElements.Add(newElem);
                        if (newElem.name == "Teleporter")
                        {
                            if (firstTeleporter == null) firstTeleporter = newElem;
                            else
                            {
                                firstTeleporter.GetComponent<Teleporter>().destination = newElem;
                                newElem.GetComponent<Teleporter>().destination = firstTeleporter;
                                firstTeleporter = null;
                            }
                        }
                        else if (newElem.tag.Contains("Tree") || newElem.tag.Contains("Obstacle"))
                        {
                            newElem.GetComponent<SpriteRenderer>().sortingOrder = setTreeLayerOrder(newElem.transform);
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    failPlace();
                }
            }
            else
            {
                currentStamp.GetComponent<SpriteRenderer>().color = Color.white;
                if (Input.GetMouseButtonUp(0))
                {
                    successfulPlace();
                    GameObject newElem = Instantiate(selectedStamp, currentStamp.transform.position, currentStamp.transform.rotation);
                    if (selectedStamp.name.Contains("Hole"))
                        holeNum++;
                    int index = newElem.name.IndexOf("_Stamp");
                    if (index >= 0)
                        newElem.name = newElem.name.Substring(0, index);
                    allElements.Add(newElem);
                    if (newElem.name == "Teleporter")
                    {
                        if (firstTeleporter == null) firstTeleporter = newElem;
                        else
                        {
                            firstTeleporter.GetComponent<Teleporter>().destination = newElem;
                            newElem.GetComponent<Teleporter>().destination = firstTeleporter;
                            firstTeleporter = null;
                        }
                    } else if (newElem.tag.Contains("Tree") | newElem.tag.Contains("Obstacle"))
                    {
                        newElem.GetComponent<SpriteRenderer>().sortingOrder = setTreeLayerOrder(newElem.transform);
                    }
                    
                }
            }

            //if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
            //|| Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                cancelPlace();
                Destroy(currentStamp);
                currentStamp = null;
                selectedStamp = null;
                selectedListElement.GetComponent<Image>().color = Color.white;
                selectedListElement = null;
                if (firstTeleporter != null)
                {
                    allElements.Remove(firstTeleporter);
                    Destroy(firstTeleporter);
                }
            }


        }
        else if(currentHold != null)
        {

            //currentHold.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
            currentHold.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)) - cursorClickDelta;


            if (currentHold.CompareTag("Tree") || currentHold.tag.Contains("Obstacle"))
            {
                currentHold.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(currentHold.transform.position.y * 100f) * -1;
                //Debug.Log(currentStamp.GetComponent<SpriteRenderer>().sortingOrder);
            }

            //if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
            //|| Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
            if(Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    allElements.Remove(currentHold);
                    if (currentHold.name.Contains("Hole"))
                    {
                        holeNum--;
                    }
                    if(currentHold.name == "Teleporter")
                    {
                        allElements.Remove(currentHold.GetComponent<Teleporter>().destination);
                        Destroy(currentHold.GetComponent<Teleporter>().destination);
                    }
                    Destroy(currentHold);
                }
                else if(Input.GetKeyDown(KeyCode.Escape))
                {
                    currentHold.transform.position = holdPosition;
                }
                currentHold = null;
                holdPosition = Vector3.zero;
                trashCan.SetActive(false);
                cancelPlace();
            }
            else
            {

                List<Collider2D> colRes = new List<Collider2D>();
                Collider2D col = currentHold.GetComponents<Collider2D>()[1];
                if (col.OverlapCollider(colFltr, colRes) > 0)
                {
                    bool hasCollision = false;
                    foreach (Collider2D element in colRes)
                    {
                        if (!checkOverlap(currentHold, element))
                        {
                            currentHold.GetComponent<SpriteRenderer>().color = Color.red;
                            hasCollision = true;
                            break;
                        }
                    }
                    if (!hasCollision)
                    {
                        currentHold.GetComponent<SpriteRenderer>().color = Color.white;
                        if (Input.GetMouseButtonUp(0))
                        {
                            successfulPlace();
                            currentHold = null;
                            holdPosition = Vector3.zero;
                            trashCan.SetActive(false);
                        }
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        failPlace();
                    }
                }
                else
                {
                    currentHold.GetComponent<SpriteRenderer>().color = Color.white;
                    if (Input.GetMouseButtonUp(0))
                    {
                        successfulPlace();
                        currentHold = null;
                        holdPosition = Vector3.zero;
                        trashCan.SetActive(false);
                    }
                }
            }

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 5);

            //if (hit.collider != null)
            //{
            //    Debug.Log(hit.collider.gameObject.name);
            //}


            if (Input.GetMouseButtonUp(0) && currentHold != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.NameToLayer("UI"));
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject == trashCan)
                    {
                        if (currentHold.name.Contains("Hole"))
                        {
                            holeNum--;
                        }
                        if (currentHold.name == "Teleporter")
                        {
                            allElements.Remove(currentHold.GetComponent<Teleporter>().destination);
                            Destroy(currentHold.GetComponent<Teleporter>().destination);
                        }
                        allElements.Remove(currentHold);
                        Destroy(currentHold);
                        currentHold = null;
                        holdPosition = Vector3.zero;
                        trashCan.SetActive(false);
                        cancelPlace();
                    }
                }
            }


        }
        else if(singleStamp != null)
        {
            singleStamp.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));


            if (singleStamp.CompareTag("Tree") || singleStamp.tag.Contains("Obstacle"))
            {
                singleStamp.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(singleStamp.transform.position.y * 100f) * -1;
            }

            List<Collider2D> colRes = new List<Collider2D>();
            Collider2D col = singleStamp.GetComponents<Collider2D>()[1];
            if (col.OverlapCollider(colFltr, colRes) > 0)
            {
                bool hasCollision = false;
                foreach (Collider2D element in colRes)
                {
                    if (!checkOverlap(singleStamp, element))
                    {
                        singleStamp.GetComponent<SpriteRenderer>().color = Color.red;
                        hasCollision = true;
                        break;
                    }

                }
                if (!hasCollision)
                {
                    singleStamp.GetComponent<SpriteRenderer>().color = Color.white;
                    if (Input.GetMouseButtonUp(0))
                    {
                        successfulPlace();
                        GameObject newElem = Instantiate(selectedStamp, singleStamp.transform.position, singleStamp.transform.rotation);
                        if (selectedStamp.name.Contains("Hole"))
                            holeNum++;
                        int index = newElem.name.IndexOf("_Stamp");
                        if (index >= 0)
                            newElem.name = newElem.name.Substring(0, index);
                        allElements.Add(newElem);
                        if (newElem.name == "Teleporter")
                        {
                            if (firstTeleporter == null) firstTeleporter = newElem;
                            else
                            {
                                firstTeleporter.GetComponent<Teleporter>().destination = newElem;
                                newElem.GetComponent<Teleporter>().destination = firstTeleporter;
                                firstTeleporter = null;
                                deselectSingle();
                            }
                        }
                        else 
                        {
                            if (newElem.tag.Contains("Tree") || newElem.tag.Contains("Obstacle"))
                                newElem.GetComponent<SpriteRenderer>().sortingOrder = setTreeLayerOrder(newElem.transform);
                            deselectSingle();
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    deselectSingle();
                    
                }
            }
            else
            {
                singleStamp.GetComponent<SpriteRenderer>().color = Color.white;
                if (Input.GetMouseButtonUp(0))
                {
                    successfulPlace();
                    GameObject newElem = Instantiate(selectedStamp, singleStamp.transform.position, singleStamp.transform.rotation);
                    if (selectedStamp.name.Contains("Hole"))
                        holeNum++;
                    int index = newElem.name.IndexOf("_Stamp");
                    if (index >= 0)
                        newElem.name = newElem.name.Substring(0, index);
                    allElements.Add(newElem);
                    if (newElem.name == "Teleporter")
                    {
                        if (firstTeleporter == null) firstTeleporter = newElem;
                        else
                        {
                            firstTeleporter.GetComponent<Teleporter>().destination = newElem;
                            newElem.GetComponent<Teleporter>().destination = firstTeleporter;
                            firstTeleporter = null;
                            deselectSingle();
                        }
                    }
                    else
                    {
                        if (newElem.tag.Contains("Tree") || newElem.tag.Contains("Obstacle"))
                            newElem.GetComponent<SpriteRenderer>().sortingOrder = setTreeLayerOrder(newElem.transform);
                        deselectSingle();
                    }
                }
            }

            //if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
            //|| Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                deselectSingle();
            }


        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D[] allHits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, 64);
                if (allHits.Length > 0)
                {

                    RaycastHit2D topHit = allHits[0];
                    foreach (RaycastHit2D elem in allHits)
                    {
                        if(SortingLayer.GetLayerValueFromID(elem.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingLayerID) > SortingLayer.GetLayerValueFromID(topHit.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingLayerID)
                            || (SortingLayer.GetLayerValueFromID(elem.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingLayerID) == SortingLayer.GetLayerValueFromID(topHit.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingLayerID)
                            && elem.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder > topHit.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder))
                        {
                            topHit = elem;
                        }
                    }
                    if (allElements.Contains(topHit.collider.gameObject.transform.parent.gameObject) && !exitMenu.activeSelf && !overwriteWindow.activeSelf)
                    {
                        clickedOnElem = topHit.collider.gameObject;
                        clickStart = Time.time;
                    }
                }

                //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 64);

                //if (hit.collider != null && allElements.Contains(hit.collider.gameObject.transform.parent.gameObject) && !exitMenu.activeSelf && !overwriteWindow.activeSelf)
                //{
                //    clickedOnElem = hit.collider.gameObject;
                //    clickStart = Time.time;
                //}
            }
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D[] allHits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, 64);
                if (allHits.Length > 0)
                {

                    RaycastHit2D hit = allHits[0];
                    foreach (RaycastHit2D elem in allHits)
                    {
                        if (SortingLayer.GetLayerValueFromID(elem.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingLayerID) > SortingLayer.GetLayerValueFromID(hit.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingLayerID)
                            || (SortingLayer.GetLayerValueFromID(elem.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingLayerID) == SortingLayer.GetLayerValueFromID(hit.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingLayerID)
                            && elem.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder > hit.collider.gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder))
                        {
                            hit = elem;
                        }
                    }


                    if (allElements.Contains(hit.collider.gameObject.transform.parent.gameObject) && !exitMenu.activeSelf && !overwriteWindow.activeSelf)
                    {
                        if (clickedOnElem == hit.collider.gameObject && Time.time - clickStart <= clickTime)
                        {
                            selectStampSound();
                            currentHold = hit.collider.gameObject.transform.parent.gameObject;
                            holdPosition = currentHold.transform.position;
                            cursorClickDelta = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)) - holdPosition;
                            trashCan.SetActive(true);
                        }
                    }
                }

                //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 64);

                //if (hit.collider != null && allElements.Contains(hit.collider.gameObject.transform.parent.gameObject) && !exitMenu.activeSelf && !overwriteWindow.activeSelf)
                //{
                //    if (clickedOnElem == hit.collider.gameObject && Time.time - clickStart <= clickTime)
                //    {
                //        selectStampSound();
                //        currentHold = hit.collider.gameObject.transform.parent.gameObject;
                //        holdPosition = currentHold.transform.position;
                //        cursorClickDelta = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)) - holdPosition;
                //        trashCan.SetActive(true);
                //    }
                //}
            }
            
        }
    }

    public void selectStamp(string stampName, bool isSingle)
    {
        selectStampSound();
        switch (stampName)
            {
                case "Tree_1":
                    selectedStamp = Tree_1;
                    break;
                case "Tree_2":
                    selectedStamp = Tree_2;
                    break;
                case "Tree_3":
                    selectedStamp = Tree_3;
                    break;
                case "Tree_4":
                    selectedStamp = Tree_4;
                    break;
                case "Stump":
                    selectedStamp = Stump;
                    break;
                case "Log":
                    selectedStamp = Log;
                    break;
                case "Bush_1":
                    selectedStamp = Bush_1;
                    break;
                case "Bush_2":
                    selectedStamp = Bush_2;
                    break;
                case "Bush_3":
                    selectedStamp = Bush_3;
                    break;
                case "Hole_Yellow":
                    selectedStamp = Hole_Yellow;
                    break;
                case "Hole_Orange":
                    selectedStamp = Hole_Orange;
                    break;
                case "Hole_Red":
                    selectedStamp = Hole_Red;
                    break;
                case "Ice_Small_1":
                    selectedStamp = Ice_Small_1;
                    break;
                case "Ice_Medium_1":
                    selectedStamp = Ice_Medium_1;
                    break;
                case "Ice_Big_1":
                    selectedStamp = Ice_Big_1;
                    break;
                case "Mud_Small_1":
                    selectedStamp = Mud_Small_1;
                    break;
                case "Mud_Medium_1":
                    selectedStamp = Mud_Medium_1;
                    break;
                case "Mud_Big_1":
                    selectedStamp = Mud_Big_1;
                    break;
                case "Teleporter":
                    selectedStamp = Teleporter;
                    break;
                case "Bird_Flying":
                    selectedStamp = Bird_Flying;
                    break;
                case "Lake_Small_1":
                    selectedStamp = Lake_Small_1;
                    break;
                case "Lake_Small_2":
                    selectedStamp = Lake_Small_2;
                    break;
                case "Lake_Small_3":
                    selectedStamp = Lake_Small_3;
                    break;
                case "Lake_Medium_1":
                    selectedStamp = Lake_Medium_1;
                    break;
                case "Lake_Medium_2":
                    selectedStamp = Lake_Medium_2;
                    break;
                case "Lake_Big_1":
                    selectedStamp = Lake_Big_1;
                    break;
                //case "CoinSpawner":
                //    selectedStamp = CoinSpawner;
                //    break;
                default:
                    break;
            }
        if (currentStamp != null)
        {
            if (firstTeleporter != null) Destroy(firstTeleporter);
            Destroy(currentStamp);
            currentStamp = null;
        }
        if (!isSingle)
        {
            currentStamp = Instantiate(selectedStamp, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
            //currentStamp.GetComponent<SpriteRenderer>().sortingLayerName = "Stamp";
        }
        else
        {
            singleStamp = Instantiate(selectedStamp, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
        }
    }


    public bool checkOverlap(GameObject over, Collider2D under)
    {
        //Debug.Log(under.gameObject.GetComponents<Collider2D>().Length);
        if (over.CompareTag("Bird"))
        {
            switch (under.gameObject.tag)
            {
                case "Border":
                    return false;
                default:
                    return true;
            }
        } else if (over.CompareTag("Tree"))
        {
            switch (under.gameObject.tag)
            {
                case "Mud":
                case "Ice":
                case "Bird":
                    return true;
                default:
                    return false;
            }
        } else if (over.CompareTag("Obstacle"))
        {
            switch (under.gameObject.tag)
            {
                case "Mud":
                case "Ice":
                case "Bird":
                    return true;
                default:
                    return false;
            }
        } else if (over.CompareTag("Teleporter"))
        {
            switch (under.gameObject.tag)
            {
                case "Mud":
                case "Ice":
                case "Bird":
                    return true;
                default:
                    return false;
            }
        } else if (over.tag.Contains("Hole"))
        {
            switch (under.gameObject.tag)
            {
                case "Mud":
                case "Ice":
                case "Bird":
                    return true;
                default:
                    return false;
            }
        } else if (over.CompareTag("Lake"))
        {
            switch (under.gameObject.tag)
            {
                case "Bird":
                    return true;
                default:
                    return false;
            }
        } else if (over.CompareTag("Ice") || over.CompareTag("Mud"))
        {
            switch (under.gameObject.tag)
            {
                case "Lake":
                case "Mud":
                case "Ice":
                case "Border":
                    return false;
                default:
                    return true;
            }
        }
        return true;
    }

    public void deselectStamp()
    {
        cancelPlace();
        if(currentStamp != null) Destroy(currentStamp);
        currentStamp = null;
        selectedStamp = null;
        selectedListElement.GetComponent<Image>().color = Color.white;
        selectedListElement = null;
        if (firstTeleporter != null)
        {
            allElements.Remove(firstTeleporter);
            Destroy(firstTeleporter);
        }
    }

    public void deselectSingle()
    {
        cancelPlace();
        if (singleStamp != null) Destroy(singleStamp);
        singleStamp = null;
        selectedStamp = null;
        selectedListElement.GetComponent<Image>().color = Color.white;
        selectedListElement = null;
        if (firstTeleporter != null)
        {
            allElements.Remove(firstTeleporter);
            Destroy(firstTeleporter);
        }
    }

    public void saveStage()
    {

        //Debug.Log(nameInputField.GetComponent<TMP_InputField>().text);
        bool validName = true;
        if(holeNum < 1)
        {
            nameText.text = "Stage must have a hole!";
            nameText.color = Color.red;
            return;
        }
        if(nameInputField.GetComponent<TMP_InputField>().text == "")
        {
            nameText.text = "Stage name is required!";
            nameText.color = Color.red;
            return;
        }
        foreach (string elem in GameDataController.controller.gameStages)
        {
            if(elem.Equals(nameInputField.GetComponent<TMP_InputField>().text))
            {
                validName = false;
                //Debug.Log("Base stage " + nameInputField.GetComponent<TMP_InputField>().text);
                break;
            }
        }
        foreach (GameData.CustomStage elem in GameDataController.controller.data.customStages)
        {
            if (elem.stageName.Equals(nameInputField.GetComponent<TMP_InputField>().text))
            {

                if (GameDataController.controller.chosenStage == null) {
                    validName = false;
                    //Debug.Log("Custom stage " + nameInputField.GetComponent<TMP_InputField>().text);
                    break;
                }
                else
                {
                    if(elem.stageName != GameDataController.controller.chosenStage.stageName)
                    {
                        validName = false;
                        //Debug.Log("Custom stage " + nameInputField.GetComponent<TMP_InputField>().text);
                        break;
                    }
                }
            }
        }
        if (nameInputField.GetComponent<TMP_InputField>().text == GameDataController.controller.customStageName)
        {
            validName = false;
            //Debug.Log("Edit stage " + nameInputField.GetComponent<TMP_InputField>().text);
        }
        if (!validName)
        {
            nameText.text = "Name already exists!";
            nameText.color = Color.red;
            return;
        }
        else
        {
            nameText.text = "Stage name:";
            nameText.color = new Color(50, 50, 50);


            // Save the list to data
            List<GameData.StageObstacle> newList = new List<GameData.StageObstacle>();
            foreach (GameObject elem in allElements)
            {
                newList.Add(new GameData.StageObstacle(elem.name, elem.transform.position));
            }
            if(GameDataController.controller.chosenStage != null)
            {
                GameDataController.controller.chosenStage.allElements = newList;
                // change Leaderboard name
                GameDataController.controller.changeLeaderboardName(GameDataController.controller.chosenStage.stageName, nameInputField.GetComponent<TMP_InputField>().text);
                GameDataController.controller.chosenStage.stageName = nameInputField.GetComponent<TMP_InputField>().text;
            }
            else
            {
                GameData.CustomStage newStage = new GameData.CustomStage(nameInputField.GetComponent<TMP_InputField>().text);
                newStage.allElements = newList;
                GameDataController.controller.data.customStages.Add(newStage);
                GameDataController.controller.chosenStage = newStage;
                //Create leaderboard
                GameDataController.controller.data.leaderboardList.Add(new GameData.StageLeaderboard(newStage.stageName));
            }
            screenshotController.captureSceenshot();
            int coinNum;
            if (int.TryParse(coinInputField.GetComponent<TMP_InputField>().text, out coinNum))
            {
                GameDataController.controller.chosenStage.numOfCoins = coinNum;
            }
            else GameDataController.controller.chosenStage.numOfCoins = 0;

            changeMade = false;
            return;
        }
    }

    public void saveAsStage()
    {

        //Debug.Log(nameInputField.GetComponent<TMP_InputField>().text);
        bool validName = true;
        bool saveAsExisting = false;
        GameData.CustomStage existing = null;
        if (holeNum < 1)
        {
            nameText.text = "Stage must have a hole!";
            nameText.color = Color.red;
            return;
        }
        if (nameInputField.GetComponent<TMP_InputField>().text == "")
        {
            nameText.text = "Stage name is required!";
            nameText.color = Color.red;
            return;
        }
        foreach (string elem in GameDataController.controller.gameStages)
        {
            if (elem.Equals(nameInputField.GetComponent<TMP_InputField>().text))
            {
                validName = false;
                //Debug.Log("Base stage " + nameInputField.GetComponent<TMP_InputField>().text);
                break;
            }
        }
        foreach (GameData.CustomStage elem in GameDataController.controller.data.customStages)
        {
            if (elem.stageName.Equals(nameInputField.GetComponent<TMP_InputField>().text))
            {
                saveAsExisting = true;
                existing = elem;
                if (GameDataController.controller.chosenStage == null)
                {
                    validName = false;
                    //Debug.Log("Custom stage " + nameInputField.GetComponent<TMP_InputField>().text);
                    break;
                }
                else
                {
                    if (elem.stageName != GameDataController.controller.chosenStage.stageName)
                    {
                        validName = false;
                        //Debug.Log("Custom stage " + nameInputField.GetComponent<TMP_InputField>().text);
                        break;
                    }
                    else
                    {
                        saveStage();
                        return;
                    }
                }
            }
        }
        if (nameInputField.GetComponent<TMP_InputField>().text == GameDataController.controller.customStageName)
        {
            validName = false;
            //Debug.Log("Edit stage " + nameInputField.GetComponent<TMP_InputField>().text);
        }
        if (!validName && !saveAsExisting)
        {
            nameText.text = "Name unavailable!";
            nameText.color = Color.red;
            return;
        }
        else if (!validName && saveAsExisting)
        {
            overwriteWindow.SetActive(true);
            overwrittenStage = existing;
            return;
        }
        else
        {
            nameText.text = "Stage name:";
            nameText.color = new Color(50, 50, 50);


            // Save the list to data
            List<GameData.StageObstacle> newList = new List<GameData.StageObstacle>();
            foreach (GameObject elem in allElements)
            {
                newList.Add(new GameData.StageObstacle(elem.name, elem.transform.position));
            }
            //if (GameDataController.controller.chosenStage != null)
            //{
            //    GameDataController.controller.chosenStage.allElements = newList;
            //    GameDataController.controller.chosenStage.stageName = nameInputField.GetComponent<TMP_InputField>().text;
            //}
            //else
            //{
                GameData.CustomStage newStage = new GameData.CustomStage(nameInputField.GetComponent<TMP_InputField>().text);
                newStage.allElements = newList;
                GameDataController.controller.data.customStages.Add(newStage);
                GameDataController.controller.chosenStage = newStage;
            //Create leaderboard
            GameDataController.controller.data.leaderboardList.Add(new GameData.StageLeaderboard(newStage.stageName));
            //}
            screenshotController.captureSceenshot();
            int coinNum;
            if (int.TryParse(coinInputField.GetComponent<TMP_InputField>().text, out coinNum))
            {
                GameDataController.controller.chosenStage.numOfCoins = coinNum;
            }
            else GameDataController.controller.chosenStage.numOfCoins = 0;
            changeMade = false;

            return;
        }
    }


    public void overwriteYesClk()
    {
        nameText.text = "Stage name:";
        nameText.color = new Color(50, 50, 50);


        // Save the list to data
        List<GameData.StageObstacle> newList = new List<GameData.StageObstacle>();
        foreach (GameObject elem in allElements)
        {
            newList.Add(new GameData.StageObstacle(elem.name, elem.transform.position));
        }
        GameDataController.controller.chosenStage = overwrittenStage;
        overwrittenStage = null;


        GameDataController.controller.chosenStage.allElements = newList;
        // change Leaderboard name
        GameDataController.controller.changeLeaderboardName(GameDataController.controller.chosenStage.stageName, nameInputField.GetComponent<TMP_InputField>().text);
        GameDataController.controller.chosenStage.stageName = nameInputField.GetComponent<TMP_InputField>().text;

        screenshotController.captureSceenshot();
        int coinNum;
        if (int.TryParse(coinInputField.GetComponent<TMP_InputField>().text, out coinNum))
        {
            GameDataController.controller.chosenStage.numOfCoins = coinNum;
        }
        else GameDataController.controller.chosenStage.numOfCoins = 0;

        overwriteWindow.SetActive(false);

        changeMade = false;

        return;
    }

    public void overwriteNoClk()
    {
        overwrittenStage = null;
        overwriteWindow.SetActive(false);
    }



    public void backBtnClk()
    {
        if (changeMade)
            exitMenu.SetActive(true);
        else
            exitYesClk();
    }

    public void exitYesClk()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void exitNoClk()
    {
        exitMenu.SetActive(false);
    }

    public int setTreeLayerOrder(Transform t)
    {
        return Mathf.RoundToInt(t.position.y * 100f) * -1;
    }

    public void selectStampSound()
    {
        AudioManager.Instance.Play("TreeHit");
    }

    public void successfulPlace()
    {
        AudioManager.Instance.Play("BushHit");
        changeMade = true;
    }

    public void failPlace()
    {
        AudioManager.Instance.Play("PlacementFail");
    }

    public void cancelPlace()
    {
        AudioManager.Instance.Play("BtnClk");
    }

    public void inputChanged()
    {
        changeMade = true;
    }
}
