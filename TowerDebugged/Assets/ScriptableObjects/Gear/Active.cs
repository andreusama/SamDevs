using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Active", menuName = "Gear/Active")]
public class Active : ScriptableObject
{
    public float intensityStorage = 0f;
    public float invTimeStorage = 0f;
    public float fadeTimeStorage = 0f;
    public enum UseType
    {
        CIRCLES,
        SUSTAIN
    }

    public enum ActiveType
    {
        DAMAGE,
        UTILITY
    }

    [SerializeField]
    protected UseType useType;

    [SerializeField]
    protected ActiveType activeType;

    [SerializeField]
    protected Sprite sprite;

    public Sprite GetSprite()
    {
        return sprite;
    }

    public UseType GetUseType()
    {
        return useType;
    }

    public ActiveType GetActiveType()
    {
        return activeType;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SummonActive(float intensity){ }

    public virtual float CalcIntensity(float velocity, float minVel, float maxVel){ return 0f; }
}
