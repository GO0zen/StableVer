using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    Player player;
    Text distanceText;
    

    GameObject results;
    GameObject records;

    Text finalDistanceText;
    Text recordText;


    bool closed;

    int rec = 0;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();
        results = GameObject.Find("Results");
        finalDistanceText = GameObject.Find("FinalDistanceText").GetComponent<Text>();
        records = GameObject.Find("Records");
        recordText = GameObject.Find("RecordText").GetComponent<Text>();

        results.SetActive(false);
        records.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        rec = PlayerPrefs.GetInt("Record");
    }

    // Update is called once per frame
    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + " m";

        if (player.isDead)
        {
            results.SetActive(true);
            finalDistanceText.text = distance + " m";
            if(distance < rec)
            {
                recordText.text = rec.ToString();
            }
            else
            {
                PlayerPrefs.SetInt("Record", distance);
                recordText.text = distance.ToString();
            }
            if (closed)
            {
                results.SetActive(false);
            }
        }   
    }


    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Records()
    {
        records.SetActive(true);
        closed = true;
    }
    
}
