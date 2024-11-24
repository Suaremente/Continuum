using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ChipCount : MonoBehaviour
{
    // Start is called before the first frame update
     
    [SerializeField]
    public int ChipNum;

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
         if (collision.CompareTag("Chip"))
        {
            ChipNum++;
            Debug.Log("Chip collided");
            ChipCount2.instance.increaseCoins(ChipNum);

            if (ChipNum == 20) {

              
            }

            if (ChipNum == 40) {

               //SceneManager.LoadScene("boss2");
            }

            if (ChipNum == 60) {

                //SceneManager.LoadScene("boss3");
            }
        } 
    }
}