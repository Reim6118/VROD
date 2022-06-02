using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_button : MonoBehaviour
{
    Vector3 position;
   public void Map_Location()
    {
        position = transform.position;
        Debug.LogWarning("Button position:" + position);
    }
}
