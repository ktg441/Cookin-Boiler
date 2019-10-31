using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frying : MonoBehaviour
{
    private GameObject attached;

    private bool heating, stirring;
    private float start, end;

    // Start is called before the first frame update
    void Start()
    {
        attached = null;
        heating = false;
        stirring = false;
        start = end = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles.x > 45 && transform.eulerAngles.x < 135)
        {
            if (attached != null)
            {
                attached.GetComponent<Rigidbody>().isKinematic = false;
                attached.transform.parent.SetParent(GameObject.Find("Interactables").transform.parent);
                attached.tag = "Food";
            }
        }

        if (heating)
        {
            if (start < 10f && transform.GetChild(0).GetComponent<Properties>().finished.activeInHierarchy && stirring)
            {
                start += Time.deltaTime;
                print("While heating" + start);
                var block = new MaterialPropertyBlock();
                block.SetColor("_BaseColor", Color.Lerp(Color.grey, Color.red, start / end));
                transform.GetChild(0).GetComponent<Properties>().finished.GetComponent<Renderer>().SetPropertyBlock(block); //Change when I get real models in
                Renderer[] renderers = transform.GetChild(0).GetComponent<Properties>().finished.GetComponentsInChildren<Renderer>();
                foreach (Renderer e in renderers)
                {
                    e.SetPropertyBlock(block);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            //print(other.name);
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.tag = "Untagged";
            other.transform.parent.SetParent(transform);
            attached = other.gameObject;
            GameObject.Find("Controller (left)").GetComponent<Hand>().syncList();
            GameObject.Find("Controller (right)").GetComponent<Hand>().syncList();
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
        if (other.name.Equals("User Spoon"))
        {
            stirring = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
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
        if (other.name.Equals("User Spoon"))
        {
            stirring = false;
        }
    }
}
