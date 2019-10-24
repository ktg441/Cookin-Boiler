using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutting : MonoBehaviour
{

    private int numCuts;

    // Start is called before the first frame update
    void Start()
    {
        numCuts = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("User Knife"))
        {
            numCuts++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print(numCuts);
        if (numCuts >= 5)
        {
            cutObject(transform.parent.gameObject);
        }
    }

    void cutObject(GameObject parent)
    {
        Vector3 oldCoords = parent.GetComponent<Properties>().unfinished.transform.position;
        parent.GetComponent<Properties>().unfinished.SetActive(false);
        parent.GetComponent<Properties>().finished.transform.position = new Vector3(oldCoords.x, oldCoords.y + 0.05f, oldCoords.z);
        parent.GetComponent<Properties>().finished.SetActive(true);
    }
}
