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


    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public MoveAction getMoveAction()
    {
        return moveAction;
    }
}
