using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
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
            foreach(GameObject enemy in enemies)
            {
                GameObject.Destroy(enemy);
            }
            foreach(GameObject chest in chests)
            {
                GameObject.Destroy(chest);
            }
            // foreach(GameObject prop in props)
            // {
            //     GameObject.Destroy(prop);
            // }
            foreach(GameObject coin in coins)
            {
                GameObject.Destroy(coin);
            }


            tilemapVisualizerTiles.Clear();
            proceduralGenAlgo.ClearAll();
            proceduralGenAlgo.CreateRooms();
            Destroy(this.gameObject);
        }
        
    }
}
