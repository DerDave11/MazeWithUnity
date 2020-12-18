using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5.0f;

    private Vector2 direction = Vector2.zero;
    private Vector2 nextDirection;

    private Node currentNode, previousNode, targetNode;

    // Start is called before the first frame update
    void Start()
    {
        Node node = GetNodeAtPosition(transform.localPosition);

        if (node != null)
        {
            currentNode = node;
            Debug.Log(currentNode);
        }

        direction = Vector2.left;
        ChangePosition(direction);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        Move();

        UpdateOrientation();

        ConsumePellet();
    }

    // Check if the user presses a key 
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePosition(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangePosition(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(Vector2.down);
        }
    }

    // Change position to where we want to move
    void ChangePosition (Vector2 d)
    {
        if (d != direction)
            nextDirection = d;

        if(currentNode != null)
        {
            Node moveToNode = CanMove(d);

            if(moveToNode != null)
            {
                direction = d;
                targetNode = moveToNode;
                previousNode = currentNode;
                currentNode = null;
            }
        }
    }

    // Make the movement on the player happen
    private void Move()
    {
        if (targetNode != currentNode && targetNode != null)
        {
            if(OverShotTarget())
            {
                currentNode = targetNode;
                transform.localPosition = currentNode.transform.position;

                Node moveToNode = CanMove(nextDirection);

                if(moveToNode != null)
                {
                    direction = nextDirection;
                }

                if(moveToNode == null)
                {
                    moveToNode = CanMove(direction);
                }

                if(moveToNode != null)
                {
                    targetNode = moveToNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
                else
                {
                    direction = Vector2.zero;
                }
            }
            else
            {
                transform.localPosition += (Vector3)(direction * speed) * Time.deltaTime;
            }
        }
    }

    // Move player to one of the neighbor nodes
    void MoveToNode(Vector2 d)
    {
        Node moveToNode = CanMove(d);

        if (moveToNode != null)
        {
            transform.localPosition = moveToNode.transform.position;
            currentNode = moveToNode;
        }
    }

    // Update the player orientation based on the direction in which the player will move
    void UpdateOrientation()
    {
        if (direction == Vector2.left)
        {
            // transform.localScale = new Vector3(-1, -1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2.right)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction == Vector2.up)
        {
            // transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else if (direction == Vector2.down)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
    }

    void ConsumePellet()
    {
        GameObject o = GetTileAtPosition(transform.position);

        if(o != null)
        {
            Tile tile = o.GetComponent<Tile>();
            if (tile != null)
            {
                if (!tile.didConsume && tile.isPellet)
                {
                    o.GetComponent<SpriteRenderer>().enabled = false;
                    tile.didConsume = true;
                }
            }
        }
    }

    /* Can PacMan move?
     * Is there a valid direction from the current position of PacMan?  
    */
    Node CanMove(Vector2 d)
    {
        Node moveToNode = null;

        // iterating over all neigbors of the current node
        for (int i = 0; i < currentNode.neighbors.Length; i++)
        {
            // Check the valid directions of every node,
            // set the moveToNode to the neigbor node in the direction we plan to move
            if (currentNode.validDirections[i] == d)
            {
                moveToNode = currentNode.neighbors[i];
                break;
            }
        }
        return moveToNode;
    }

    GameObject GetTileAtPosition (Vector2 pos)
    {
        int tileX = Mathf.RoundToInt(pos.x);
        int tileY = Mathf.RoundToInt(pos.y);

        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[tileX, tileY];

        if(tile != null)
            return tile;

        return null;
    }

    // Find the Node at players position 
    Node GetNodeAtPosition(Vector2 pos)
    {
        GameObject tile = GameObject.Find("Game").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];

        if (tile != null)
        {
            return tile.GetComponent<Node>();
        }
        return null;
    }

    // When this method returns true, we run over the target node
    bool OverShotTarget()
    {
        float nodeToTarget = LengthFromNode(targetNode.transform.position);
        float nodeToSelf = LengthFromNode(transform.localPosition);

        return nodeToSelf > nodeToTarget;
    }

    // 
    float LengthFromNode (Vector2 targetPosition)
    {
        Vector2 vec = targetPosition - (Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }
}
