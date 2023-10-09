using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGeneration : SimpleRandomWalkFloorGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;

    [SerializeField]
  
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();
        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);
        //CreateCorridors(floorPositions, potentialRoomPositions);
        
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);
        
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);
        
        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);
       
        for(int i = 0; i< corridors.Count; i++)
        {
            corridors[i] = IncreaseCorridorSize(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }


        tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);
    }

    private List<Vector2Int> IncreaseCorridorSize(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int prevDir = Vector2Int.zero;
        for(int i = 1; i <corridor.Count; i++)
        {
            Vector2Int dirFromCell = corridor[i] - corridor[i - 1];
            if(prevDir != Vector2Int.zero && dirFromCell != prevDir)
            {
                for(int x = -1; x < 2; x++)
                {
                    for(int y = -1; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }
                prevDir = dirFromCell;
            }

            else
            {
                Vector2Int newCorridorTileOffset = GetDirection90From(dirFromCell);
                newCorridor.Add(corridor[i - 1]);

                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
            }
        }
        return newCorridor;
    }

    private Vector2Int GetDirection90From(Vector2Int dir)
    {
        if (dir == Vector2Int.up) return Vector2Int.right;
        if (dir == Vector2Int.right) return Vector2Int.down;

        if (dir == Vector2Int.down) return Vector2Int.left;

        if (dir == Vector2Int.left) return Vector2Int.up;
        return Vector2Int.zero;


    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach(var pos in deadEnds)
        {
            if(roomFloors.Contains(pos) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, pos);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach(var pos in floorPositions)
        {
            int neighboursCount = 0;
            foreach(var direction in Direction2D.CardinalDirectionList)
            {
                if (floorPositions.Contains(pos + direction)) neighboursCount++;
                
            }

            if (neighboursCount == 1) deadEnds.Add(pos);
        }

        return deadEnds;

    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        foreach(var roomPosition in roomToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currPos = startPosition;
        potentialRoomPositions.Add(currPos);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currPos, corridorLength);
            corridors.Add(corridor);
            currPos = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currPos);

            floorPositions.UnionWith(corridor);
        }
        return corridors;
    }
}
