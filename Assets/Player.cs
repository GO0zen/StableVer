using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float gravity;
    public Vector2 velocity;
    public float maxXVelocity = 100;
    public float maxAcceleration = 10;
    public float acceleration = 10;
    public float distance = 0;
    public float jumpVelocity = 20;
    public float groundHeight = 10;
    public bool isGrounded = false;

    public bool isHoldingJump = false;
    public float maxHoldJumpTime = 0.4f;
    public float maxMaxHoldJumpTime = 0.4f;
    public float holdJumpTimer = 0.0f;

    public float jumpGroundThreshold = 1;

    public bool isDead = false;

    public LayerMask groundLayerMask;
    public LayerMask obstacleLayerMask;
    public LayerMask transfromatorLayerMask;
    public LayerMask boostLayerMask;
    public LayerMask coinLayerMask;

    GroundFall fall;
    CameraController cameraController;
    powerManager powerUp;

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void Update()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);


        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;

                if (fall != null)
                {
                    fall.player = null;
                    fall = null;
                    cameraController.StopShaking();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }



    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            velocity.x = 0;
            return;
        }

        Vector2 pos = transform.position;

        if (pos.y < -20)
        {
            isDead = true;
            
        }

        if (!isGrounded)
        {
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }


            pos.y += velocity.y * Time.fixedDeltaTime;
            if (!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }

            Vector2 rayOrigin = new Vector2(pos.x + 0.5f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, groundLayerMask);
            if (hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y >= ground.groundHeight)
                    {
                        groundHeight = ground.groundHeight;
                        pos.y = groundHeight;
                        velocity.y = 0;
                        isGrounded = true;
                    }

                    fall = ground.GetComponent<GroundFall>();
                    if (fall != null)
                    {
                        fall.player = this;
                        cameraController.StartShaking();
                    }
                }
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);


            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            Vector2 wallDir = Vector2.right;
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, wallDir, velocity.x * Time.fixedDeltaTime, groundLayerMask);
            if (wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y < ground.groundHeight)
                    {
                        velocity.x = 0;
                    }
                }
            }

            if(velocity.y < 0)
            {
                Vector2 trObstOrigin = new Vector2(pos.x - 0.5f, pos.y);
                RaycastHit2D obstHitTrY = Physics2D.Raycast(trObstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, transfromatorLayerMask);
                if (obstHitTrY.collider != null)
                {
                    TrObst trans = obstHitTrY.collider.GetComponent<TrObst>();
                    if (trans != null)
                    {
                        if (pos.y <= trans.edgeHeight)
                        {
                            groundHeight = trans.edgeHeight;
                            //Debug.Log(groundHeight);
                            pos.y = groundHeight;
                            velocity.y = 0;
                            isGrounded = true;
                        }
                    }
                }
            }
        }

        distance += velocity.x * Time.fixedDeltaTime;

        if (isGrounded)
        {
            
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;

            
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }


            Vector2 rayOrigin = new Vector2(pos.x - 0.6f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            if (fall != null)
            {
                rayDistance = -fall.fallSpeed * Time.fixedDeltaTime;
            }
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }



        }

        Vector2 obstOrigin = new Vector2(pos.x, pos.y);
        RaycastHit2D obstHitX = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, obstacleLayerMask);
        if (obstHitX.collider != null)
        {
            Obstacle obstacle = obstHitX.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                hitObstacle(obstacle);
            }
        }

        RaycastHit2D obstHitY = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, obstacleLayerMask);
        if (obstHitY.collider != null)
        {
            Obstacle obstacle = obstHitY.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                hitObstacle(obstacle);
            }
        }


        Vector2 glassOrigin = new Vector2(pos.x, pos.y);
        RaycastHit2D glassHitX = Physics2D.Raycast(glassOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, obstacleLayerMask);
        if (glassHitX.collider != null)
        {
            GlassBox glassBox = glassHitX.collider.GetComponent<GlassBox>();
            if (glassBox != null)
            {
                hitBox(glassBox);
            }
        }

        RaycastHit2D glassHitY = Physics2D.Raycast(glassOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, obstacleLayerMask);
        if (glassHitY.collider != null)
        {
            GlassBox glassBox = glassHitY.collider.GetComponent<GlassBox>();
            if (glassBox != null)
            {
                hitBox(glassBox);
            }
        }

        Vector2 boostOrigin = new Vector2(pos.x + 0.1f, pos.y);
        RaycastHit2D boostHitX = Physics2D.Raycast(boostOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, boostLayerMask);
        if (boostHitX.collider != null)
        {
            Boost boost = boostHitX.collider.GetComponent<Boost>();
            powerUp = GameObject.Find("powerManager").GetComponent<powerManager>();
            if (boost != null)
            {
                Destroy(boost.gameObject);
                
                if (powerUp.powerActive == false)
                {
                    powerUp.powerActive = true;
                }
                Debug.Log(powerUp.powerActive);
            }
        }

        RaycastHit2D boostHitY = Physics2D.Raycast(boostOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, boostLayerMask);
        if (boostHitY.collider != null)
        {
            Boost boost = boostHitY.collider.GetComponent<Boost>();
            powerUp = GameObject.Find("powerManager").GetComponent<powerManager>();
            if (boost != null)
            {
                Destroy(boost.gameObject);
                
                if (powerUp.powerActive == false)
                {
                    powerUp.powerActive = true;
                }
                Debug.Log(powerUp.powerActive);
            }
        }

        Vector2 coinOrigin = new Vector2(pos.x, pos.y);
        RaycastHit2D coinHitX = Physics2D.Raycast(coinOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, coinLayerMask);
        if (coinHitX.collider != null)
        {
            Coin coin = coinHitX.collider.GetComponent<Coin>();
            if (coin != null)
            {
                Destroy(coin.gameObject);
            }
        }

        RaycastHit2D coinHitY = Physics2D.Raycast(boostOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, coinLayerMask);
        if (coinHitY.collider != null)
        {
            Coin coin = coinHitY.collider.GetComponent<Coin>();
            
            if (coin != null)
            {
                Destroy(coin.gameObject);
            }
        }


        transform.position = pos;
    }


    void hitObstacle(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);
        velocity.x *= 0.7f;
    }

    void hitBox(GlassBox box)
    {
        velocity.x = 0;
        isDead = true;
        Destroy(gameObject);
        Destroy(box.gameObject);    
    }
}
