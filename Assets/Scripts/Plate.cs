using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{

    public List<GameObject> attached;

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
                foreach (GameObject curr in attached.ToArray())
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
        if (other.CompareTag("NoCollide"))
        {
            Physics.IgnoreCollision(other, transform.parent.GetComponent<MeshCollider>(), true);
        }
        if (other.CompareTag("Food") && attached.Count < 10)
        {
            //print(other.name);
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.tag = "Untagged";
            other.transform.parent.SetParent(transform);
            attached.Add(other.gameObject);
            Physics.IgnoreCollision(other, transform.parent.GetComponent<MeshCollider>(), true);
            GameObject.Find("Controller (left)").GetComponent<Hand>().syncList();
            GameObject.Find("Controller (right)").GetComponent<Hand>().syncList();
            if (checkFullDish())
            {
                completeDish(transform.parent.parent.gameObject);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {

    }

    private bool checkFullDish()
    {
        bool food1, food2, food3, food4, food5, food6; //List of ingredients
        food1 = food2 = food3 = food4 = food5 = food6 = false;
        foreach (GameObject curr in attached)
        {
            if (curr.transform.parent.name.Contains("Onion") && (curr.transform.parent.GetComponent<Properties>().getHeat() > curr.transform.parent.GetComponent<Properties>().heatTime))
            {
                food1 = true;
            }
            else if (curr.transform.parent.name.Contains("Zucchini") && (curr.transform.parent.GetComponent<Properties>().getHeat() > curr.transform.parent.GetComponent<Properties>().heatTime))
            {
                food2 = true;
            }
            else if (curr.transform.parent.name.Contains("Garlic") && (curr.transform.parent.GetComponent<Properties>().getHeat() > curr.transform.parent.GetComponent<Properties>().heatTime))
            {
                food3 = true;
            }
            else if (curr.transform.parent.name.Contains("Corn") && (curr.transform.parent.GetComponent<Properties>().getHeat() > curr.transform.parent.GetComponent<Properties>().heatTime))
            {
                food4 = true;
            }
            else if (curr.transform.parent.name.Contains("Carrot"))
            {
                if (food5)
                {
                    food6 = true;
                }
                else
                {
                    food5 = true;
                }
            }
        }

        print("food1 " + food1);
        print("food2 " + food2);
        print("food3 " + food3);
        print("food4 " + food4);
        print("food5 " + food5);
        print("food6 " + food6);

        if (food1 && food2 && food3 && food4 && food5 && food6)
        {
            return true;
        }

        return false;
    }

    private void completeDish(GameObject parent)
    {
        Vector3 oldCoords = parent.GetComponent<Properties>().unfinished.transform.position;
        parent.GetComponent<Properties>().unfinished.SetActive(false);
        parent.GetComponent<Properties>().finished.transform.position = new Vector3(oldCoords.x, oldCoords.y + 0.03f, oldCoords.z);
        parent.GetComponent<Properties>().finished.SetActive(true);
        GameObject.Find("Controller (left)").GetComponent<Hand>().removeInactives();
        GameObject.Find("Controller (right)").GetComponent<Hand>().removeInactives();
    }
}
