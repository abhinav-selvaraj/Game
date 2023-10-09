using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startingPos, int lenght)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startingPos);

        var previousPos = startingPos;

        for (int i = 0; i < lenght; i++)
        {
            var newPos = previousPos + Direction2D.GetRandDirection();
            path.Add(newPos);
            previousPos = newPos;
        }
        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandDirection();
        var currentPosition = startPosition;

        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }

        return corridor;
    }
}

public static class Direction2D
{
    public static List<Vector2Int> CardinalDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //up
        new Vector2Int(1,0), //right
        new Vector2Int(0,-1), //down
        new Vector2Int(-1,0),//left

    };

    public static Vector2Int GetRandDirection()
    {
        return CardinalDirectionList[Random.Range(0, CardinalDirectionList.Count)];
    }
}
