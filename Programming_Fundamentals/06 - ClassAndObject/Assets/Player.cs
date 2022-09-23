using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ProcessingLite.GP21
{
    Vector2 position;
    Vector2 phantomPosition;
    [SerializeField] PlayerMovement movement;
    [SerializeField] float playerSize = 1;

    private void Start()
    {
        position = new Vector2(Width/2,Height/2);
        movement.SetPlayerSize(playerSize);
    }

    void Update()
    {
        position = movement.UpdateMovemnet(position);
    }

    public void DrawPlayer()
    {
        Stroke(255,255,255);
        Circle(position.x, position.y, playerSize * 2);
        DrawPhantomPlayer();
    }

    private void DrawPhantomPlayer()
    {
        if (position.x < Width/2)
        {
            phantomPosition = new Vector2(position.x + Width, position.y);
        }
        else
        {
            phantomPosition = new Vector2(position.x - Width, position.y);
        }
        Circle(phantomPosition.x, phantomPosition.y, playerSize * 2);
    }

    public Vector2 GetPosition()
    {
        return position;
    }

    public float GetPlayerSize()
    {
        return playerSize;
    }
}
