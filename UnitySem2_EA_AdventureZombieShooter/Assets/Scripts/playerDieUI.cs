using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerDieUI : MonoBehaviour
{

    public Text zombieKilled_count;
    public Text score;
    public GameObject UI;


    // Start is called before the first frame update
    void Start()
    {
        zombieKilled_count=  GetComponent<Text>();
        score = GetComponent<Text>();
        UI = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
