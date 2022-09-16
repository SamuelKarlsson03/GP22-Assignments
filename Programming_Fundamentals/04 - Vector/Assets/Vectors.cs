using UnityEngine;

public class Vectors : ProcessingLite.GP21
{

    public Vector2 circlePos;
    public float radius;
    public float circleMoveSpeed;
    public float maxSpeed;
    Vector2 circleMouseVector;

    void Start()
    {
        circleMouseVector = Vector2.zero;
        Stroke(255);
    }

    void Update()
    {
        Background(0);
        Vector2 mousePos = new Vector2((Input.mousePosition.x/Screen.width)*Width,(Input.mousePosition.y/Screen.height)*Height);
        if (Input.GetMouseButtonDown(0))
        {
            circlePos = mousePos;
        }
        if (Input.GetMouseButton(0))
        {
            Line(circlePos.x,circlePos.y, mousePos.x, mousePos.y);
            circleMouseVector = circlePos - mousePos;
        }
        //Update circle position based on clamped movement between -maxSpeed and maxSpeed.
        circlePos += Vector2.Max(Vector2.Min(circleMouseVector,new Vector2(maxSpeed,maxSpeed)),new Vector2(-1*maxSpeed,-1*maxSpeed)) * Time.deltaTime * circleMoveSpeed;
        if (circlePos.x < 0 || circlePos.x > Width)
        {
            circleMouseVector.x *= -1;
            circlePos.x = Mathf.Clamp(circlePos.x, 0, Width);
        }
        if (circlePos.y < 0 || circlePos.y > Height)
        {
            circleMouseVector.y *= -1;
            circlePos.y = Mathf.Clamp(circlePos.y,0,Height);
        }
        Circle(circlePos.x, circlePos.y, radius * 2);

    }
}
