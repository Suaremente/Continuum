using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChipCount : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField]
    int ChipNum;

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.CompareTag("Chip"))
        {
            ChipNum++;
            Debug.Log("Chip collided");
        }
    }
}