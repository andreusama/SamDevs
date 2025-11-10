using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class ActiveButton : MonoBehaviour
{
    [SerializeField]
    private Active active;

    [SerializeField]
    private Image spriteImage;

    public GameObject cover;

    public Image Wheel; //the thing you're trying to rotate

    Vector2 dir;
    float dist;
    float check;
    bool isRotating;
    float angle;
    bool checkPoint;

    float windowTime = 0f;

    float velocity;

    float minVel = 60f;
    float maxVel = 500f;
    public void SetActive(Active newActive)
    {
        if (newActive != null)
        {
            active = newActive;
            spriteImage.sprite = newActive.GetSprite();
            cover.SetActive(false);
        }
        else
        {
            cover.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (LevelTraveler.MyTravelInstance != null)
        SetActive(LevelTraveler.MyTravelInstance.Gear.active);
    }

    void Update()
    {
        if (active == null)
        {
            return;
        }
        //is rotating is set true if mouse is down on the handle
        if (isRotating && active.GetUseType() == Active.UseType.CIRCLES)
        {
            //Vector from center to mouse pos
            dir = (Input.mousePosition - Wheel.transform.position);
            //Distance between mouse and the center
            dist = Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y);

            //get the radial velocity of the rotation
            //if mouse is not outside nor too inside the wheel
            //Debug.Log("DISTANCE:" + dist);
            if (dist < 300 && dist > 30)
            {
                angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg; //alien technology
                angle = (angle > 0) ? angle : angle + 360; //0 to 360 instead of -180 to 180

                //this if blocks going back or jumping too far
                //if ((angle < check && check - angle < 90) || angle > 350)
                {
                    float lastVelocity = velocity;
                    velocity = Mathf.Abs(angle - check) / Time.deltaTime;
                    //velocity = Mathf.Max(lastVelocity, velocity);

                    if (windowTime >= 0.1f)
                    {
                        if (velocity > 0f)
                        {
                            //get the first velocity while rolling and then only get the velocity if it's higher than the first one, use Mathf Max()
                            buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().passiveObject.ActualState = PassiveObject.States.LIGHTING;
                            buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().passiveObject.SetIntensity(active, velocity, minVel, maxVel);
                        }
                        //active.SummonActive();
                        windowTime = 0f;
                    }
                    else
                    {
                        windowTime += Time.deltaTime;
                    }
                    check = angle;
                    Wheel.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
                    //Vector3.back for counter clockwise, Vector3.forward for clockwise..I think
                }
                //else
                //{
                //    Debug.Log("Stopped!");
                //    buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().passiveObject.SetIntensity(CalcIntensity(0));
                //}
            }
        }
        //to confirm if it has passed full circle
        if (angle > 160 && angle < 200)
        {
            checkPoint = true;
        }

        if (angle > 350 && checkPoint)
        {
            if (LevelTraveler.MyTravelInstance.Level.isTutorial == true)
            {
                TutorialManager.Instance.NextPhase(TutorialManager.GAMEPLAY_TUTORIAL_PHASE.ACTIVE);
            }
            checkPoint = false;
            //Debug.Log("SCORE++");
        }
    }

    public void BeginDrag()
    {
        Debug.Log("Begin drag!");
        isRotating = true;
    }
    public void EndDrag()
    {
        Debug.Log("End drag!");
        buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().passiveObject.InvencibilityWindow = true;
        isRotating = false;
    }
    public void PointerUp()
    {
        Debug.Log("End drag!");
        buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().passiveObject.InvencibilityWindow = true;
        isRotating = false;
    }
    public void Out()
    {
        Debug.Log("End drag!");
        buildController.MyBuildInstance.actualTower.GetComponent<TowerHolder>().passiveObject.InvencibilityWindow = true;
        isRotating = false;
    }
    //public float CalcIntensity(float velocity)
    //{
    //    float intensity = Mathf.Clamp(velocity, minVel, maxVel);

    //    intensity = StatController.Map(intensity, minVel, maxVel, 0f, 1f);

    //    //Debug.Log("Intensity:" + intensity);
        
    //    return intensity;
    //}
    //public void CircularTick()
    //{
        
    //}

    //private float CalcHeat(float intensity)
    //{

    //    //we will get rid of the ice if we circle quick!
    //    return (((PlayerStats.MyInstance.Debuff.VidaM / 10) * StatController.Map(intensity, 0f, 1f, 0f, 1.5f))) / 10;
    //}
}
