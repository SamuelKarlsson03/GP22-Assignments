using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamKar : IRandomWalker
{
	//Add your own variables here.
	int width;
	int height;

	bool[] visited;
	int leftValue;
	int rightValue;
	int upValue;
	int downValue;
	int totalVisits;

	bool hasPath;
	Vector2 pathPosition;

	Vector2 currentPos;
	//Do not use processing variables like width or height

	public string GetName()
	{
		return "NameOfWinner";
	}

	public Vector2 GetStartPosition(int playAreaWidth, int playAreaHeight)
	{
		width = playAreaWidth;
		height = playAreaHeight;

		visited = new bool[width * height];

		float x = Random.Range(0 + width * 0.25f, playAreaWidth * 0.75f);
		float y = Random.Range(0 + height * 0.25f, playAreaHeight * 0.75f);
		x = Mathf.FloorToInt(x);
		y = Mathf.FloorToInt(y);
		currentPos = new Vector2(x,y);
		visited[VisitedIndex(x, y)] = true;
		totalVisits++;

		//a PVector holds floats but make sure its whole numbers that are returned!
		return new Vector2(x, y);
	}

	public Vector2 Movement()
	{
		Debug.Log(pathPosition);
		if(hasPath)
        {
			Vector2 pathMovement = StepTowardsPathDestination();
			UpdateDirValues(pathMovement);
			return pathMovement;
		}
		//add your own walk behavior for your walker here.
		//Make sure to only use the outputs listed below.
		Vector2 move = Vector2.zero;

		float xPercent = currentPos.x / width;
		float yPercent = currentPos.y / height;

		float cirlceFormingFactor = 50;

		float distanceFromCenter = Vector2.Distance(new Vector2(width / 2, height / 2), currentPos);
		distanceFromCenter /= Mathf.Min(width, height);

		float leftScore = distanceFromCenter - (Vector2.Distance(new Vector2(width / 2, height / 2), currentPos + new Vector2(-1,0))) / Mathf.Min(width, height);
		leftScore += 1;
		leftScore = Mathf.Pow(leftScore, cirlceFormingFactor);
		float rightScore = distanceFromCenter - (Vector2.Distance(new Vector2(width / 2, height / 2), currentPos + new Vector2(1, 0))) / Mathf.Min(width, height);
		rightScore += 1;
		rightScore = Mathf.Pow(rightScore, cirlceFormingFactor);
		float upScore = distanceFromCenter - (Vector2.Distance(new Vector2(width / 2, height / 2), currentPos + new Vector2(0, 1))) / Mathf.Min(width, height);
		upScore +=  1;
		upScore = Mathf.Pow(upScore, cirlceFormingFactor);
		float downScore = distanceFromCenter - (Vector2.Distance(new Vector2(width / 2, height / 2), currentPos + new Vector2(0, -1))) / Mathf.Min(width, height);
		downScore += 1;
		downScore = Mathf.Pow(downScore, cirlceFormingFactor);

  //      float xStrength = 0;
  //      float left = 2;

  //      float yStrength = 0;
  //      float down = 2;

  //      if (xPercent < 0.5f)
  //      {
  //          xStrength = Mathf.Pow(1 - (Mathf.Abs(0.25f - xPercent)), 1);
  //          if (xPercent > 0.25)
  //          {
  //              left = 0.5f;
  //          }
  //      }
  //      else if (xPercent >= 0.5f)
  //      {
  //          xStrength = Mathf.Pow(1 - (Mathf.Abs(0.75f - xPercent)), 1);
  //          if (xPercent < 0.75)
  //          {
  //              left = 0.5f;
  //          }
  //      }

  //      if (yPercent < 0.5f)
  //      {
  //          yStrength = Mathf.Pow(1 - (Mathf.Abs(0.25f - yPercent)), 1);
  //          if (yPercent > 0.25)
  //          {
  //              down = 0.5f;
  //          }
  //      }
  //      else if (yPercent >= 0.5f)
  //      {
  //          yStrength = Mathf.Pow(1 - (Mathf.Abs(0.75f - yPercent)), 1);
  //          if (yPercent < 0.75)
  //          {
  //              down = 0.5f;
  //          }
  //      }
  //      left = 1;
		//xStrength = 1;
		//down = 1;
		//yStrength = 1;

        float leftWeight = (1f / (1f + (leftValue/totalVisits)));
		float rightWeight = (1f / (1f + (rightValue / totalVisits)));
		float downWeight = (1f / (1f + (downValue / totalVisits)));
		float upWeight = (1f / (1f + (upValue / totalVisits)));
		float totalWeight = leftWeight + rightWeight + downWeight + upWeight;
		float value = Random.value * totalWeight;

		if (value < leftWeight)
		{
			move = new Vector2(-1, 0);
		}
		else if (value < rightWeight + leftWeight)
		{
			move = new Vector2(1, 0);
		}
		else if (value < upWeight + rightWeight + leftWeight)
		{
			move = new Vector2(0, 1);
		}
		else
		{
			move = new Vector2(0, -1);
		}
		int attempts = 0;
		while ((currentPos + move).x < 0 || (currentPos + move).x > width || (currentPos + move).y < 0 || (currentPos + move).y > height )
		{
			while (visited[VisitedIndex((currentPos + move).x, (currentPos + move).y)])
			{
				attempts++;
				if (attempts >= 100)
				{
					GetNewPath();
					move = StepTowardsPathDestination();
					break;
				}
				value = Random.value * totalWeight;

				if (value < leftWeight)
				{
					move = new Vector2(-1, 0);
				}
				else if (value < rightWeight + leftWeight)
				{
					move = new Vector2(1, 0);
				}
				else if (value < upWeight + rightWeight + leftWeight)
				{
					move = new Vector2(0, 1);
				}
				else
				{
					move = new Vector2(0, -1);
				}
			}
		}
		//while(((currentPos + move).x < 0 || (currentPos + move).x > width || (currentPos + move).y < 0 || (currentPos + move).y > height)
		UpdateDirValues(move);
		return move;
	}

	private void UpdateDirValues(Vector2 dir)
    {
		if(dir.x == -1)
        {
			int rightValueToAdd = 0;
			for(int i = 0; i < height; i++)
            {
				if(visited[VisitedIndex(currentPos.x - 1, i)])
                {
					rightValueToAdd++;
                }
            }
			rightValue += rightValueToAdd;
        }
		if (dir.x == 1)
		{
			int leftValueToAdd = 0;
			for (int i = 0; i < height; i++)
			{
				if (visited[VisitedIndex(currentPos.x + 1, i)])
				{
					leftValueToAdd++;
				}
			}
			leftValue += leftValueToAdd;
		}
		if (dir.y == -1)
		{
			int upValueToAdd = 0;
			for (int i = 0; i < width; i++)
			{
				if (visited[VisitedIndex(i, currentPos.y -1)])
				{
					upValueToAdd++;
				}
			}
			upValue += upValueToAdd;
		}
		if (dir.x == 1)
		{
			int downValueToAdd = 0;
			for (int i = 0; i < width; i++)
			{
				if (visited[VisitedIndex(i, currentPos.y + 1)])
				{
					downValueToAdd++;
				}
			}
			downValue += downValueToAdd;
		}

		currentPos += dir;
		visited[VisitedIndex(currentPos.x, currentPos.y)] = true;
		totalVisits++;
	}

	private void GetNewPath()
    {
		int counter = 1;
		bool pathFound = false;
		while(!pathFound)
        {
			counter++;
			Vector2 attemptedPath = new Vector2(Random.Range(Mathf.Max(currentPos.x - (counter * 3),0), Mathf.Min(currentPos.x + (counter * 3),width)), Random.Range(Mathf.Max(currentPos.y - (counter * 3),0), Mathf.Min(currentPos.y + (counter * 3),height)));
			if(!visited[VisitedIndex(attemptedPath.x,attemptedPath.y)])
            {
				attemptedPath.x = Mathf.FloorToInt(attemptedPath.x);
				attemptedPath.y = Mathf.FloorToInt(attemptedPath.y);
				pathPosition = attemptedPath;
				hasPath = true;
				pathFound = true;
				return;
            }
        }
    }

	private Vector2 StepTowardsPathDestination()
    {
		Vector2 pathMovement = Vector2.zero;
		if (currentPos.x > pathPosition.x)
		{
			pathMovement = new Vector2(-1, 0);
		}
		else if (currentPos.y > pathPosition.y)
		{
			pathMovement = new Vector2(0, -1);
		}
		else if (currentPos.x < pathPosition.x)
        {
			pathMovement = new Vector2(1, 0);
        }
		else if (currentPos.y < pathPosition.y)
		{
			pathMovement = new Vector2(0, 1);
		}
		
		if((currentPos + pathMovement) == pathPosition)
        {
			hasPath = false;
        }
		return pathMovement;
	}

	private int VisitedIndex(int x, int y)
    {
		return (y * width) + x;
    }
	private int VisitedIndex(float x, float y)
	{
		int ix = Mathf.FloorToInt(x);
		int iy = Mathf.FloorToInt(y);
		if ((iy * width) + ix > visited.Length || (iy * width) + ix < 0)
        {
			return 0;
        }
		return (iy * width) + ix;
	}
}
