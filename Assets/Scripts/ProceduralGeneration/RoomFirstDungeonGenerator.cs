using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class RoomFirstDungeonGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;

    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;

    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;

    [SerializeField]
    private bool randomWalkRooms = false;

    [SerializeField]
    public GameObject player;

    [SerializeField]
    public List<GameObject> propsToPlace;

    [SerializeField]
    public List<GameObject> enemiesToPlace;

    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary = new Dictionary <Vector2Int, HashSet<Vector2Int>>();
    private HashSet<Vector2Int> floorPositions, corridorPositions;

    private List<Color> roomColors = new List<Color>();

    [SerializeField]
    private bool showRoomGizmo = false, showCorridorsGizmo;


    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    // Start is called before the first frame update
    void Start()
    {
        RunProceduralGeneration();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        HashSet<Vector2Int> floorToFill = new HashSet<Vector2Int>();
        floor = CreateSimpleRooms(roomsList);
        floorToFill = floor;

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }


        System.Random rand = new System.Random();
        
        var spawnRoom = roomsDictionary.First();
        var bossRoom = roomsDictionary.Last();
        var treasureRoom = roomsDictionary.ElementAt(rand.Next(1, roomsDictionary.Count-1));
        var playerSpawnRoom = SpawnPlayer(spawnRoom.Key);
        

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        HashSet<Vector2Int> newCorridors = IncreaseCorridorSize(corridors);
        floor.UnionWith(newCorridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        foreach(var key in roomsDictionary.Keys)
        {
            if(key == spawnRoom.Key || key == bossRoom.Key) {

                foreach (var value in roomsDictionary[key])
                {
                    floorToFill.Remove(value);
                }
            }
        }
        
        foreach (var corridor in newCorridors)
        {
            floorToFill.Remove(corridor);
        }

        ItemPlacementHelper placementHelper = new ItemPlacementHelper();
        placementHelper.ItemPlacementHelperMethod(floor, floorToFill, propsToPlace, spawnRoom.Value, roomsList, bossRoom.Value, treasureRoom.Value, enemiesToPlace);
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        foreach (var room in roomsList)
        {
            HashSet<Vector2Int> singleFloor = new HashSet<Vector2Int>();
            Vector2Int roomPosition = (Vector2Int)Vector3Int.RoundToInt(room.center);

            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position); 
                    singleFloor.Add(position);
                }
            }
            roomsDictionary[roomPosition] = singleFloor;
            roomColors.Add(UnityEngine.Random.ColorHSV());
            // singleFloor.Clear();
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];

        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        corridorPositions = new HashSet<Vector2Int>(corridors);
        return corridors;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);
        while(position.y != destination.y)
        {
            if(destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if(destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }

        while (position.x != destination.x)
        {
            if(destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if(destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }

        return corridor;

    }

    private Vector2Int SpawnPlayer(Vector2Int roomPosition)
    {
        player.transform.position = new Vector3(roomPosition.x, roomPosition.y, 0);
        // Instantiate(player, new Vector3(roomCenter.x, roomCenter.y, 0), Quaternion.identity);
        // Instantiate(canvas, new Vector3(roomCenter.x, roomCenter.y, 0), Quaternion.identity);
        return roomPosition;
    }

    private HashSet<Vector2Int> IncreaseCorridorSize(HashSet<Vector2Int> corridors){

        HashSet<Vector2Int> newCorridors = new HashSet<Vector2Int>();
        foreach (var corridor in corridors)
        {
            for (int x = -1; x < 3; x++)
            {
                for (int y = -1; y < 3; y++)
                {
                    newCorridors.Add(corridor + new Vector2Int(x, y));
                }
            }
        }
        return newCorridors;

    }

}
