using UnityEngine;

public class Ball : ProcessingLite.GP21
{
    Vector2 velocity;
    Vector2 position;
    int r;
    int g;
    int b;
    float size;

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

    public void UpdatePos()
    {
        position += velocity * Time.deltaTime;
        BouceWallDetection();
    }

    private void BouceWallDetection()
    {
        if (position.y < size)
        {
            position = new Vector3(position.x, size);
            velocity.y *= -1f;
        }
        else if (position.y > Height-size)
        {
            position = new Vector3(position.x, Height-size);
            velocity.y *= -1f;
        }

        if (position.x < size)
        {
            position = new Vector3(size,position.y);
            velocity.x *= -1f;
        }
        else if (position.x > Width-size)
        {
            position = new Vector3(Width-size, position.y);
            velocity.x *= -1f;
        }
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public float GetBallSize()
    {
        return size;
    }
}
