using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Actions;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }


    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> onBusyStatusChange;
    public event EventHandler onActionStarted;


    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private MoveAction moveAction;


    [SerializeField] private BaseAction selectedAction;

    [SerializeField] private bool isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 unit action system  " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        setSelectedUnit(selectedUnit);
    }
    void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        //MoveIfValidGridPosition();
        //SpinUnit();
        if (handleUnitSelection())
        {
            return;
        }
        
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.getGridPosition(MouseWorld.GetPosition());
            if (!selectedAction.isValidActionGridPosition(mouseGridPosition))
            {
                return;
            }
            if (!selectedUnit.TrySpendingActionPointToPerformAction(selectedAction))
            {
                return;
            }
            setBusy();
            onActionStarted?.Invoke(this, EventArgs.Empty);
            selectedAction.TakeAction(mouseGridPosition, clearBusy);
        }
    }

    private void MoveIfValidGridPosition(MoveAction moveAction)
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (handleUnitSelection())
            {
                return;
            }

            GridPosition mouseGridPosition = LevelGrid.Instance.getGridPosition(MouseWorld.GetPosition());
            if (moveAction.isValidActionGridPosition(mouseGridPosition))
            {
                setBusy();
                moveAction.TakeAction(mouseGridPosition, clearBusy);
            }
        }
    }

    //Signature for the delegate needs to match the called method signature
    private void SpinUnit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            setBusy();
            selectedUnit.getSpinAction().TakeAction(selectedUnit.GetGridPosition(), clearBusy);
        }
    }

    private bool handleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool unitHit = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask);
            if (unitHit)
            {
                //return raycastHit.collider.gameObject.GetComponent<Unit>();

                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        //This unit is already selected
                        return false;
                    }

                    if (unit.IsEnemy())
                    {
                        return false;
                    }
                    setSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
        
    }

    private void setSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        setSelectedAction(unit.getMoveAction());
        // Below code line  and if checks are doing the same thing
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        //if (OnSelectedUnitChanged != null)
        //{
        //    OnSelectedUnitChanged(this, EventArgs.Empty);
        //}
    }

    public void setSelectedAction(BaseAction action)
    {
        selectedAction = action;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }


    public Unit getSelectedUnit()
    {
        return selectedUnit;
    }

    public bool getBusyState()
    {
        return isBusy;
    }

    public BaseAction getSelectedAction()
    {
        return selectedAction;
    }

    private void setBusy()
    {   
        isBusy = true;
        onBusyStatusChange?.Invoke(this, isBusy);
    }

    private void clearBusy()
    {
        isBusy = false;
        onBusyStatusChange?.Invoke(this, isBusy);
    }
}
