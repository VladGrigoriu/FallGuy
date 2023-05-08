using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class MapShuffle 
{
    public Floor[] floors;
    public List<GameObject> enemiesToPlace;
    public PropObject[] propsToPlace;
    [Range(0f, 100f)] public float chance = 100f;
    [HideInInspector] public double _weight;
}

public class Key : MonoBehaviour
{
    public TMP_Text floorDisplayText;
    private int floorNumber;

    [SerializeField]
    private MapShuffle[] mapShuffles;

    private double accumulatedFloorWeights;
    private System.Random rand = new System.Random();

    private void Start() 
    {
        floorDisplayText = GameObject.FindGameObjectWithTag("FloorDisplayText").GetComponent<TMP_Text>();
        floorNumber = PlayerPrefs.GetInt("FloorNumber");
        CalculateFloorsWeights(mapShuffles);
    }

    private void CalculateFloorsWeights(MapShuffle[] mapShuffles)
    {
        accumulatedFloorWeights = 0f;
        foreach (MapShuffle map in mapShuffles)
        {
            accumulatedFloorWeights += map.chance;
            map._weight = accumulatedFloorWeights;
        }
    }

    private int GetRandomFloorIndex(MapShuffle[] mapShuffles)
    {
        double r = rand.NextDouble() * accumulatedFloorWeights;

        for (int i = 0; i < mapShuffles.Length; i++)
            if(mapShuffles[i]._weight >= r) return i;
        return 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            
            PlayerPrefs.SetInt("FloorNumber", floorNumber+1);
            floorDisplayText.text = "Floor " + (floorNumber+1).ToString();
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            var player = playerObject.GetComponent<Player>();
            float difficulty = player.GetDifficultyScale();
            player.SetDifficultyScale(difficulty + (difficulty/3));

            var proceduralGen = GameObject.FindGameObjectWithTag("ProceduralGen");
            var proceduralGenAlgo = proceduralGen.GetComponent<RoomFirstDungeonGenerator>();
            var tilemapVisualizer = GameObject.FindGameObjectWithTag("TilemapVisualizer");
            var tilemapVisualizerTiles = tilemapVisualizer.GetComponent<TilemapVisualizer>();


            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
            GameObject[] props = GameObject.FindGameObjectsWithTag("prop");
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
            GameObject skullIcon = GameObject.FindGameObjectWithTag("SkullIcon");
            GameObject chestIcon = GameObject.FindGameObjectWithTag("ChestIcon");
            GameObject campfire = GameObject.FindGameObjectWithTag("Campfire");

            foreach(GameObject enemy in enemies)
            {
                GameObject.Destroy(enemy);
            }
            foreach(GameObject chest in chests)
            {
                GameObject.Destroy(chest);
            }
            foreach(GameObject prop in props)
            {
                GameObject.Destroy(prop);
            }
            foreach(GameObject coin in coins)
            {
                GameObject.Destroy(coin);
            }
            Destroy(skullIcon);
            Destroy(chestIcon);
            Destroy(campfire);

            MapShuffle mapToGenerate = mapShuffles[GetRandomFloorIndex(mapShuffles)];

            tilemapVisualizerTiles.Clear();
            tilemapVisualizerTiles.floors = mapToGenerate.floors;

            proceduralGenAlgo.ClearAll();
            proceduralGenAlgo.enemiesToPlace = mapToGenerate.enemiesToPlace;
            proceduralGenAlgo.propsToPlace = mapToGenerate.propsToPlace;

            proceduralGenAlgo.CreateRooms();
            Destroy(this.gameObject);
        }
        
    }
}
