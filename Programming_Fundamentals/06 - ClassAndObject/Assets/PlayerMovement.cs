using UnityEngine;

public class PlayerMovement : ProcessingLite.GP21
{
    [SerializeField] float maxSpeed = 50f;
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float acceleration = 40f;
    [SerializeField] float deceleration = 20f;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float bouceyness = 0.8f;
    [SerializeField] bool useAcceleration = true;

    private float currentXVel;
    private float currentYVel;
    private bool gravityOn;

    float width;
    float height;
    float playerSize = 1;

    private void Start()
    {
        width = Width;
        height = Height;
    }

    public void SetPlayerSize(float size)
    {
        playerSize = size;
    }

    public Vector2 UpdateMovemnet(Vector2 pos)
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 normalized = new Vector2(x, y).normalized;
        x = normalized.x;
        y = normalized.y;
        Decelerate(x,y);

        if (!useAcceleration)
        {
            pos += new Vector2(x*Time.deltaTime*normalSpeed,y*Time.deltaTime*normalSpeed);
        }
        else
        {
            UpdateVelocity(x,y);
            pos += new Vector2(currentXVel * Time.deltaTime, currentYVel * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ToggleGravity();
        }

        if (gravityOn)
        {
            UpdateGravity();
        }

        pos = ClampYPosition(pos);
        pos = WrapXPosition(pos);
        return pos;
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

    private void ToggleGravity()
    {
        gravityOn = !gravityOn;
    }

    private void UpdateGravity()
    {
        currentYVel -= gravity * Time.deltaTime;
    }

    private Vector2 ClampYPosition(Vector2 pos)
    {
        if (pos.y < playerSize)
        {
            pos = new Vector3(pos.x, playerSize, 0);
            currentYVel *= -1f * bouceyness;
        }
        if (pos.y > height-playerSize)
        {
            pos = new Vector3(pos.x, height-playerSize, 0);
            currentYVel *= -1f * bouceyness;
        }
        return pos;
    }

    private Vector2 WrapXPosition(Vector2 pos)
    {
        while (pos.x < 0)
        {
            pos += new Vector2(Width, 0);
        }
        while (pos.x > width)
        {
            pos -= new Vector2(Width, 0);
        }
        return pos;
    }
}
