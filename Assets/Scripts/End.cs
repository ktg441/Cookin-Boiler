using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{

    private GameObject[] racks;
    private GameObject[] racku;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals("Finished Food"))
        {
            racks = GameObject.FindGameObjectsWithTag("Rack");
            racku = GameObject.FindGameObjectsWithTag("Rack_Utensil");
            foreach (GameObject e in racks)
            {
                e.SetActive(false);

            }
            foreach (GameObject e in racku)
            {
                e.SetActive(false);
            }
            GameObject.Find("Timer").SetActive(false);
        }
    }
}
