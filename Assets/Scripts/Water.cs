using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private GameObject water;
    private GameObject attached;

    private bool heating;
    private int start, end;
    // Start is called before the first frame update
    void Start()
    {
        water = GameObject.Find("Full");
        water.SetActive(false);
        attached = null;
        heating = false;
        start = end = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.eulerAngles.x > 45 && transform.eulerAngles.x < 135)
        {
            water.SetActive(false);
            if (attached != null)
            {
                attached.GetComponent<Rigidbody>().isKinematic = false;
                attached.transform.parent.SetParent(GameObject.Find("Interactables").transform.parent);
                attached.tag = "Food";
            }
        }

        if (heating)
        {
            start += (int)Time.deltaTime % 60;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Running Water"))
        {
            water.SetActive(true);
        }
        if (water.activeInHierarchy)
        {
            if (other.CompareTag("Food"))
            {
                //print(other.name);
                other.GetComponent<Rigidbody>().isKinematic = true;
                if (other.GetComponent<Interactable>().m_ActiveHand)
                {
                    other.GetComponent<Interactable>().m_ActiveHand.m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
                    other.GetComponent<Interactable>().m_ActiveHand.Drop();
                }
                other.tag = "Untagged";
                other.transform.parent.SetParent(transform);
                attached = other.gameObject;
            }

            if (other.CompareTag("Heat"))
            {
                heating = true;
                start = other.transform.parent.GetComponent<Properties>().getHeat();
                end = other.transform.parent.GetComponent<Properties>().heatTime;
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (water.activeInHierarchy)
        {
            if (other.CompareTag("Heat"))
            {
                print("Current time: " + start);
                heating = false;
                other.transform.parent.GetComponent<Properties>().setHeat(start);
                start = 0;
                end = 0;
            }
        }
    }

}
