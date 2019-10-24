using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faucet : MonoBehaviour
{

    private bool state;
    private GameObject water;

    // Start is called before the first frame update
    void Start()
    {
        state = false;
        water = GameObject.Find("Running Water");
        water.SetActive(state);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            state = !state;
            water.SetActive(state);
        }
    }

}