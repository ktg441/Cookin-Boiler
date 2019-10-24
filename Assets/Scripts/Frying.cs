using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frying : MonoBehaviour
{
    private GameObject attached;
    // Start is called before the first frame update
    void Start()
    {
        attached = null;
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
            if (other.GetComponent<Interactable>().m_ActiveHand != null)
            {
                other.GetComponent<Interactable>().m_ActiveHand.syncList();
            }
        }
    }
}
