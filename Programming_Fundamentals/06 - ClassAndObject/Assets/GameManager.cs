using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : ProcessingLite.GP21
{
    private List<Ball> balls;

    [SerializeField] Player player;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] int maxBallAmount;
    [SerializeField] float spawnCooldown;
    float timeSinceLastSpawn;

    float score;
    bool gameOver = false;

    // Start is called before the first frame update
    private void Start()
    {
        Fill(255);
        StartGame();
    }
    void StartGame()
    {
        gameOver = false;
        score = 0;
        balls = new List<Ball>();
        timeSinceLastSpawn = 0;
        for(int i = 0; i < 3; i++)
        {
            SpawnBall();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Background(0);
        if (gameOver)
        {
            TextSize(20);
            Text($"Score:{score:0} Press [R] to restart",5,Height/2);
            StrokeWeight(1f);
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }
        else
        {
            timeSinceLastSpawn += Time.deltaTime;
            score += Time.deltaTime * 100;
            if (timeSinceLastSpawn >= spawnCooldown)
            {
                timeSinceLastSpawn -= spawnCooldown;
                if (balls.Count < maxBallAmount)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        SpawnBall();
                    }
                }
            }
            MoveBalls();
            if (CheckPlayerCollision())
            {
                gameOver = true;
            }

        }
        DrawBalls();
        DrawPlayer();
    }

    private void RestartGame()
    {
        StartGame();
    }
    private bool CheckPlayerCollision()
    {
        for(int i = 0; i < balls.Count; i++)
        {
            Vector2 ballPos = balls[i].GetPosition();
            float ballSize = balls[i].GetBallSize();
            Vector2 playerPos = player.GetPosition();
            float playerSize = player.GetPlayerSize();
            if ((ballPos - playerPos).magnitude < ballSize + playerSize)
            {
                return true;
            }
        }
        return false;
    }

    public void DrawBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].Draw();
        }
    }

    public void DrawPlayer()
    {
        player.DrawPlayer();
    }

    private void MoveBalls()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].UpdatePos();
        }
    }

    private void SpawnBall()
    {
        balls.Add(new Ball(player.GetPosition()));
    }
}
