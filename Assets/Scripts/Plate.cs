using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{

    private List<GameObject> attached;

    // Start is called before the first frame update
    void Start()
    {
        attached = new List<GameObject>(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles.x > 45 && transform.eulerAngles.x < 135)
        {
            if (attached.Count > 0)
            {
                foreach (GameObject curr in attached)
                {
                    curr.GetComponent<Rigidbody>().isKinematic = false;
                    curr.transform.parent.SetParent(GameObject.Find("Interactables").transform.parent);
                    curr.tag = "Food";
                    attached.Remove(curr);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food") && attached.Count < 10)
        {
            //print(other.name);
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.tag = "Untagged";
            other.transform.parent.SetParent(transform);
            attached.Add(other.gameObject);
            if (other.GetComponent<Interactable>().m_ActiveHand != null)
            {
                other.GetComponent<Interactable>().m_ActiveHand.syncList();
            }
            if (checkFullDish())
            {
                completeDish(transform.parent.gameObject);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {

    }

    private bool checkFullDish()
    {
        bool food1, food2, food3, food4; //List of ingredients
        food1 = food2 = food3 = food4 = false;
        foreach (GameObject curr in attached)
        {
            if (curr.name.Equals(""))
            {
                //Set trues
            }
        }

        if(food1 && food2 && food3 && food4)
        {
            return true;
        }

        return false;
    }

    private void completeDish(GameObject parent)
    {
        Vector3 oldCoords = parent.GetComponent<Properties>().unfinished.transform.position;
        parent.GetComponent<Properties>().unfinished.SetActive(false);
        parent.GetComponent<Properties>().finished.transform.position = new Vector3(oldCoords.x, oldCoords.y + 0.05f, oldCoords.z);
        parent.GetComponent<Properties>().finished.SetActive(true);
    }
}
