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

    public int heatTime = 1; 

    private GameObject fake = null;
    private GameObject parents = null;
    public GameObject onionPrefab = null;
    public GameObject garlicPrefab = null;
    public GameObject zucchiniPrefab = null;
    public GameObject cornPrefab = null;
    public GameObject carrotPrefab = null;
    public GameObject platePrefab = null;

    private int nextOnion;
    private int nextGarlic;
    private int nextZucch;
    private int nextCorn;
    private int nextCarrot;
    private int nextPlate;

    private bool isOnion;
    private bool isGarlic;
    private bool isZucchini;
    private bool isCorn;
    private bool isCarrot;
    private bool isPlate;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
        m_Joint = GetComponent<FixedJoint>();
        handLock = false;
        fake = GameObject.Find("Fake");
        isOnion = false;
        isGarlic = false;
        isZucchini = false;
        isCorn = false;
        isCarrot = false;
        isPlate = false;
        parents = GameObject.Find("FoodInstantiate");
        nextOnion = 0;
        nextGarlic = 0;
        nextZucch = 0;
        nextCorn = 0;
        nextCarrot = 0;
        nextPlate = 0;
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
        else if (other.name.Equals("onion-rack") || other.name.Equals("garlic-rack") || other.name.Equals("zucchini-rack") || other.name.Equals("corn-rack") || other.name.Equals("carrot-rack") || other.name.Equals("plate-rack"))
        {
            switch(other.name)
            {
                case "onion-rack":
                    isOnion = true;
                    break;
                case "garlic-rack":
                    isGarlic = true;
                    break;
                case "zucchini-rack":
                    isZucchini = true;
                    break;
                case "corn-rack":
                    isCorn = true;
                    break;
                case "carrot-rack":
                    isCarrot = true;
                    break;
                case "plate-rack":
                    isPlate = true;
                    break;
            }
            m_ContactInteractables.Add(fake.GetComponent<Interactable>());
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
        else if (other.name.Equals("onion-rack") || other.name.Equals("garlic-rack") || other.name.Equals("zucchini-rack") || other.name.Equals("corn-rack") || other.name.Equals("carrot-rack") || other.name.Equals("plate-rack"))
        {
            m_ContactInteractables.Remove(fake.GetComponent<Interactable>());
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

    public void removeInactives()
    {
        foreach (Interactable current in m_ContactInteractables.ToArray())
        {
            if (current.gameObject.activeInHierarchy == false)
            {
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

        if (m_CurrentInteractable.name.Equals("Fake"))
        {
            print("It's fake");
            if (!handLock)
            {
                if (isOnion)
                {
                    GameObject newOnion = Instantiate(onionPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
                    newOnion.name = newOnion.name + " " + nextOnion;
                    newOnion.GetComponent<Properties>().heatTime = 3f;
                    nextOnion++;
                    newOnion.transform.SetParent(parents.transform);
                    m_CurrentInteractable = newOnion.transform.GetChild(0).GetComponent<Interactable>();
                    isOnion = false;
                }
                else if (isGarlic)
                {
                    GameObject newGarlic = Instantiate(garlicPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
                    newGarlic.name = newGarlic.name + " " + nextGarlic;
                    newGarlic.GetComponent<Properties>().heatTime = 3f;
                    nextGarlic++;
                    newGarlic.transform.SetParent(parents.transform);
                    m_CurrentInteractable = newGarlic.transform.GetChild(0).GetComponent<Interactable>();
                    isGarlic = false;
                }
                else if (isZucchini)
                {
                    GameObject newZucchini = Instantiate(zucchiniPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
                    newZucchini.name = newZucchini.name + " " + nextZucch;
                    newZucchini.GetComponent<Properties>().heatTime = 3f;
                    nextZucch++;
                    newZucchini.transform.SetParent(parents.transform);
                    m_CurrentInteractable = newZucchini.transform.GetChild(0).GetComponent<Interactable>();
                    isZucchini = false;
                }
                else if (isCorn)
                {
                    GameObject newCorn = Instantiate(cornPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
                    newCorn.name = newCorn.name + " " + nextCorn;
                    newCorn.GetComponent<Properties>().heatTime = 7f;
                    nextCorn++;
                    newCorn.transform.SetParent(parents.transform);
                    m_CurrentInteractable = newCorn.transform.GetChild(0).GetComponent<Interactable>();
                    isCorn = false;
                }
                else if (isCarrot)
                {
                    GameObject newCarrot = Instantiate(carrotPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
                    newCarrot.name = newCarrot.name + " " + nextCarrot;
                    newCarrot.GetComponent<Properties>().heatTime = 7f;
                    nextCarrot++;
                    newCarrot.transform.SetParent(parents.transform);
                    m_CurrentInteractable = newCarrot.transform.GetChild(0).GetComponent<Interactable>();
                    isCarrot = false;
                }
                else if (isPlate)
                {
                    GameObject newPlate = Instantiate(platePrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
                    newPlate.name = newPlate.name + " " + nextPlate;
                    nextPlate++;
                    newPlate.transform.SetParent(parents.transform);
                    m_CurrentInteractable = newPlate.transform.GetChild(0).GetComponent<Interactable>();
                    isPlate = false;
                }
                //newOnion.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                handLock = true;
            }
        }

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
        else if (m_CurrentInteractable.name.Equals("Dish"))
        {
            if (!handLock)
            {
                GameObject parentDish = GameObject.Find("Dish").transform.GetChild(0).gameObject.activeInHierarchy ? GameObject.Find("Dish").transform.GetChild(0).gameObject : GameObject.Find("Dish").transform.GetChild(1).gameObject;
                parentDish.transform.eulerAngles = new Vector3(m_CurrentInteractable.transform.eulerAngles.x, m_CurrentInteractable.transform.eulerAngles.y, 0f);
                parentDish.transform.position = new Vector3(m_CurrentInteractable.transform.position.x, m_CurrentInteractable.transform.position.y, m_CurrentInteractable.transform.position.z);
                handLock = true;
            }
        }
        else if (m_CurrentInteractable.name.Equals("Finished Food"))
        {
            if (!handLock)
            {
                GameObject.Find("Finished Food").transform.position = new Vector3(m_CurrentInteractable.transform.position.x, m_CurrentInteractable.transform.position.y + 0.1f, m_CurrentInteractable.transform.position.z);
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
