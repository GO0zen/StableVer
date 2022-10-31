using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrObst : MonoBehaviour
{
    Player player;

    public BoxCollider2D collider;
    public Vector2 pos;
    public float edgeHeight;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        collider = GetComponent<BoxCollider2D>();
        
    }
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

        edgeHeight = transform.position.y + (collider.size.y * 87.25f);
        //Debug.Log(edgeHeight);
        Vector2 pos = transform.position;

        pos.x -= player.velocity.x * Time.fixedDeltaTime;
        if (pos.x < -100)
        {
            Destroy(gameObject);
        }

        transform.position = pos;
    }
}

