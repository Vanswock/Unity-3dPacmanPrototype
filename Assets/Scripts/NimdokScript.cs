using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimdokScript : MonoBehaviour
{
    int frightenedTimer = 10;
    private Renderer ghostRenderer;
    private Color originalColor;
    private Color frightenedColor=Color.blue;
    private Color eatenColor = Color.white;
    [SerializeField] private TwoDtoThreeDMaze maze;
    [SerializeField] private ChaseToScatterGlobalTimer chaseToScatterGlobalTimer;

    private Vector3Int currentCell;
    private Vector3 targetpos;
    private Vector3Int choosingdir;
    private Vector3Int currentDir = Vector3Int.zero;
    public GameObject player;


    [SerializeField] private float moveSpeed = 5f;
    int calculatedDistanceUp;
    int calculatedDistanceDown;
    int calculatedDistanceLeft;
    int calculatedDistanceRight;

    public bool IsFrightened = false;
    private bool IsInCage = false;
    private bool IsCaught = false;

    private void Awake()
    {
        targetpos = transform.position;
    }

    private void Start()
    {
        ghostRenderer = GetComponent<Renderer>();
        originalColor = ghostRenderer.material.color;
        StartCoroutine(GhostModeLoop());
        // Figure out which maze cell the Player starts in.
        currentCell = maze.WorldToCell(transform.position);

        // Make sure Player is exactly at the center of that cell.
        transform.position = maze.CellToWorld(currentCell);

        targetpos = transform.position;
        

    }


    private void Update()
    {

        if (transform.position == targetpos)
        {
            ghostRenderer.material.color = originalColor;
            IsInCage = currentCell == maze.WorldToCell(new Vector3(16.5f, 0, 14.5f));

            calculatedDistanceUp = int.MaxValue;
            calculatedDistanceDown = int.MaxValue;
            calculatedDistanceLeft = int.MaxValue;
            calculatedDistanceRight = int.MaxValue;
            if (IsInCage)
            {
                IsCaught = false;
            }
            if (IsCaught)
            {   ghostRenderer.material.color = eatenColor;
                choosingdir = ChooseBacktoCagedir();
                moveSpeed = 7f;
                
            }

            else if (IsFrightened)
            {
                ghostRenderer.material.color = frightenedColor;
                choosingdir = ChooseFrighteneddir();
                moveSpeed = 3f;

            }
            else if (chaseToScatterGlobalTimer.mode == "Chase")
            {  
                choosingdir = ChooseChasedir();
                moveSpeed = 5f;
            }
            else if (chaseToScatterGlobalTimer.mode == "Scatter")
            {
                choosingdir = ChooseScatterdir();
                moveSpeed = 5f;
            }
            if (CanMove(choosingdir))
                currentDir = choosingdir;


            if (CanMove(currentDir))
            {
                currentCell += currentDir;

                targetpos = maze.CellToWorld(currentCell);
            }
            else
                currentDir = Vector3Int.zero;
        }
        else
        {
            OnMovement();
        }

    }
    private bool CanMove(Vector3Int direction)
    {
        Vector3Int nextCell = currentCell + direction;

        if (!maze.IsWalkable(nextCell))
            return false;
        if ((!IsInCage && !IsCaught) && (nextCell == new Vector3Int(15, 0, 15) ||
           nextCell == new Vector3Int(16, 0, 15)))
        {
            return false;
        }
        return true;
    }



    private Vector3Int ChooseChasedir()
    {
        Vector3Int targetCell = maze.WorldToCell(player.transform.position); 
      
        if (CanMove(Vector3Int.forward))
        {
            calculatedDistanceUp = (targetCell - (currentCell + Vector3Int.forward)).sqrMagnitude;
        }

        if (CanMove(Vector3Int.back))
        {
            calculatedDistanceDown = (targetCell - (currentCell + Vector3Int.back)).sqrMagnitude;

        }
        if (CanMove(Vector3Int.left))
        {
            calculatedDistanceLeft = (targetCell - (currentCell + Vector3Int.left)).sqrMagnitude;
        }
        if (CanMove(Vector3Int.right))
        {
            calculatedDistanceRight = (targetCell - (currentCell + Vector3Int.right)).sqrMagnitude;

        }

        int[] distances = { calculatedDistanceUp, calculatedDistanceLeft, calculatedDistanceDown, calculatedDistanceRight };

        Vector3Int[] directions = { Vector3Int.forward, Vector3Int.left, Vector3Int.back, Vector3Int.right };

        int smallestDistance = int.MaxValue;
        Vector3Int chosenDirection = Vector3Int.zero;

        for (int i = 0; i < distances.Length; i++)
        {
            if (directions[i] == -currentDir)
            {
                continue;
            }

            if (distances[i] < smallestDistance)
            {
                smallestDistance = distances[i];
                chosenDirection = directions[i];
            }
        }
        if ((transform.position - player.transform.position).sqrMagnitude <= 64f)
        {
            chosenDirection = ChooseScatterdir();
        }
        return chosenDirection;
    }

    private Vector3Int ChooseScatterdir()
    {
        Vector3Int targetCell = maze.WorldToCell(new Vector3(2.5f, 0f, 3.5f)); //name will be changed from playercell
        if (CanMove(Vector3Int.forward))
        {
            calculatedDistanceUp = (targetCell - (currentCell + Vector3Int.forward)).sqrMagnitude;
        }

        if (CanMove(Vector3Int.back))
        {
            calculatedDistanceDown = (targetCell - (currentCell + Vector3Int.back)).sqrMagnitude;

        }
        if (CanMove(Vector3Int.left))
        {
            calculatedDistanceLeft = (targetCell - (currentCell + Vector3Int.left)).sqrMagnitude;
        }
        if (CanMove(Vector3Int.right))
        {
            calculatedDistanceRight = (targetCell - (currentCell + Vector3Int.right)).sqrMagnitude;

        }

        int[] distances = { calculatedDistanceUp, calculatedDistanceLeft, calculatedDistanceDown, calculatedDistanceRight };

        Vector3Int[] directions = { Vector3Int.forward, Vector3Int.left, Vector3Int.back, Vector3Int.right };

        int smallestDistance = int.MaxValue;
        Vector3Int chosenDirection = Vector3Int.zero;

        for (int i = 0; i < distances.Length; i++)
        {
            if (directions[i] == -currentDir)
            {
                continue;
            }

            if (distances[i] < smallestDistance)
            {
                smallestDistance = distances[i];
                chosenDirection = directions[i];
            }
        }
        return chosenDirection;

    }

    private Vector3Int ChooseBacktoCagedir()
    {
        Vector3Int targetCell = maze.WorldToCell(new Vector3(16.5f, 0, 14.5f)); //name will be changed from playercell
        if (CanMove(Vector3Int.forward))
        {
            calculatedDistanceUp = (targetCell - (currentCell + Vector3Int.forward)).sqrMagnitude;
        }

        if (CanMove(Vector3Int.back))
        {
            calculatedDistanceDown = (targetCell - (currentCell + Vector3Int.back)).sqrMagnitude;

        }
        if (CanMove(Vector3Int.left))
        {
            calculatedDistanceLeft = (targetCell - (currentCell + Vector3Int.left)).sqrMagnitude;
        }
        if (CanMove(Vector3Int.right))
        {
            calculatedDistanceRight = (targetCell - (currentCell + Vector3Int.right)).sqrMagnitude;

        }

        int[] distances = { calculatedDistanceUp, calculatedDistanceLeft, calculatedDistanceDown, calculatedDistanceRight };

        Vector3Int[] directions = { Vector3Int.forward, Vector3Int.left, Vector3Int.back, Vector3Int.right };

        int smallestDistance = int.MaxValue;
        Vector3Int chosenDirection = Vector3Int.zero;

        for (int i = 0; i < distances.Length; i++)
        {
            if (directions[i] == -currentDir)
            {
                continue;
            }

            if (distances[i] < smallestDistance)
            {
                smallestDistance = distances[i];
                chosenDirection = directions[i];
            }
        }
        return chosenDirection;

    }

    private Vector3Int ChooseFrighteneddir()
    {
        Vector3Int[] directions = { Vector3Int.forward, Vector3Int.left, Vector3Int.back, Vector3Int.right };
        Vector3Int chosenDirection = Vector3Int.zero;
        List<Vector3Int> validDirections = new List<Vector3Int>();

        for (int i = 0; i < directions.Length; i++)
        {
            if (directions[i] == -currentDir)
            {
                continue;
            }
            if (CanMove(directions[i]))
            {
                validDirections.Add(directions[i]);
            }
        }

        chosenDirection = validDirections[Random.Range(0, validDirections.Count)];


        return chosenDirection;
    }
    private void OnMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetpos, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsFrightened)
        {
            IsCaught = true;
            IsFrightened = false;
        }
    }
    IEnumerator GhostModeLoop()
    {
        while (true)
        {
            if (IsFrightened)
            {
                yield return new WaitForSeconds(frightenedTimer);
                IsFrightened = false;
            }

            yield return null;
        }
    }
}