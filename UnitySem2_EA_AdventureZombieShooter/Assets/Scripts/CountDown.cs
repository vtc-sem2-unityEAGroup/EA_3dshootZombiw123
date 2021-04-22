using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    public float myTimer = 60.0f;
    public Slider slider;
    public GameObject timesup;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();   
        slider.maxValue = myTimer;
        slider.minValue =0;
    }

    // Update is called once per frame
    void Update()
    {
        if (myTimer > 0) {
            myTimer -= Time.deltaTime;
            
        } else {
            myTimer = 0;
            timesup.SetActive(true);
            Time.timeScale = 0;
           //HealthScript.PlayerDied();
            if (true) 
                SceneManager.LoadScene("Menu");
            
            
        }
        slider.value = myTimer;
        text.text = "Time:" + (myTimer).ToString("00");
    }

}
