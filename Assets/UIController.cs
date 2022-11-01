using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    Player player;
    Text distanceText;
    Text coinText;
    

    GameObject results;
    GameObject records;
    powerManager pwr;

    Text finalDistanceText;
    Text recordText;


    bool closed;

    int rec = 0;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();
        coinText = GameObject.Find("CoinText").GetComponent<Text>();
        results = GameObject.Find("Results");
        finalDistanceText = GameObject.Find("FinalDistanceText").GetComponent<Text>();
        records = GameObject.Find("Records");
        recordText = GameObject.Find("RecordText").GetComponent<Text>();
        pwr = GameObject.Find("powerManager").GetComponent<powerManager>();

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
        int coin = player.coinsNum;

        if (pwr.enabledByButton)
        {
            player.coinsNum -= 5;
            pwr.enabledByButton = false;    
        }

        coinText.text = "Coins: " + coin.ToString();

        

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
