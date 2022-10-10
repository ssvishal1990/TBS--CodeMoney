using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] private float characterRotateSpeed = 4f; 
    [SerializeField] private Animator unitAnimator;
    
    private Vector3 targetPosition;


    private GridPosition gridPosition;

    private MoveAction moveAction;

    private void Awake()
    {
        targetPosition = transform.position;
        moveAction = GetComponent<MoveAction>();
        //LevelGrid.Instance.SetUnitAtPosition(LevelGrid.Instance.getGridPosition(transform.position), this);
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.getGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtPosition(gridPosition, this);
    }
    private void Update()
    {
        UpdateCharacterPosition();
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Move(MouseWorld.GetPosition());
        //}
    }

    private void UpdateCharacterPosition()
    {
        unitAnimator.SetBool("isWalking", true);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (!checkIfReached())
        {
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * characterRotateSpeed);
            unitAnimator.SetBool("isWalking", true);
        }
        else
        {
            unitAnimator.SetBool("isWalking", false);
        }
        GridPosition newgridPosition = LevelGrid.Instance.getGridPosition(transform.position);
        if (gridPosition != newgridPosition)
        {
            Debug.Log("Change in grid Position detected");
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newgridPosition);
            gridPosition = newgridPosition;
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private bool checkIfReached()
    {
        int distance =  (int)Mathf.Round(Vector3.Distance(transform.position, targetPosition));
        if (distance == 0)
        {
            return true;
        }else
        {
            return false;
        }
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public MoveAction getMoveAction()
    {
        return moveAction;
    }
}
