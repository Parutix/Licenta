using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SRWParams_",menuName = "PCG/SRWData")]
public class SRWData : ScriptableObject
{
    public int noOfIterations = 10, walkLength = 10;
    public bool startRandom = true;
}
