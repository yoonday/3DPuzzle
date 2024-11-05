using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Data
{
    public bool[] isComplete = new bool[4];
    public Vector3[] respawnPoint = new Vector3[4];
}
