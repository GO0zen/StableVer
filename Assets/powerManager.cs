using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerManager : MonoBehaviour
{
    public bool powerActive;

    public float lengthCounter = 7f;

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
    }
}
