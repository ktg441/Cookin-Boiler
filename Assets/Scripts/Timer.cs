using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int Minutes = 0;
    public int Seconds = 0;

    private Text m_text;
    private float m_leftTime;

    private GameObject[] racks;
    private GameObject[] racku;

    private void Awake()
    {
        m_text = GetComponent<Text>();
        m_leftTime = GetInitialTime();
    }

    private void Update()
    {
        if (m_leftTime > 0f)
        {
            //  Update countdown clock
            m_leftTime -= Time.deltaTime;
            Minutes = GetLeftMinutes();
            Seconds = GetLeftSeconds();

            //  Show current clock
            if (m_leftTime > 0f)
            {
                m_text.text = "Time Left : " + Minutes + ":" + Seconds.ToString("00");
            }
            else
            {
                //  The countdown clock has finished
                m_text.text = "Time : 0:00";
                racks = GameObject.FindGameObjectsWithTag("Rack");
                racku = GameObject.FindGameObjectsWithTag("Rack_Utensil");
                foreach (GameObject e in racks)
                {
                    e.SetActive(false);

                }
                foreach (GameObject e in racku)
                {
                    e.SetActive(false);
                }
            }
        }
    }

    private float GetInitialTime()
    {
        return Minutes * 60f + Seconds;
    }

    private int GetLeftMinutes()
    {
        return Mathf.FloorToInt(m_leftTime / 60f);
    }

    private int GetLeftSeconds()
    {
        return Mathf.FloorToInt(m_leftTime % 60f);
    }
}