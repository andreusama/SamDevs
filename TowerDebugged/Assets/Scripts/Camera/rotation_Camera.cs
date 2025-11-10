using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class rotation_Camera : MonoBehaviour {

    private new Camera mainCamera;

    public float turnspeed = 5;
    public float cameraMovementSpeed = 0.1f;
    private int distance = 100;
    private GameObject gc;

    [SerializeField]
    private float offset = 4.33f;

    public Transform rotation;
    public Transform position;

    private bool elevating;
    private bool delevating;
    public int cameraLevel;

    public int GetCameraLevel()
    {
        return cameraLevel;
    }

    public int maxLevel;

    private bool waiting;

    public float offsetY = 0.0f;

    public bool GetWaiting()
    {
        return waiting;
    }
    // Use this for initialization

    private static rotation_Camera cameraInstance;

    public static rotation_Camera MyCameraInstance
    {
        get
        {
            if (cameraInstance == null)
            {
                cameraInstance = FindObjectOfType<rotation_Camera>();
            }
            return cameraInstance;
        }


    }

    private void Awake()
    {
        rotation = this.gameObject.GetComponent<Transform>();
        mainCamera = this.gameObject.GetComponentInChildren<Camera>();
        gc = GameObject.FindWithTag("GameController");

        Debug.Log(mainCamera.name);
    }

    void Start ()
    {
        waiting = false;
        if (InLevelSaveLoad.MySaveController.newGame == true)
        {
            cameraLevel = 1;
            maxLevel = 1;
        }
    }

    
    IEnumerator NextUp()
    {
        //Debug.Log("UP");
        waiting = true;
        FeedbackController.MyFeedbackInstance.MovementFx();

        Vector3 actualposition = position.transform.localPosition;
        Vector3 finalPosition = buildController.MyBuildInstance.towerPositions[cameraLevel - 1];
        finalPosition.y += offsetY;

        Vector3 temporalTrans = position.transform.localPosition;

        int iteration = 0;
        while (temporalTrans.y <= finalPosition.y)
        {
            CheckBioma(temporalTrans);
            //Debug.Log("Map results: " + Map(temporalTrans.y, 0, finalPosition.y, 0, 1) + "iteration number : " + iteration);
            position.transform.localPosition = Vector3.Lerp(actualposition, finalPosition, Map(temporalTrans.y, actualposition.y, finalPosition.y, 0, 1));
            yield return new WaitForSeconds(0.001f);
            temporalTrans.y += 0.12f * cameraMovementSpeed;
            iteration++;
        }
        waiting = false;

        if (cameraLevel < maxLevel)
            cameraLevel++;

        //Debug.Log("Camera Level: " + cameraLevel);
        //Debug.Log("Tower Holder Count: " + buildController.MyBuildInstance.towerPositions.Count);
       
        buildController.MyBuildInstance.CheckIfFloorAndChangeBuildingPoint();
        UIController.MyUiInstance.UpdateMovementUI();
        yield break;
    }

    IEnumerator NextFlatUp()
    {
        waiting = true;
        FeedbackController.MyFeedbackInstance.MovementFx();

        Vector3 actualposition = position.transform.localPosition;

        //if IS FLOOR BP + 1
        Vector3 finalPosition = Vector3.zero;
        if (buildController.MyBuildInstance.IsFloor(cameraLevel) == true)
        {
            if(buildController.MyBuildInstance.floorsList[buildController.MyBuildInstance.GetBuildingPoint()] != null)
                finalPosition = buildController.MyBuildInstance.floorsList[buildController.MyBuildInstance.GetBuildingPoint()].buildingPoint;

            cameraLevel = buildController.MyBuildInstance.floorsList[buildController.MyBuildInstance.GetBuildingPoint()].level;
            buildController.MyBuildInstance.SetBuildingPoint(buildController.MyBuildInstance.GetBuildingPoint());

            Debug.Log("CameraLevel: " + cameraLevel);
            Debug.Log("Build Point: " + (buildController.MyBuildInstance.GetBuildingPoint()));
        }
        else
        {
            //if NOT FLOOR BP
            Debug.Log("Going To: " + buildController.MyBuildInstance.GetBuildingPoint());
            finalPosition = buildController.MyBuildInstance.floorsList[buildController.MyBuildInstance.GetBuildingPoint() - 1].buildingPoint;

            cameraLevel = buildController.MyBuildInstance.floorsList[buildController.MyBuildInstance.GetBuildingPoint() - 1].level;
            buildController.MyBuildInstance.SetBuildingPoint(buildController.MyBuildInstance.GetBuildingPoint() - 1);
        }

        Vector3 temporalTrans = position.transform.localPosition;

        int iteration = 0;
        while (temporalTrans.y <= finalPosition.y)
        {
            CheckBioma(temporalTrans);
            //Debug.Log("Map results: " + Map(temporalTrans.y, 0, finalPosition.y, 0, 1) + "iteration number : " + iteration);
            position.transform.localPosition = Vector3.Lerp(actualposition, finalPosition, Map(temporalTrans.y, actualposition.y, finalPosition.y, 0, 1));
            yield return new WaitForSeconds(0.0025f);
            temporalTrans.y += 0.12f;
            iteration++;
        }
        waiting = false;


        buildController.MyBuildInstance.CheckIfFloorAndChangeBuildingPoint();
        UIController.MyUiInstance.UpdateMovementUI();
        yield break;
    }

    IEnumerator NextFlatDown()
    {
        waiting = true;
        FeedbackController.MyFeedbackInstance.MovementFx();
        Vector3 actualposition = position.transform.localPosition;
        Vector3 finalPosition = Vector3.zero;
        if (buildController.MyBuildInstance.IsFloor(cameraLevel) == true)
        {
            Debug.Log("Going To: " + (buildController.MyBuildInstance.GetBuildingPoint() - 1));
            if (buildController.MyBuildInstance.floorsList[buildController.MyBuildInstance.GetBuildingPoint() - 1] != null)
                finalPosition = buildController.MyBuildInstance.floorsList[buildController.MyBuildInstance.GetBuildingPoint() - 1].buildingPoint;

            cameraLevel = buildController.MyBuildInstance.floorsList[buildController.MyBuildInstance.GetBuildingPoint() - 1].level;
            buildController.MyBuildInstance.SetBuildingPoint(buildController.MyBuildInstance.GetBuildingPoint() - 1);

            Debug.Log("CameraLevel: " + cameraLevel);
            Debug.Log("Build Point: " + (buildController.MyBuildInstance.GetBuildingPoint()));
        }
        else
        {
            Debug.Log("Going To: " + (buildController.MyBuildInstance.GetBuildingPoint() - 1));
            if (buildController.MyBuildInstance.floorsList[buildController.MyBuildInstance.GetBuildingPoint() - 1] != null)
                finalPosition = buildController.MyBuildInstance.floorsList[buildController.MyBuildInstance.GetBuildingPoint() - 1].buildingPoint;

            cameraLevel = buildController.MyBuildInstance.floorsList[buildController.MyBuildInstance.GetBuildingPoint() - 1].level;
            buildController.MyBuildInstance.SetBuildingPoint(buildController.MyBuildInstance.GetBuildingPoint() - 1);

            Debug.Log("CameraLevel: " + cameraLevel);
            Debug.Log("Build Point: " + (buildController.MyBuildInstance.GetBuildingPoint()));
        }

        Vector3 temporalTrans = position.transform.localPosition;

        int iteration = 0;
        while (temporalTrans.y >= finalPosition.y)
        {
            CheckBioma(temporalTrans);
            position.transform.localPosition = Vector3.Lerp(actualposition, finalPosition, Map(Mathf.Abs(temporalTrans.y), actualposition.y, finalPosition.y, 0, 1));
            yield return new WaitForSeconds(0.0025f);
            temporalTrans.y -= 0.12f;
            iteration++;
        }

        waiting = false;

        buildController.MyBuildInstance.CheckIfFloorAndChangeBuildingPoint();
        UIController.MyUiInstance.UpdateMovementUI();
        yield break;
    }

    IEnumerator TopFlat(int speedScale)
    {
        waiting = true;
        FeedbackController.MyFeedbackInstance.MovementFx();

        Vector3 actualposition = position.transform.localPosition;
        Vector3 finalPosition = buildController.MyBuildInstance.towerPositions[(buildController.MyBuildInstance.towerPositions.Count - 1)];
        finalPosition.y -= offsetY;

        Vector3 temporalTrans = position.transform.localPosition;

        int iteration = 0;

        while (temporalTrans.y <= finalPosition.y)
        {
            CheckBioma(temporalTrans);
            //Debug.Log("Map results: " + Map(temporalTrans.y, 0, finalPosition.y, 0, 1) + "iteration number : " + iteration);
            position.transform.localPosition = Vector3.Lerp(actualposition, finalPosition, Map(temporalTrans.y, actualposition.y, finalPosition.y, 0, 1));
            yield return new WaitForSeconds(0.001f);
            temporalTrans.y += 0.12f * cameraMovementSpeed * speedScale;
            iteration++;
        }
        waiting = false;
        cameraLevel = maxLevel;
        buildController.MyBuildInstance.CheckIfFloorAndChangeBuildingPoint();
        UIController.MyUiInstance.UpdateMovementUI();
        yield break;
    }

    IEnumerator BottomFlat(int speedScale)
    {
        waiting = true;
        FeedbackController.MyFeedbackInstance.MovementFx();
        Debug.Log("decreasing");
        Vector3 actualposition = position.transform.localPosition;
        Vector3 finalPosition = new Vector3(0.09f, 0f, 0f);

        Vector3 temporalTrans = position.transform.localPosition;

        int iteration = 0;
        while (temporalTrans.y >= finalPosition.y)
        {
            CheckBioma(temporalTrans);
            position.transform.localPosition = Vector3.Lerp(actualposition, finalPosition, Map(Mathf.Abs(temporalTrans.y), actualposition.y, finalPosition.y, 0, 1));
            yield return new WaitForSeconds(0.001f);
            temporalTrans.y -= 0.12f * cameraMovementSpeed * speedScale;
            iteration++;
        }

        waiting = false;
        cameraLevel = 1;
        buildController.MyBuildInstance.CheckIfFloorAndChangeBuildingPoint();
        UIController.MyUiInstance.UpdateMovementUI();
        yield break;
    }

    IEnumerator NextDown()
    {
        Debug.Log("DOWN");
        waiting = true;
        FeedbackController.MyFeedbackInstance.MovementFx();

        Debug.Log("Actual Position: " + position.transform.localPosition);
        Debug.Log("Aim Position: " + buildController.MyBuildInstance.towerPositions[cameraLevel - 2]);

        Vector3 actualposition = position.transform.localPosition;
        Vector3 finalPosition = buildController.MyBuildInstance.towerPositions[cameraLevel - 2];
        finalPosition.y -= offsetY;

        Vector3 temporalTrans = position.transform.localPosition;

        int iteration = 0;
        while (temporalTrans.y >= finalPosition.y)
        {
            CheckBioma(temporalTrans);
            position.transform.localPosition = Vector3.Lerp(actualposition, finalPosition, Map(Mathf.Abs(temporalTrans.y), actualposition.y, finalPosition.y, 0, 1));
            yield return new WaitForSeconds(0.0025f);
            temporalTrans.y -= 0.12f * cameraMovementSpeed;
            iteration++;
        }

        waiting = false;

        if (cameraLevel > 1)
            cameraLevel--;

        buildController.MyBuildInstance.CheckIfFloorAndChangeBuildingPoint();
        UIController.MyUiInstance.UpdateMovementUI();
        yield break;
    }

    public void Top()
    {
        float speedScale = 0;
        speedScale = cameraLevel - maxLevel;

        if (waiting == false && cameraLevel < maxLevel)
            StartCoroutine(TopFlat((int)Mathf.Sqrt(Mathf.Abs(speedScale))));
        //position.transform.localPosition += new Vector3(0, 2.25f, 0);
    }

    public void Bottom()
    {
        float speedScale = 0;
        speedScale = cameraLevel;

        if (waiting == false)
            StartCoroutine(BottomFlat((int)Mathf.Sqrt(Mathf.Abs(speedScale))));
        //position.transform.localPosition += new Vector3(0, 2.25f, 0);
    }
    public void Up()
    {
        Debug.Log("Max Level:" + maxLevel);
        Debug.Log("Actual Camera Level:" + cameraLevel);
        if(waiting == false && cameraLevel < maxLevel)
            StartCoroutine(NextUp());
        //position.transform.localPosition += new Vector3(0, 2.25f, 0);
    }

    public void UpFlat()
    {

        if (waiting == false)
            StartCoroutine(NextFlatUp());
        //position.transform.localPosition += new Vector3(0, 2.25f, 0);
    }

    public void Down()
    {
        Debug.Log("Actual Camera Level:" + cameraLevel);
        if (waiting == false && cameraLevel > 1)
            StartCoroutine(NextDown());
        //position.transform.localPosition -= new Vector3(0, 2.25f, 0);
    }

    public void DownFlat()
    {
        if (waiting == false)
            StartCoroutine(NextFlatDown());
        //position.transform.localPosition += new Vector3(0, 2.25f, 0);
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //vida escalable con vida actual
    }

    private void CheckBioma(Vector3 cameraWorldPosition)
    {
        ////Debug.Log("Checking Bioma");
        //foreach (Zone zone in buildController.MyBuildInstance.zonesList)
        //{
        //    if (zone.StartingPoint.y < cameraWorldPosition.y && zone.FinishingPoint.y > cameraWorldPosition.y)
        //    {
        //        zone.StartBioma(true);
        //    }
        //    else
        //    {
        //        zone.EndBioma(false);
        //    }
        //}
    }

    //NEW CODE EASY

    public void GoToLevel(int level)
    {
        //Debug.Log("Going to level antes de comprobaciones");
        if (level > maxLevel || level < 1)
        {
            Debug.Log("Level out of bounds...");
            return;
        }

        //Debug.Log("Going to level despues de comprobaciones");

        float speedScale = 0;

        speedScale = cameraLevel - level;

        Debug.Log("Speed scale : " + (int)Mathf.Sqrt(Mathf.Abs(speedScale)));
        
        if (waiting == false)
            StartCoroutine(TravelTo(level, false, (int)Mathf.Sqrt(Mathf.Abs(speedScale))));

    }
    public void Plus()
    {
        Debug.Log("Actual Camera Level:" + cameraLevel);

        if (waiting == false && cameraLevel < maxLevel)
            StartCoroutine(TravelTo(1, true, 1));
    }

    public void Minus()
    {
        Debug.Log("Actual Camera Level:" + cameraLevel);

        if (waiting == false && cameraLevel > 1)
            StartCoroutine(TravelTo(-1, true, 1));
    }

    public void Maximum()
    {
        float speedScale = 0;
        speedScale = cameraLevel - maxLevel;

        Debug.Log("Actual Camera Level:" + cameraLevel);
        if (TutorialManager.Instance.isTutorial == true)
        {
            TutorialManager.Instance.NextPhase(TutorialManager.GAMEPLAY_TUTORIAL_PHASE.MAP);
        }
        if (waiting == false && cameraLevel < maxLevel)
            StartCoroutine(TravelTo(maxLevel, false, (int)Mathf.Sqrt(Mathf.Abs(speedScale))));
    }

    public void Minimum()
    {
        float speedScale = 0;
        speedScale = cameraLevel;

        Debug.Log("Actual Camera Level:" + cameraLevel);
        if (TutorialManager.Instance.isTutorial == true)
        {
            TutorialManager.Instance.NextPhase(TutorialManager.GAMEPLAY_TUTORIAL_PHASE.MAP);
        }
        if (waiting == false && cameraLevel > 1)
            StartCoroutine(TravelTo(1, false, (int)Mathf.Sqrt(Mathf.Abs(speedScale))));
    }

    public void CityUp()
    {
        Debug.Log("Actual Camera Level:" + cameraLevel);

        if (waiting == false)
            StartCoroutine(TravelTo(1, false, 1));
    }

    public IEnumerator TravelTo(int aim, bool type, int speedScale)
    {
        Debug.Log("Starting Traveling to level!" + aim);
        float time = 0.0f;
        float smoothTime = 0.2f;
        //type false go directly to aim
        //type true additive or substractive aim

        if (speedScale == 0)
        {
            speedScale = 1;
        }

        int keyBuffer = 0;

        if (type == false)
        {
            keyBuffer = aim;
        }
        else 
        {
            keyBuffer = cameraLevel + aim;
            Debug.Log("Buffer Travel: " + keyBuffer);
        }

        //DOWN
        waiting = true;
        FeedbackController.MyFeedbackInstance.MovementFx();

        Vector3 actualposition = position.transform.localPosition;
        Vector3 finalPosition = buildController.MyBuildInstance.floorsList[keyBuffer].checkPoint;

        Vector3 temporalTrans = position.transform.localPosition;

        finalPosition.y -= offsetY;
        //DOWN
        while (time < smoothTime)
        {
            time += Time.smoothDeltaTime * speedScale;
            position.transform.localPosition = Vector3.Lerp(actualposition, finalPosition, time / smoothTime);
            yield return null;
        }

        waiting = false;

        if (type == false)
        {
            cameraLevel = aim;
        }
        else if (type == true)
        {
            cameraLevel += aim;
        }

        buildController.MyBuildInstance.CheckIfFloorAndChangeBuildingPoint();
        UIController.MyUiInstance.UpdateMovementUI();
        yield break;
    }
}
