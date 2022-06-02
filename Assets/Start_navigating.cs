using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_navigating : MonoBehaviour
{
    public GameObject holder;
    // Start is called before the first frame update
    public void SetActive()
    {
        if(holder.active==false)
        {
            holder.SetActive(true);

        }
        else
        {
            holder.SetActive(false);
        }
    }

    
}
