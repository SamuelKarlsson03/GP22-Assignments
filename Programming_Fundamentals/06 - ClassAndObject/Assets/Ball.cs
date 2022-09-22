using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//We still need to inherence from ProcessingLite
public class Ball : ProcessingLite.GP21
{
    //Our class variables
    Vector2 velocity; //Ball direction
    Vector2 position;
    int r;
    int g;
    int b;
    float size;

    //Ball Constructor, called when we type new Ball(x, y);
    public Ball(Vector2 playerPos)
    {
        position = GenerateValidSpawnPosition(playerPos);
        velocity = new Vector2();
        velocity.x = Random.Range(0f, 11f) - 5;
        velocity.y = Random.Range(0f, 11f) - 5;
        r = Random.Range(0,256);
        g = Random.Range(0,256);
        b = Random.Range(0,256);
        size = Random.Range(0.3f,0.8f);
    }

    public void Draw()
    {
        Stroke(r,g,b);
        Circle(position.x, position.y, size * 2);
    }

    private void Update()
    {
        UpdatePos();
        BouceWallDetection();
    }

    public Vector2 GenerateValidSpawnPosition(Vector2 playerPos)
    {
        int attemptsMade = 0;
        float safetyDistance = 3;
        Vector2 attemptedPos = new Vector2(Random.Range(0f, Width), Random.Range(0, Height));
        while ((playerPos - attemptedPos).sqrMagnitude < safetyDistance)
        {
            attemptedPos = new Vector2(Random.Range(0f, Width), Random.Range(0, Height));
            attemptsMade++;
            if (attemptsMade > 100)
            {
                return new Vector2(-1, -1);
            }
        }
        return attemptedPos;
    }

    public Vector2 GenerateValidSpawnPosition(Vector2 playerPos, float safetyDistance)
    {
        int attemptsMade = 0;
        Vector2 attemptedPos = new Vector2(Random.Range(0f, Width), Random.Range(0, Height));
        while ((playerPos-attemptedPos).sqrMagnitude < safetyDistance)
        {
            attemptedPos = new Vector2(Random.Range(0f, Width), Random.Range(0, Height));
            attemptsMade++;
            if(attemptsMade > 100)
            {
                return new Vector2(-1,-1);
            }
        }
        return attemptedPos;
    }

    //Update our ball
    public void UpdatePos()
    {
        position += velocity * Time.deltaTime;
    }

    private void BouceWallDetection()
    {
        if (position.y + Height < 0)
        {
            position = new Vector3(position.x, Height * -1f, 0);
            velocity.y *= -1f;
        }
        else if (position.y > Height)
        {
            position = new Vector3(position.x, Height, 0);
            velocity.y *= -1f;
        }

        if (position.x + (Width) < 0)
        {
            position = new Vector3(Width * -1f,position.y, 0);
            velocity.x *= -1f;
        }
        else if (position.x > Width)
        {
            position = new Vector3(Width, position.y, 0);
            velocity.x *= -1f;
        }
    }
}
