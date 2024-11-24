using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    public GameObject go;
    ChipCount cnt;
    // Start is called before the first frame update
    void Start()
    {
       go.SetActive(false);
    }

    public void activate()
    {

        go.SetActive(true);
        if (cnt.ChipNum == 1)
        {
            Debug.Log("ACTIVATED");
        }
    } 
    private void FixedUpdate()
    {
        if (cnt.ChipNum == 1) {
            activate();
            Debug.Log("ACTIVATED"); 
        }
        
    }


}
