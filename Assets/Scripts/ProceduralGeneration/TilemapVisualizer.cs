using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[System.Serializable]
public class Floor 
{
    public TileBase floorTile;
    [Range(0f, 100f)] public float chance = 100f;
    [HideInInspector] public double _weight;
}


public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;

    [SerializeField]
    private Floor[] floors;

    private double accumulatedWeights;
    private System.Random rand = new System.Random();


    [SerializeField]
    private TileBase wallTop,
    wallSideRight,
    wallSideLeft, 
    wallBottom, 
    wallFull, 
    wallInnerCornerDownLeft, 
    wallInnerCornerDownRight, 
    wallDiagonalCornerDownRight, 
    wallDiagonalCornerDownLeft, 
    wallDiagonalCornerUpRight, 
    wallDiagonalCornerUpLeft; //in future make an array and select random element to get a random floor and use it accordingly with walls tile

    private void Awake()
    {
        CalculateFloorWeights();
    }

    private int GetRandomFloorIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;

        for (int i = 0; i < floors.Length; i++)
            if(floors[i]._weight >= r) return i;
        return 0;
    }

    private void CalculateFloorWeights()
    {
        accumulatedWeights = 0f;
        foreach (Floor floor in floors)
        {
            accumulatedWeights += floor.chance;
            floor._weight = accumulatedWeights;
        }
    }

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        // PaintTiles(floorPositions, floorTilemap, floorToPaint.floorTile);
        foreach (var position in floorPositions)
        {
            Floor floorToPaint = floors[GetRandomFloorIndex()];
            PaintSingleTile(floorTilemap, floorToPaint.floorTile, position);
        }
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile) 
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position) 
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = System.Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if(WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if(WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if(WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if(WallTypesHelper.wallBottom.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if(WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        if(tile != null) PaintSingleTile(wallTilemap, tile, position);
    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = System.Convert.ToInt32(binaryType, 2);
        TileBase tile = null;

        if(WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownLeft;
        }
        else if(WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornerDownRight;
        }
        else if(WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if(WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerDownRight;
        }
        else if(WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if(WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if(WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if(WallTypesHelper.wallBottomEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }

        if(tile != null) PaintSingleTile(wallTilemap, tile, position);
        
    
    }



    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}
