using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Ball> balls;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] int maxBallAmount;
    int counter;
    // Start is called before the first frame update
    void Start()
    {
        balls = new List<Ball>();  
        for(int i = 0; i < 40000; i++)
        {
            SpawnBall();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(balls.Count);
        for (int i = 0; i < 100; i++)
        {
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        GameObject ballObject = Instantiate(ballPrefab);
        balls.Add(ballObject.GetComponent<Ball>());
    }
}
