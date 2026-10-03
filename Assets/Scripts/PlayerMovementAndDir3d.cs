using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovementAndDir3d : MonoBehaviour
{
    private InputActionPlayer inputActions;
    private InputAction moveActionUp;
    private InputAction moveActionDown;
    private InputAction moveActionLeft;
    private InputAction moveActionRight;

    [SerializeField] private TwoDtoThreeDMaze maze;
    [SerializeField] GameObject Ellen;
    [SerializeField] GameObject Gorrister;
    [SerializeField] GameObject Benny;
    [SerializeField] GameObject Nimdok;


    private EllenScript ellenBehaviour;
    private GorristerScript gorristerBehaviour;
    private BennyScript bennyBehaviour;
    private NimdokScript nimdokBehaviour;



    private Vector3Int currentCell;

    private Vector3 targetpos;
    private Vector3Int desiredDir = Vector3Int.left; //SET THIS TO LEFT AS DEFAULT 
    public Vector3Int currentDir = Vector3Int.zero;

    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        inputActions = new InputActionPlayer();

        moveActionUp = inputActions.Movement.Up;
        moveActionDown = inputActions.Movement.Down;
        moveActionLeft = inputActions.Movement.Left;
        moveActionRight = inputActions.Movement.Right;
        targetpos = transform.position;
    }

    private void Start()
    {
        // Figure out which maze cell the Player starts in.
        currentCell = maze.WorldToCell(transform.position);

        // Make sure Player is exactly at the center of that cell.
        transform.position = maze.CellToWorld(currentCell);

        targetpos = transform.position;

        ellenBehaviour = Ellen.GetComponent<EllenScript>();
        gorristerBehaviour = Gorrister.GetComponent<GorristerScript>();
        bennyBehaviour = Benny.GetComponent<BennyScript>();
        nimdokBehaviour = Nimdok.GetComponent<NimdokScript>();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        moveActionUp.performed += OnUpPressed;
        moveActionDown.performed += OnDownPressed;
        moveActionLeft.performed += OnLeftPressed;
        moveActionRight.performed += OnRightPressed;
    }
    private void OnDisable()
    {
        moveActionUp.performed -= OnUpPressed;
        moveActionDown.performed -= OnDownPressed;
        moveActionLeft.performed -= OnLeftPressed;
        moveActionRight.performed -= OnRightPressed;
        inputActions.Disable();
    }
    private void Update()
    {
        if (transform.position == targetpos)
        {
            if (CanMove(desiredDir))
                currentDir = desiredDir;

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
        if (nextCell == new Vector3Int(15, 0, 15) ||
    nextCell == new Vector3Int(16, 0, 15))
        {
            return false;
        }
        return true;
    }


    private void OnUpPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            desiredDir = Vector3Int.forward;
        }
    }

    private void OnDownPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            desiredDir = Vector3Int.back;
        }
    }

    private void OnLeftPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            desiredDir = Vector3Int.left;
        }
    }

    private void OnRightPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            desiredDir = Vector3Int.right;
        }
    }
    private void OnMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetpos, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            ellenBehaviour.IsFrightened = true;
            gorristerBehaviour.IsFrightened = true;
            bennyBehaviour.IsFrightened = true;
            nimdokBehaviour.IsFrightened = true;
            Destroy(other.gameObject);
        }
    }

}

