using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class SimpleRandomWalkMapGenerator : AbstractDungeonGenerator
{

    [SerializeField]
    private int iterations = 10;

    [SerializeField]
    public int walkLength = 10;

    [SerializeField]
    public bool startRandomlyEachIteration = true;

    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(iterations, walkLength, startRandomlyEachIteration, startPosition);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(int iterationsNumber, int walkerLength, bool startRandom, Vector2Int startPosition)
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < iterationsNumber; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, walkerLength);
            floorPositions.UnionWith(path);
            if(startRandom) currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
        }

        return floorPositions;
    }

}
