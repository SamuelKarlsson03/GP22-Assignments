using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamKar : IRandomWalker
{
	//Add your own variables here.
	int width;
	int height;

	int[] visited;
	int leftValue;
	int rightValue;
	int upValue;
	int downValue;
	int totalVisits;

	int leftMoves;
	int rightMoves;
	int upMoves;
	int downMoves;

	string holderName = "Holder";
	GameObject holderObject;
	int myHolderIndex;
	float holderScale;
	Transform[] holderTransforms;
	bool hasInitializedHolder = false;
	int initializedHolderCounter;

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

		visited = new int[width * height];

		float x = Random.Range(0 + width * 0.25f, playAreaWidth * 0.75f);
		float y = Random.Range(0 + height * 0.25f, playAreaHeight * 0.75f);
		x = Mathf.FloorToInt(x);
		y = Mathf.FloorToInt(y);
		currentPos = new Vector2(x,y);
		visited[VisitedIndex(x, y)] = -1;
		totalVisits++;

		//a PVector holds floats but make sure its whole numbers that are returned!
		return new Vector2(x, y);
	}

	public Vector2 Movement()
	{
		initializedHolderCounter++;
		if (!hasInitializedHolder && initializedHolderCounter >= 10)
		{
			InitializeHolderProperties();
            hasInitializedHolder = true;
		}
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
		if (hasInitializedHolder)
		{
			Vector2 closestEnemy = GetPosOfClosestEnemy();
			float scaredDistance = 5;
			float leftScore = visited[VisitedIndex(currentPos + new Vector2(-1, 0))];
			if (Vector2.Distance(closestEnemy, currentPos) > Vector2.Distance(closestEnemy, currentPos + new Vector2(-1, 0)))
			{
				leftScore -= Mathf.Pow((Vector2.Distance(closestEnemy, currentPos) / Vector2.Distance(closestEnemy, currentPos + new Vector2(-1, 0))), scaredDistance)-1;
			}
			float rightScore = visited[VisitedIndex(currentPos + new Vector2(1, 0))];
			if (Vector2.Distance(closestEnemy, currentPos) > Vector2.Distance(closestEnemy, currentPos + new Vector2(1, 0)))
			{
				rightScore -= Mathf.Pow((Vector2.Distance(closestEnemy, currentPos) / Vector2.Distance(closestEnemy, currentPos + new Vector2(1, 0))), scaredDistance)-1;
			}
			float upScore = visited[VisitedIndex(currentPos + new Vector2(0, 1))];
			if (Vector2.Distance(closestEnemy, currentPos) > Vector2.Distance(closestEnemy, currentPos + new Vector2(0, 1)))
			{
				upScore -= Mathf.Pow((Vector2.Distance(closestEnemy, currentPos) / Vector2.Distance(closestEnemy, currentPos + new Vector2(0, 1))), scaredDistance)-1;
			}
			float downScore = visited[VisitedIndex(currentPos + new Vector2(0, -1))];
			if (Vector2.Distance(closestEnemy, currentPos) > Vector2.Distance(closestEnemy, currentPos + new Vector2(0, -1)))
			{
				downScore -= Mathf.Pow((Vector2.Distance(closestEnemy, currentPos) / Vector2.Distance(closestEnemy, currentPos + new Vector2(0, -1))), scaredDistance)-1;
			}

			float[] scores = new float[] { leftScore, rightScore, upScore, downScore };
			Debug.Log($"left:{leftScore} right{rightScore} up{upScore} down{downScore}");
			int largest = FindLargestInArray(scores);
			switch (largest)
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
		float value;
		int attempts = 0;
		while ((currentPos + move).x < 0 || (currentPos + move).x > width || (currentPos + move).y < 0 || (currentPos + move).y > height || visited[VisitedIndex((currentPos + move).x, (currentPos + move).y)] < 0)
		{
			attempts++;
			if (attempts >= 100)
			{
				GetNewPath();
				move = StepTowardsPathDestination();
				break;
			}
            value = Random.Range(0, 4);

            if (value == 0)
            {
                move = new Vector2(1, 0);
            }
            else if (value == 1)
            {
                move = new Vector2(-1, 0);
            }
            else if (value == 2)
            {
                move = new Vector2(0, 1);
            }
            else if (value == 3)
            {
                move = new Vector2(0, -1);
            }
        }
		//while(((currentPos + move).x < 0 || (currentPos + move).x > width || (currentPos + move).y < 0 || (currentPos + move).y > height)
		if(hasInitializedHolder)
		{
			GetObjectsInHolder();
		}
		Debug.Log(TilesOwned());
		UpdateDirValues(move);
		return move;
	}

	private void UpdateDirValues(Vector2 dir)
    {
		currentPos += dir;
		if (visited[VisitedIndex(currentPos.x, currentPos.y)] != -1)
		{
			totalVisits++;
		}
		visited[VisitedIndex(currentPos.x, currentPos.y)] = -1;
	}

	private void GetNewPath()
    {
		int counter = 1;
		bool pathFound = false;
		while(!pathFound)
        {
			counter++;
			Vector2 attemptedPath = new Vector2(Random.Range(Mathf.Max(currentPos.x - (counter * 3),0), Mathf.Min(currentPos.x + (counter * 3),width)+1), Random.Range(Mathf.Max(currentPos.y - (counter * 3),0), Mathf.Min(currentPos.y + (counter * 3),height)+1));
			if(visited[VisitedIndex(attemptedPath.x,attemptedPath.y)] != -1)
            {
				attemptedPath.x = Mathf.FloorToInt(attemptedPath.x);
				attemptedPath.y = Mathf.FloorToInt(attemptedPath.y);
				pathPosition = attemptedPath;
				hasPath = true;
				pathFound = true;
				return;
            }
			if(counter >= 200)
			{
				for(int i = 0; i< visited.Length; i++)
				{
					visited[i] = 0;
				}
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
		if(ix >= width)
		{
			ix = width-1;
		}
		if(iy >= height)
		{
			iy = height-1;
		}
		ix = Mathf.Max(ix, 0);
		iy = Mathf.Max(iy, 0);
		return (iy * width) + ix;
	}
    private int VisitedIndex(Vector2 position)
    {
        int ix = Mathf.FloorToInt(position.x);
        int iy = Mathf.FloorToInt(position.y);
        if (ix >= width)
        {
            ix = width - 1;
        }
        if (iy >= height)
        {
            iy = height - 1;
        }
        ix = Mathf.Max(ix, 0);
        iy = Mathf.Max(iy, 0);
        return (iy * width) + ix;
    }

    private void InitializeHolderProperties()
	{
        Transform[] childTransforms = GameObject.Find("Holder").GetComponentsInChildren<Transform>();
		holderTransforms = childTransforms;
		bool foundScale = false;
		for (int scale = 0; scale < 100; scale++)
		{
            for (int i = 1; i < childTransforms.Length; i++)
			{
				if (IsSimilar(childTransforms[i].localPosition.x / (0.005f * scale),currentPos.x) && IsSimilar(childTransforms[i].localPosition.y / (0.005f * scale),currentPos.y))
				{
					holderScale = scale * 0.005f;
					myHolderIndex = i;
					foundScale = true;
					break;
				}
			}
			if(foundScale)
			{
				Debug.Log("Scale " + holderScale);
				break;
			}
		}
    }

	private void GetObjectsInHolder()
	{
		for(int i = 1; i < holderTransforms.Length; i++)
		{
			if (i != myHolderIndex)
			{
				if (visited[VisitedIndex(holderTransforms[i].localPosition / holderScale)] != 1)
				{
					visited[VisitedIndex(holderTransforms[i].localPosition / holderScale)] = 1;
				}

			}
		}
	}

	private Vector2 GetPosOfClosestEnemy()
	{
		Vector2 closestEnemy = new Vector2();
		float closestDist = 9999;
        for (int i = 1; i < holderTransforms.Length; i++)
        {
            if (i != myHolderIndex)
            {
				if (Vector2.Distance(holderTransforms[i].localPosition / holderScale,currentPos) < closestDist)
				{
					closestDist = Vector2.Distance(holderTransforms[i].localPosition / holderScale, currentPos);
					closestEnemy = holderTransforms[i].localPosition / holderScale;

                }
            }
        }
		return closestEnemy;
    }

	private int TilesOwned()
	{
		int count = 0;
		for(int i = 0; i < visited.Length; i++)
		{
			if (visited[i] == -1)
			{
				count++;
			}
		}
		return count;
	}

    private bool IsSimilar(float a, float b)
	{
		if(a - b < 0.001f && a - b > -0.001f)
		{
			return true;
		}
		else return false;
	}

	private int FindLargestInArray(float[] arr)
	{
		float largest = -9999999f;
		int largestIndex = 0;
		for(int i = 0; i < arr.Length; i++)
		{
			if (arr[i] > largest)
			{
				largest = arr[i];
				largestIndex = i;
			}
		}
		return largestIndex;
	}
}
