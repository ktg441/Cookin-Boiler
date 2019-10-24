using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private GameObject water;
    private GameObject attached;

    private bool heating;
    private float start, end;
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
            if(start < 10f)
            {
                start += Time.deltaTime;
                print("While heating" + start);
                var block = new MaterialPropertyBlock();
                block.SetColor("_BaseColor", Color.Lerp(Color.grey, Color.red, start/end));
                transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().SetPropertyBlock(block); //Change when I get real models in
            }
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
                other.tag = "Untagged";
                other.transform.parent.SetParent(transform);
                attached = other.gameObject;
                if (other.GetComponent<Interactable>().m_ActiveHand != null)
                {
                    other.GetComponent<Interactable>().m_ActiveHand.syncList();
                }
            }

            if (other.CompareTag("Heat"))
            {
                if (transform.childCount > 0)
                {
                    heating = true;
                    start = transform.GetChild(0).gameObject.GetComponent<Properties>().getHeat();
                    end = transform.GetChild(0).gameObject.GetComponent<Properties>().heatTime;
                    print("Current start is : " + start);
                }
                
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (water.activeInHierarchy)
        {
            
        }
        if (other.CompareTag("Heat"))
        {
            if (transform.childCount > 0)
            {
                print("Current time: " + start);
                heating = false;
                transform.GetChild(0).gameObject.GetComponent<Properties>().setHeat(start);
            }
            start = 0;
            end = 0;
        }
    }

}
