using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UIElements;

public class PassiveObject : MonoBehaviour
{
    public Active active;
    public ParticleSystem fire;
    public ParticleSystem smoke;

    [Header("Fire Intensity")]
    [Range(0.0f, 1.0f)]
    public float mySliderFloat;

    //this is aimed in order to copy the exact value of mySliderFloat when starting the invencibility window and fade. 
    private float intensityBuffer = 0f;
    
    private ParticleSystem fireInstance;
    private ParticleSystem smokeInstance;

    float xMin = 0f;
    float xMax = 1f;

    private float actualTime = 0f;
    private float fadeTime = 0f;
    private float aimTime = 2f;

    private bool fadingOut = false;

    public enum States
    {
        IDLE,
        LIGHTING,
        INVENCIBLE,
        FADING,
    }

    private States actualState;

    //create a getter and setter of actualState
    public States ActualState
    {
        get { return actualState; }
        set
        {
            actualState = value;
        }
    }
    //make a getter and setter of fadingOut that when the value is false modifies actual time
    public bool FadingOut
    {
        get { return fadingOut; }
        set
        {
            fadingOut = value;
            if (fadingOut == true)
            {
                fadeTime = TimeController.MyTimeInstance.TimeFromSteps(TimeController.MyTimeInstance.GetStepsToNextDanger());
                aimTime = fadeTime;
                Debug.Log("Actual time: " + actualTime);
                active.fadeTimeStorage = fadeTime;
            }
        }
    }

    private bool invencibilityWindow = false;

    public bool InvencibilityWindow
    {
        get { return invencibilityWindow; }
        set
        {
            invencibilityWindow = value;
            if (invencibilityWindow == true)
            {
                actualTime = TimeController.MyTimeInstance.TimeFromSteps(3);
                aimTime = actualTime;
                intensityBuffer = mySliderFloat;
                active.invTimeStorage = actualTime;

                actualState = States.INVENCIBLE;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (LevelTraveler.MyTravelInstance != null)
        {
            active = LevelTraveler.MyTravelInstance.Gear.active;
            Debug.Log("Intensity in the active is " + active.intensityStorage);

        }
        fireInstance = fire.GetComponent<ParticleSystem>();
        smokeInstance = smoke.GetComponent<ParticleSystem>();

        actualState = States.IDLE;

        
        actualTime = active.invTimeStorage;
        if (fadeTime >= 0)
        {
            actualState = States.INVENCIBLE;
        }
        fadeTime = active.fadeTimeStorage;
        if (fadeTime >= 0)
        {
            actualState = States.FADING;
        }
        
        aimTime = TimeController.MyTimeInstance.TimeFromSteps(3);
        intensityBuffer = active.intensityStorage;

        SetIntensity(active, active.intensityStorage);
    }

    // Update is called once per frame
    void Update()
    {
        switch (actualState)
        {
            case States.IDLE:
                
                break;
            case States.LIGHTING:
                
                break;
            case States.INVENCIBLE:
                
                if (actualTime >= 0)
                {
                    actualTime -= Time.deltaTime;
                    active.invTimeStorage = actualTime;
                    //lerp intensity from 100 to 0
                    float intensity = intensityBuffer;
                    SetIntensity(active, intensity);
                }
                else
                {
                    FadingOut = true;
                    InvencibilityWindow = false;
                    actualState = States.FADING;
                }
                
                break;
            case States.FADING:
                
                if (fadeTime >= 0)
                {
                    fadeTime -= Time.deltaTime;
                    active.fadeTimeStorage = fadeTime;
                    //lerp intensity from 100 to 0
                    float intensity = Mathf.Lerp(0, intensityBuffer, fadeTime / aimTime);
                    SetIntensity(active, intensity);

                    Debug.Log("ACTUAL INTENSITY IN PASSIVE WAS: " + intensity);
                }
                else
                {
                    FadingOut = false;
                    actualState = States.IDLE;
                }
                
                break;
            default:
                break;
        }
        
        
        

    }

    //for low fire 4 is a good value
    //for hight fire 25 is a good value
    //create a method that controls the size and lifetime of the particle system
    public void SetParticleSize(float intensity)
    {
        //map the intensity from 4 to 25 and set the particle system size max value
        float newValue = Map(intensity, 0f, 1f, 0f, 30f);
        var main = fireInstance.main;
        main.startSize = new ParticleSystem.MinMaxCurve(newValue - 10f, newValue); ;
        var main2 = smokeInstance.main;
        main2.startSize = new ParticleSystem.MinMaxCurve(newValue - 10f, newValue); ;
    }

    //create a method that given an intensity float maps it to a value inside the range of the SliderFloat
    public void SetIntensity(Active _active, float velocity = 0f, float minVel = 0f, float maxVel = 0f)
    {
        if (_active == null)
        {
            mySliderFloat = 0;
            LevelTraveler.MyTravelInstance.Gear.active.intensityStorage = 0f;
            SetParticleSize(mySliderFloat);
            return;
        }

        _active.intensityStorage = _active.CalcIntensity(velocity, minVel, maxVel);
        _active.SummonActive(_active.CalcIntensity(velocity, minVel, maxVel));

        mySliderFloat = Mathf.Lerp(0.0f, 1.0f, _active.CalcIntensity(velocity, minVel, maxVel));
        SetParticleSize(mySliderFloat);
    }

    public void SetIntensity(Active _active, float intensity = 0f)
    {
        if (_active == null)
        {
            mySliderFloat = 0;
            LevelTraveler.MyTravelInstance.Gear.active.intensityStorage = 0f;
            SetParticleSize(mySliderFloat);
            return;
        }

        _active.intensityStorage = intensity;
        Debug.Log("intensity buffer setted to" + intensity);
        _active.SummonActive(intensity);

        mySliderFloat = Mathf.Lerp(0.0f, 1.0f, intensity);
        SetParticleSize(mySliderFloat);
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        //vida escalable con vida actual
    }
    
}
