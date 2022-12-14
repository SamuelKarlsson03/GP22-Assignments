using System;
using System.Collections.Generic;
using UnityEngine;

public class WalkerTest : ProcessingLite.GP21
{
	//This file is only for testing your movement/behavior.
	//The Walkers will compete in a different program!

	List<IRandomWalker> walkers;
	List<Vector2> walkerPos;
	List<Vector3> walkerColors;
	float scaleFactor = 0.02f;
	List<bool> walkerAlive;

	void Start()
	{
		//Some adjustments to make testing easier
		//Application.targetFrameRate = 120;
		QualitySettings.vSyncCount = 0;
		walkers = new List<IRandomWalker>();
		walkerPos = new List<Vector2>();
		walkerColors = new List<Vector3>();
		walkerAlive = new List<bool>();
		//Create a walker from the class Example it has the type of WalkerInterface
		//walkerColors.Add(new Vector3(Random.Range(128, 255), Random.Range(128, 255), Random.Range(128, 255)));

		for (int i = 0; i < 1; i++)
		{
			walkers.Add(new SamKar());
			//walkerColors.Add(new Vector3(Random.Range(128, 255), Random.Range(128, 255), Random.Range(128, 255)));
			walkerColors.Add(new Vector3(100+15*i, 0, 0));
			walkerAlive.Add(true);
		}
        for (int i = 0; i < 200; i++)
        {
            walkers.Add(new Example());
            //walkerColors.Add(new Vector3(Random.Range(128, 255), Random.Range(128, 255), Random.Range(128, 255)));
            walkerColors.Add(new Vector3(0, 255, 0));
			walkerAlive.Add(false);
		}
        //walkers.Add(new Example());
        //walkerColors.Add(new Vector3(0, 255, 0));
        //walkers.Add(new SamKar());
        //walkerColors.Add(new Vector3(0, 0, 255));
        //Get the start position for our walker.
        for (int i = 0; i < walkers.Count; i++)
		{
			walkerPos.Add(walkers[i].GetStartPosition((int)(Width / scaleFactor), (int)(Height / scaleFactor)));
		}
	}

	void Update()
	{
		for (int ticksInFrame = 0; ticksInFrame < 1; ticksInFrame++)
		{
			//Draw the walker
			for (int i = 0; i < walkers.Count; i++)
			{
				Stroke((int)walkerColors[i].x, (int)walkerColors[i].y, (int)walkerColors[i].z);
				Point(walkerPos[i].x * scaleFactor, walkerPos[i].y * scaleFactor);
			}
			//Get the new movement from the walker.
			for (int i = 0; i < walkers.Count; i++)
			{
				if(walkerAlive[i])
                {
					walkerPos[i] += walkers[i].Movement();
                }
				for(int j = 0; j < walkers.Count; j++)
                {
					if(i != j)
                    {
						if(walkerPos[i] == walkerPos[j])
                        {
							walkerColors[i] = new Vector3(0,0,0);
							walkerColors[j] = new Vector3(0,0,0);
							walkerAlive[i] = false;
							walkerAlive[j] = false;
						}
                    }
                }
			}
		}
	}
}
