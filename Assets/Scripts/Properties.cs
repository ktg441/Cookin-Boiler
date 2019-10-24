using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour
{
    public GameObject unfinished;
    public GameObject finished;
    // Start is called before the first frame update
    void Start()
    {
        unfinished = transform.GetChild(0).gameObject;
        finished = transform.GetChild(1).gameObject;
        finished.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
