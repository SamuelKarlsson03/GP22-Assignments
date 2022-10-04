using UnityEngine;

class Example : IRandomWalker
{
    //Add your own variables here.
    //Do not use processing variables like width or height

    Vector2 currentPos;
    int width;
    int height;

	public string GetName()
	{
		return "Kalle"; //When asked, tell them our walkers name
	}

	public Vector2 GetStartPosition(int playAreaWidth, int playAreaHeight)
	{
		//Select a starting position or use a random one.
		float x = Random.Range(0, playAreaWidth);
		float y = Random.Range(0, playAreaHeight);
        width = playAreaWidth;
        height = playAreaHeight;
        currentPos = new Vector2(x,y);
		//a PVector holds floats but make sure its whole numbers that are returned!
		return new Vector2(x, y);
	}

	public Vector2 Movement()
	{
		//add your own walk behavior for your walker here.
		//Make sure to only use the outputs listed below.
		Vector2 move = Vector2.zero;
        switch (Random.Range(0, 4))
        {
            case 0:
                move = new Vector2(-1, 0);
                break;
            case 1:
                move = new Vector2(1, 0);
                break;
            case 2:
                move = new Vector2(0, 1);
                break;
            default:
                move = new Vector2(0, -1);
                break;
        }
        while ((currentPos + move).x < 0 || (currentPos + move).x > width || (currentPos + move).y < 0 || (currentPos + move).y > height)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    move = new Vector2(-1, 0);
                    break;
                case 1:
                    move = new Vector2(1, 0);
                    break;
                case 2:
                    move = new Vector2(0, 1);
                    break;
                default:
                    move = new Vector2(0, -1);
                    break;
            }
        }
        currentPos += move;
        return move;
	}
}

//All valid outputs:
// Vector2(-1, 0);
// Vector2(1, 0);
// Vector2(0, 1);
// Vector2(0, -1);

//Any other outputs will kill the walker!
