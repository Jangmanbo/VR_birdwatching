using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Bird")]
public class BirdInfo : ScriptableObject
{
    public int ID;
    public float flySpeed, moveSpeed, turnSpeed, landingInterval;
    [Range(0f, 1f)] public float flyProbability;
}
