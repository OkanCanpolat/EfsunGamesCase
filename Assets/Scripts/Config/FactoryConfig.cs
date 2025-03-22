using System;
using UnityEngine;

[CreateAssetMenu (fileName = "FactoryConfig" , menuName = "Configs/ Factory Config")]
public class FactoryConfig : ScriptableObject
{
    [Header("General")]
    public FactoryRequirementContainer Requirement;
    public ResourceType Output;
    public int OutputAmount;
    public int ProductionTime;
    public int Capacity;
    public Sprite OutputSprite;

    [Header("UI Position Settings")]
    public float ReferenceCameraDistance;
    public float YOffset;
}

[Serializable]
public class FactoryRequirementContainer
{
    public ResourceType Type;
    public int Amount;
    public Sprite Sprite;
}
