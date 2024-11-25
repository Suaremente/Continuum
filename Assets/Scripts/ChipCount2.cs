using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChipCount2 : MonoBehaviour
{

    public static ChipCount2 instance;
    public TMP_Text chipText;
    public int currentCoins = 0;
    public GameObject obj; 

    private void Awake()
    {
        instance = this; 
    }
    // Start is called before the first frame update
    void Start()
    {
        chipText.text = currentCoins.ToString() + "/20"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseCoins(int v) {

        if (currentCoins == 1)
        {
            obj = GameObject.FindWithTag("Truck");
            obj.SetActive(false);
        }
        currentCoins++; 
        chipText.text = currentCoins.ToString() + "/20";
        if (currentCoins == 20) {
            
            obj.SetActive(true);
        
        }
    }
}
