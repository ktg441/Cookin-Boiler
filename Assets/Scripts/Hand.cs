using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour
{
    public SteamVR_Action_Boolean m_GrabAction = null;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private FixedJoint m_Joint = null;

    private Interactable m_CurrentInteractable = null;
    public List<Interactable> m_ContactInteractables = new List<Interactable>(); //Make private when done testing. Make sure to add grip to hands

    private bool handLock;

    private int plateCount;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
        handLock = false;
        plateCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Down
        if(m_GrabAction.GetStateDown(m_Pose.inputSource))
        {
            //print(m_Pose.inputSource + " Trigger Down");
            Pickup();
        }

        //Up
        if (m_GrabAction.GetStateUp(m_Pose.inputSource))
        {
            //print(m_Pose.inputSource + " Trigger Up");
            Drop();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //print("Tags " + other.name + " " + other.tag);
        if (!other.gameObject.CompareTag("Utensil") && !other.gameObject.CompareTag("Rack") && !other.gameObject.CompareTag("Food"))
            return;

        if(other.name.Equals("knife-rack"))
        {
            m_ContactInteractables.Add(GameObject.Find("User Knife").GetComponent<Interactable>());
        }
        else if(other.name.Equals("spoon-rack"))
        {
            m_ContactInteractables.Add(GameObject.Find("User Spoon").GetComponent<Interactable>());
        }
        else if(other.name.Equals("ladle-rack"))
        {
            m_ContactInteractables.Add(GameObject.Find("User Ladle").GetComponent<Interactable>());
        }
        else if(other.name.Equals("wok-rack"))
        {
            m_ContactInteractables.Add(GameObject.Find("User Pan").GetComponent<Interactable>());
        }
        else if (other.name.Equals("pot-rack"))
        {
            m_ContactInteractables.Add(GameObject.Find("User Pot").GetComponent<Interactable>());
        }
        else if (other.name.Equals("plate-rack"))
        {
            m_ContactInteractables.Add(GameObject.Find("Dish" + plateCount).GetComponent<Interactable>());
        }
        else
        {
            m_ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (!other.gameObject.CompareTag("Utensil") && !other.gameObject.CompareTag("Rack") && !other.gameObject.CompareTag("Food"))
            return;

        if (other.name.Equals("knife-rack"))
        {
            m_ContactInteractables.Remove(GameObject.Find("User Knife").GetComponent<Interactable>());
        }
        else if (other.name.Equals("spoon-rack"))
        {
            m_ContactInteractables.Remove(GameObject.Find("User Spoon").GetComponent<Interactable>());
        }
        else if (other.name.Equals("ladle-rack"))
        {
            m_ContactInteractables.Remove(GameObject.Find("User Ladle").GetComponent<Interactable>());
        }
        else if (other.name.Equals("wok-rack"))
        {
            m_ContactInteractables.Remove(GameObject.Find("User Pan").GetComponent<Interactable>());
        }
        else if (other.name.Equals("pot-rack"))
        {
            m_ContactInteractables.Remove(GameObject.Find("User Pot").GetComponent<Interactable>());
        }
        else if (other.name.Equals("plate-rack"))
        {
            m_ContactInteractables.Remove(GameObject.Find("Dish" + plateCount).GetComponent<Interactable>());
        }
        else
        {
            m_ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
        }
    }

    public void syncList()
    {
        foreach (Interactable current in m_ContactInteractables.ToArray())
        {
            if (current.gameObject.tag.Equals("Untagged"))
            {
                Drop();
                m_ContactInteractables.Remove(current);
            }
        }
    }

    public void Pickup()
    {
        //Get nearest interactable
        m_CurrentInteractable = GetNearestInteractable();

        //Null check
        if (!m_CurrentInteractable)
            return;

        //Already held, check
        if (m_CurrentInteractable.m_ActiveHand)
            m_CurrentInteractable.m_ActiveHand.Drop();

        //Position
        m_CurrentInteractable.transform.position = transform.position;

        if (m_CurrentInteractable.name.Equals("User Knife"))
        {
            if (!handLock)
            {
                GameObject.Find("User Knife").transform.eulerAngles = new Vector3(0f, m_CurrentInteractable.transform.eulerAngles.y , m_CurrentInteractable.transform.eulerAngles.z);
                handLock = true;
            }
        }
        else if(m_CurrentInteractable.name.Equals("User Spoon"))
        {
            if (!handLock)
            {
                GameObject.Find("User Spoon").transform.eulerAngles = new Vector3(m_CurrentInteractable.transform.eulerAngles.x, m_CurrentInteractable.transform.eulerAngles.y, 0f);
                handLock = true;
            }

        }
        else if (m_CurrentInteractable.name.Equals("User Ladle"))
        {
            if (!handLock)
            {
                GameObject.Find("User Ladle").transform.eulerAngles = new Vector3(0f, m_CurrentInteractable.transform.eulerAngles.y, m_CurrentInteractable.transform.eulerAngles.z);
                //m_CurrentInteractable.transform.position = new Vector3(m_CurrentInteractable.transform.position.x, m_CurrentInteractable.transform.position.y - 0.15f, m_CurrentInteractable.transform.position.z + 0f);
                handLock = true;
            }
        }
        else if (m_CurrentInteractable.name.Equals("User Pan"))
        {
            if (!handLock)
            {
                GameObject.Find("User Pan").transform.eulerAngles = new Vector3(-90f, m_CurrentInteractable.transform.eulerAngles.y, m_CurrentInteractable.transform.eulerAngles.z);
                handLock = true;
            }
        }
        else if (m_CurrentInteractable.name.Equals("User Pot"))
        {
            if (!handLock)
            {
                GameObject.Find("User Pot").transform.eulerAngles = new Vector3(-90f, m_CurrentInteractable.transform.eulerAngles.y, m_CurrentInteractable.transform.eulerAngles.z);
                GameObject.Find("User Pot").transform.position = new Vector3(m_CurrentInteractable.transform.position.x , m_CurrentInteractable.transform.position.y - 0.05f, m_CurrentInteractable.transform.position.z);
                handLock = true;
            }
        }
        else if (m_CurrentInteractable.name.Contains("Dish"))
        {
            if (!handLock && plateCount < 5)
            {
                GameObject.Find("Dish" + plateCount).transform.eulerAngles = new Vector3(m_CurrentInteractable.transform.eulerAngles.x, m_CurrentInteractable.transform.eulerAngles.y, 0f);
                GameObject.Find("Dish" + plateCount).transform.position = new Vector3(m_CurrentInteractable.transform.position.x, m_CurrentInteractable.transform.position.y, m_CurrentInteractable.transform.position.z);
                handLock = true;
                plateCount++;
            }
        }


        // Attach
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        m_Joint.connectedBody = targetBody;

        //Set active hand
        m_CurrentInteractable.m_ActiveHand = this;
    }

    public void Drop()
    {
        //Null check
        if (!m_CurrentInteractable)
            return;

        handLock = false;
        //Apply velocity
        Rigidbody targetBody = m_CurrentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = m_Pose.GetVelocity();
        targetBody.angularVelocity = m_Pose.GetAngularVelocity();

        //Detach
        m_Joint.connectedBody = null;

        //Clear
        m_CurrentInteractable.m_ActiveHand = null;
        m_CurrentInteractable = null;
    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach(Interactable interactable in m_ContactInteractables)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude;

            if(distance < minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        }

        return nearest;
    }
}
