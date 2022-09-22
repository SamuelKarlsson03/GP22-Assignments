using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//We still need to inherence from ProcessingLite
public class Ball : MonoBehaviour
{
    //Our class variables
    Vector2 velocity; //Ball direction
    Camera cam;
    float size;

    //Ball Constructor, called when we type new Ball(x, y);
    public void CreateBall(float x, float y)
    {
        //Create the velocity vector and give it a random direction.
        velocity = new Vector2();
        velocity.x = Random.Range(0f, 11f) - 5;
        velocity.y = Random.Range(0f, 11f) - 5;
        float r = Random.Range(0f,1f);
        float g = Random.Range(0f,1f);
        float b = Random.Range(0f,1f);
        int a = 1;
        size = Random.Range(0.3f,0.8f);
        this.gameObject.transform.localScale = new Vector3(size,size,1);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(r,g,b,a);
        cam = Camera.main;
    }
    private void Start()
    {
        CreateBall(0,0); 
    }   

    private void Update()
    {
        UpdatePos();
        BouceWallDetection();
    }

    //Update our ball
    public void UpdatePos()
    {
        this.gameObject.transform.position += (Vector3)velocity * Time.deltaTime;
    }

    private void BouceWallDetection()
    {
        if (this.gameObject.transform.position.y + cam.orthographicSize < 0)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, cam.orthographicSize * -1f, 0);
            velocity.y *= -1f;
        }
        else if (this.gameObject.transform.position.y > cam.orthographicSize)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, cam.orthographicSize, 0);
            velocity.y *= -1f;
        }

        if (this.gameObject.transform.position.x + (cam.orthographicSize*cam.aspect) < 0)
        {
            this.gameObject.transform.position = new Vector3(cam.orthographicSize * cam.aspect * -1f,this.gameObject.transform.position.y, 0);
            velocity.x *= -1f;
        }
        else if (this.gameObject.transform.position.x > cam.orthographicSize * cam.aspect)
        {
            this.gameObject.transform.position = new Vector3(cam.orthographicSize * cam.aspect, this.gameObject.transform.position.y, 0);
            velocity.x *= -1f;
        }
    }
}
