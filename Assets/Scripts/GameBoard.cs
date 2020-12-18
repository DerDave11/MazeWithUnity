/* B-GOPA01-XX1-N01 - Einsendeaufgabe Grundalgen der objektorientierten Programmierung
 * Aufgabe 4
 * Wilhelm Buechner Hochschule Darmstadt
 * David Goeppner, 906504
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The GameBoard is tracking all GameObjects with their position in an array 
 * In the Start method we are iterate over all GameObjects and store them in the Array objects
 * The Player object will not be stored in the array because it would override one other Gameobject
 */

public class GameBoard : MonoBehaviour
{
    private static readonly int boardWidth = 45;
    private static readonly int boardHeight = 40;

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];  

    // Start is called before the first frame update
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            Vector2 pos = o.transform.position;

            if (o.name != "Player")
            {
                board[(int)pos.x, (int)pos.y] = o;
            }
            else
            {
                Debug.Log("Found Player at: " + pos);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
