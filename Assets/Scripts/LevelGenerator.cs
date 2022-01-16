using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Vector3 currentPos;
    private Vector3 obstaclePos;
    private List<GameObject> currentTerrainList;
    private GameObject currentObject;
    private bool generateWater;

    [SerializeField] private List<GameObject> levelObjects;
    [SerializeField] private List<GameObject> obstacles;

    int previousTerrain;
    int currentTerrain;

    void Start()
    {
        currentTerrainList = new List<GameObject>();
        currentPos = new Vector3(-12, 0, 0);
        generateWater = false;
        for (int i = 0; i < 16; i++)
        {
            GenerateGrassOnly();
        }
        for (int i = 0; i < 30; i++)
        {
            if (i == 8)
                generateWater = true;
            GenerateLevel();
        }
    }

    public void GenerateLevel()
    {
        GameObject currentTerrainObject;
        while (currentTerrain == previousTerrain || currentTerrain == 3 || (!generateWater && currentTerrain == 2))
            currentTerrain = Random.Range(0, levelObjects.Count);
        int terrainLoop = Random.Range(1, 6);
        if (currentTerrain == 2 && currentPos.y == 0)
            currentPos.y = -0.4f;
        for (int i = 0; i < terrainLoop; i++)
        {
            if (currentTerrain == 2 && generateWater && Random.Range(0, 4) == 1)
                currentTerrain = 3;
            currentTerrainObject = Instantiate(levelObjects[currentTerrain], currentPos, Quaternion.identity);
            currentTerrainList.Add(currentTerrainObject);
            if (currentTerrain == 0)
            {
                obstaclePos = new Vector3(currentPos.x + 0.25f, currentPos.y, -49);
                while (obstaclePos.z < 49)
                {
                    if (Random.Range(0, 8) == 0)
                    {
                        Instantiate(obstacles[0], obstaclePos, Quaternion.identity).transform.parent = currentTerrainObject.transform;
                    }
                    obstaclePos.z++;
                }
            }
            else if (currentTerrain == 2)
            {
                obstaclePos = new Vector3(currentPos.x, currentPos.y + 0.53f, -49);
                while (obstaclePos.z < 49)
                {
                    if (Random.Range(0, 3) == 0)
                    {
                        Instantiate(obstacles[1], obstaclePos, Quaternion.identity).transform.parent = currentTerrainObject.transform;
                    }
                    obstaclePos.z++;
                }
            }
            else if (currentTerrain == 4)
            {
                obstaclePos = new Vector3(currentPos.x, currentPos.y + 0.25f, -49);
                while (obstaclePos.z < 49)
                {
                    if (Random.Range(0, 8) == 0)
                    {
                        Instantiate(obstacles[2], obstaclePos, Quaternion.identity).transform.parent = currentTerrainObject.transform;
                    }
                    obstaclePos.z++;
                }
            }
            currentPos.x++;
            obstaclePos.x++;
            if (currentTerrain == 3)
                currentTerrain = 2;
        }
        if (currentTerrain == 2 && currentPos.y == -0.4f)
            currentPos.y = 0;
        previousTerrain = currentTerrain;
    }

    private void GenerateGrassOnly()
    {
        currentTerrainList.Add(Instantiate(levelObjects[0], currentPos, Quaternion.identity));
        currentPos.x++;
        previousTerrain = 0;
        currentTerrain = 0;
    }

    public void DestroyTerrain()
    {
        for (int i = 0; i < 2; i++)
            Destroy(currentTerrainList[i]);
        currentTerrainList.RemoveRange(0, 2);
    }
}