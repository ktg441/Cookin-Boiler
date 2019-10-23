using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private GameObject water;
    // Start is called before the first frame update
    void Start()
    {
        water = GameObject.Find("Full");
        water.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.eulerAngles.x > 45 && transform.eulerAngles.x < 135)
        {
            water.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Running Water"))
        {
            water.SetActive(true);
        }
    }
}
