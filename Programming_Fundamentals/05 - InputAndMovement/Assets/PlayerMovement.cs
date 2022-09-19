using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] GameObject playerObject;
    private GameObject phantomObject;
    [SerializeField] float maxSpeed = 50f;
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float acceleration = 40f;
    [SerializeField] float deceleration = 20f;
    [SerializeField] float gravity = 9.8f;
    [Range(0, 1)]
    [SerializeField] float bouceyness = 0.8f;
    private Camera cam;
    private float currentXVel;
    private float currentYVel;
    private bool gravityOn;
    [SerializeField] bool useAcceleration = true;

    // Start is called before the first frame update
    void Start()
    {
        playerObject = this.gameObject;
        phantomObject = Instantiate(playerObject);
        Destroy(phantomObject.GetComponent<PlayerMovement>());
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 normalized = new Vector2(x, y).normalized;
        x = normalized.x;
        y = normalized.y;

        Decelerate(x,y);

        if (!useAcceleration)
        {
            playerObject.transform.position += new Vector3(x*Time.deltaTime*normalSpeed,y*Time.deltaTime*normalSpeed,0);
        }
        else
        {
            UpdateVelocity(x,y);
            Move();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGravity();
        }

        if (gravityOn)
        {
            UpdateGravity();
        }

        ClampYPosition();
        WrapXPosition();
        UpdatePhantomPosition();
    }

    private void Decelerate(float x, float y)
    {
        if (x == 0)
        {
            currentXVel /= Mathf.Pow(deceleration, Time.deltaTime);
        }
        if (y == 0 && !gravityOn)
        {
            currentYVel /= Mathf.Pow(deceleration, Time.deltaTime);
        }
    }

    private void UpdateVelocity(float x, float y)
    {
        currentXVel = Mathf.Clamp(currentXVel + (x * Time.deltaTime * acceleration), -1f * maxSpeed, maxSpeed);
        currentYVel = Mathf.Clamp(currentYVel + (y * Time.deltaTime * acceleration), -1f * maxSpeed, maxSpeed);
    }

    private void Move()
    {
        playerObject.transform.position += new Vector3(currentXVel * Time.deltaTime, currentYVel * Time.deltaTime, 0);
    }

    private void ToggleGravity()
    {
        gravityOn = !gravityOn;
    }

    private void UpdateGravity()
    {
        currentYVel -= gravity * Time.deltaTime;
    }

    private void ClampYPosition()
    {
        if (playerObject.transform.position.y + cam.orthographicSize < 0)
        {
            playerObject.transform.position = new Vector3(playerObject.transform.position.x, cam.orthographicSize * -1f, 0);
            currentYVel *= -1f * bouceyness;
        }
        if (playerObject.transform.position.y > cam.orthographicSize)
        {
            playerObject.transform.position = new Vector3(playerObject.transform.position.x, cam.orthographicSize, 0);
            currentYVel *= -1f * bouceyness;
        }
    }

    private void WrapXPosition()
    {
        if (playerObject.transform.position.x + (cam.orthographicSize * cam.aspect) < 0)
        {
            playerObject.transform.position = new Vector3(playerObject.transform.position.x + cam.orthographicSize * cam.aspect * 2, playerObject.transform.position.y, 0);
        }
        if (playerObject.transform.position.x > cam.aspect * cam.orthographicSize)
        {
            playerObject.transform.position = new Vector3(playerObject.transform.position.x - cam.orthographicSize * cam.aspect * 2, playerObject.transform.position.y, 0);
        }
    }

    private void UpdatePhantomPosition()
    {
        if (playerObject.transform.position.x < 0)
        {
            phantomObject.transform.position = new Vector3(playerObject.transform.position.x + (cam.orthographicSize * cam.aspect * 2), playerObject.transform.position.y, 0);
        }
        else
        {
            phantomObject.transform.position = new Vector3(playerObject.transform.position.x - (cam.orthographicSize * cam.aspect * 2), playerObject.transform.position.y, 0);

        }
    }
}
