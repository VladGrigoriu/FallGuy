using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemPlacementHelper : MonoBehaviour
{
    Dictionary<PlacementType, HashSet<Vector2Int>> tileByType = new Dictionary<PlacementType, HashSet<Vector2Int>>();

    HashSet<Vector2Int> roomFloorNoCorridor;


    public void ItemPlacementHelperMethod(HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridor, List<GameObject> propsToPlace, HashSet<Vector2Int> playerSpawnRoom, List<BoundsInt> roomsList,  HashSet<Vector2Int> bossRoom, HashSet<Vector2Int> treasureRoom, List<GameObject> enemiesToPlace)
    {
        Graph graph = new Graph(roomFloor);
        roomFloorNoCorridor.ExceptWith(playerSpawnRoom);
        roomFloorNoCorridor.ExceptWith(treasureRoom);
        roomFloorNoCorridor.ExceptWith(bossRoom);
        this.roomFloorNoCorridor = roomFloorNoCorridor;

        foreach (var position in roomFloorNoCorridor)
        {
            int neighboursCount8Dir = graph.GetNeighbours8Directions(position).Count;
            PlacementType type = neighboursCount8Dir == 1 ? PlacementType.Corner : neighboursCount8Dir < 8 ?  PlacementType.NearWall : PlacementType.OpenSpace;

            if(tileByType.ContainsKey(type) == false) tileByType[type] = new HashSet<Vector2Int>();

            if(type == PlacementType.NearWall && graph.GetNeighbours4Directions(position).Count < 4) continue;
            tileByType[type].Add(position);
            
            PlaceItems(type, propsToPlace, position);
            PlaceEnemies(type, position, enemiesToPlace);
        }
        PlaceItemsBossRoom(propsToPlace, bossRoom);
        PlaceItemsTreasureRoom(propsToPlace, treasureRoom);

    }

    public void PlaceItems(PlacementType placementType, List<GameObject> propsToPlace, Vector2Int floorPosition)
    {
        // if(playerSpawnRoom.Contains(floorPosition)) return;
        // if(bossRoom.Contains(floorPosition)) return;
        // if(treasureRoom.Contains(floorPosition)) return;
        System.Random rand = new System.Random();
        if(rand.Next(100) < 10)
        {
            int index = rand.Next(propsToPlace.Count);
            Instantiate(propsToPlace[index], new Vector3(floorPosition.x, floorPosition.y, 0), Quaternion.identity);
            tileByType[placementType].Remove(floorPosition);
        }

    }

    public void PlaceEnemies(PlacementType placementType, Vector2Int floorPosition, List<GameObject> enemiesToPlace)
    {
        System.Random rand = new System.Random();
        if(rand.Next(100) < 1)
        {
            int index = rand.Next(enemiesToPlace.Count);
            Instantiate(enemiesToPlace[index], new Vector3(floorPosition.x, floorPosition.y, 0), Quaternion.identity);
            tileByType[placementType].Remove(floorPosition);
        }
    }

    public void  PlaceItemsBossRoom(List<GameObject> propsToPlace, HashSet<Vector2Int> bossRoom)
    {
        Graph graph = new Graph(bossRoom);
         foreach (var position in bossRoom)
        {
            int neighboursCount8Dir = graph.GetNeighbours8Directions(position).Count;
            PlacementType type = neighboursCount8Dir == 1 ? PlacementType.Corner : neighboursCount8Dir < 8 ?  PlacementType.NearWall : PlacementType.OpenSpace;

            if(tileByType.ContainsKey(type) == false) tileByType[type] = new HashSet<Vector2Int>();

            if(type == PlacementType.NearWall && graph.GetNeighbours4Directions(position).Count < 4) continue;
            tileByType[type].Add(position);
            
            // PlaceItems(type, propsToPlace, position);
            System.Random rand = new System.Random();
            if(rand.Next(100) < 100)
            {
                
                Instantiate(propsToPlace[0], new Vector3(position.x, position.y, 0), Quaternion.identity);
                tileByType[type].Remove(position);
            
            }
        }

    }

    public void  PlaceItemsTreasureRoom(List<GameObject> propsToPlace, HashSet<Vector2Int> treasureRoom)
    {
        Graph graph = new Graph(treasureRoom);
         foreach (var position in treasureRoom)
        {
            int neighboursCount8Dir = graph.GetNeighbours8Directions(position).Count;
            PlacementType type = neighboursCount8Dir == 1 ? PlacementType.Corner : neighboursCount8Dir < 8 ?  PlacementType.NearWall : PlacementType.OpenSpace;

            if(tileByType.ContainsKey(type) == false) tileByType[type] = new HashSet<Vector2Int>();

            if(type == PlacementType.NearWall && graph.GetNeighbours4Directions(position).Count < 4) continue;
            tileByType[type].Add(position);
            
            // PlaceItems(type, propsToPlace, position);
            System.Random rand = new System.Random();
            if(rand.Next(100) < 100)
            {
                
                Instantiate(propsToPlace[4], new Vector3(position.x, position.y, 0), Quaternion.identity);
                tileByType[type].Remove(position);
                
            }
        }

    }

    public Vector2? GetItemPlacementPosition(PlacementType placementType, int iterationsMax, Vector2Int size, bool addOffset)
    {
        int itemArea = size.x * size.y;
        if(tileByType[placementType].Count < itemArea) return null;

        int iteration = 0;
        while (iteration < iterationsMax)
        {
            iteration++;
            int index = UnityEngine.Random.Range(0, tileByType[placementType].Count);
            Vector2Int position = tileByType[placementType].ElementAt(index);

            if(itemArea > 1)
            {
                var (result, placementPositions) = PlaceBigItem(position, size, addOffset);

                if(result == false) continue;

                tileByType[placementType].ExceptWith(placementPositions);
                tileByType[PlacementType.NearWall].ExceptWith(placementPositions);
            }
            else
            {
                tileByType[placementType].Remove(position);
            }
            return position;
        }
        return null;
    }

    private (bool, List<Vector2Int>) PlaceBigItem(Vector2Int originPosition, Vector2Int size, bool addOffset)
    {
        List<Vector2Int> positions = new List<Vector2Int>() { originPosition };
        int maxX = addOffset ? size.x + 1 : size.x;
        int maxY = addOffset ? size.y +1 : size.y;
        int minX = addOffset ? -1 : 0;
        int minY = addOffset ? -1 : 0;

        for (int row = minX; row <= maxX; row++)
        {
            for (int col = minY; col <= maxY; col++)
            {
                if(col == 0 && row == 0) continue;

                Vector2Int newPosToCheck = new Vector2Int(originPosition.x + row, originPosition.y + col);
                if(roomFloorNoCorridor.Contains(newPosToCheck) == false) return (false, positions);
                positions.Add(newPosToCheck);
            }
        }
        return (true, positions);
    } 

    public enum PlacementType
    {
        OpenSpace,
        NearWall,
        Corner
    }
}
