using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerManager : MonoBehaviour
{
    Player player;  
    public bool powerActive;
    public bool enabledByButton;

    public float lengthCounter = 7f;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (powerActive)
        {
            lengthCounter -= Time.deltaTime;

            if (lengthCounter <= 0.1)
            {
                powerActive = false;
                lengthCounter = 7;
            }
        }
        else if (player.coinsNum >= 5 && Input.GetKey(KeyCode.W))
        {

            enabledByButton = true;
            powerActive = true;
            lengthCounter -= Time.deltaTime;

            if (lengthCounter <= 0.1)
            {
                powerActive = false;
                lengthCounter = 7;
            }

            
        }
    }
}
