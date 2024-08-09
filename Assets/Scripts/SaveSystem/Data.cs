using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Data 
{
    public int name;
    public float[] position;

    public Data(Vector3 position, int name)
    {
        this.position = new float[3];
        this.position[0] = position.x;
        this.position[1] = position.y;
        this.position[2] = position.z;

        this.name = name;
    }
}
