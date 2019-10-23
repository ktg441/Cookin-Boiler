using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private GameObject water;
    private GameObject attached;
    // Start is called before the first frame update
    void Start()
    {
        water = GameObject.Find("Full");
        water.SetActive(false);
        attached = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.eulerAngles.x > 45 && transform.eulerAngles.x < 135)
        {
            water.SetActive(false);
            if (attached != null)
            {
                attached.transform.SetParent(null);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Running Water"))
        {
            water.SetActive(true);
        }
        if (other.CompareTag("Food"))
        {
            other.transform.SetParent(transform);
            attached = other.gameObject;
            //other.transform.position = transform.position;
        }
    }

}
