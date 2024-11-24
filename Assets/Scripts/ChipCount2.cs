using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChipCount2 : MonoBehaviour
{

    public static ChipCount2 instance;
    public TMP_Text chipText;
    public int currentCoins = 0;

    private void Awake()
    {
        instance = this; 
    }
    // Start is called before the first frame update
    void Start()
    {
        chipText.text = currentCoins.ToString() + "/60"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseCoins(int v) {

        currentCoins++; 
        chipText.text = currentCoins.ToString() + "/60";
    }
}
