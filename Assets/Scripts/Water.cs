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
                attached.GetComponent<Rigidbody>().isKinematic = false;
                attached.transform.parent.SetParent(GameObject.Find("Interactables").transform.parent);
                attached.tag = "Food";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Running Water"))
        {
            water.SetActive(true);
        }
        if (other.CompareTag("Food") && water.activeInHierarchy)
        {
            //print(other.name);
            other.GetComponent<Rigidbody>().isKinematic = true;
            if(other.GetComponent<Interactable>().m_ActiveHand)
            {
                other.GetComponent<Interactable>().m_ActiveHand.m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
                other.GetComponent<Interactable>().m_ActiveHand.Drop();          
            }
            other.tag = "Untagged";
            other.transform.parent.SetParent(transform);
            attached = other.gameObject;
        }
    }

}
