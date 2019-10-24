using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour
{
    public GameObject unfinished;
    public GameObject finished;

    public int heatTime;
    private int heated;
    // Start is called before the first frame update
    void Start()
    {
        unfinished = transform.GetChild(0).gameObject;
        finished = transform.GetChild(1).gameObject;
        finished.SetActive(false);
        heated = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getHeat()
    {
        return heated;
    }

    public void setHeat(int newHeat)
    {
        heated = newHeat;
    }
}
