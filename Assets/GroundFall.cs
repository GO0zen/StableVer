using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFall : MonoBehaviour
{

    bool shouldFall = false;
    public float fallSpeed = 1;

    public Player player;
    public List<Obstacle> obstacles = new List<Obstacle>();
    public List<TrObst> transformators = new List<TrObst>();
    public List<GlassBox> glass = new List<GlassBox>();
    public List<Boost> boost = new List<Boost>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
     
        if (shouldFall)
        {
            Vector2 pos = transform.position;
            float fallAmount = fallSpeed * Time.fixedDeltaTime;
            pos.y -= fallAmount;

            if (player != null)
            {
                player.groundHeight -= fallAmount;
                Vector2 playerPos = player.transform.position;
                playerPos.y -= fallAmount;
                player.transform.position = playerPos;
            }

            foreach (Obstacle o in obstacles)
            {
                if (o != null)
                {
                    Vector2 oPos = o.transform.position;
                    oPos.y -= fallAmount;
                    o.transform.position = oPos;
                }
            }

            foreach (TrObst t in transformators)
            {
                if (t != null)
                {
                    Vector2 oPos = t.transform.position;
                    oPos.y -= fallAmount;
                    t.transform.position = oPos;
                }
            }

            foreach (GlassBox g in glass)
            {
                if (g != null)
                {
                    Vector2 oPos = g.transform.position;
                    oPos.y -= fallAmount;
                    g.transform.position = oPos;
                }
            }

            foreach (Boost b in boost)
            {
                if (b != null)
                {
                    Vector2 oPos = b.transform.position;
                    oPos.y -= fallAmount;
                    b.transform.position = oPos;
                }
            }

            transform.position = pos;
        }
        else
        {
            if (player != null)
            {
                shouldFall = true;
            }
        }
        

    }
}
