using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Actions;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }


    public event EventHandler OnSelectedUnitChanged;


    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private MoveAction moveAction;


    private BaseAction selectedAction;

    private bool isBusy;

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
        MoveIfValidGridPosition();
        SpinUnit();
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }

    private void MoveIfValidGridPosition()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (handleUnitSelection())
            {
                return;
            }

            GridPosition mouseGridPosition = LevelGrid.Instance.getGridPosition(MouseWorld.GetPosition());
            if (selectedUnit.getMoveAction().isValidActionGridPosition(mouseGridPosition))
            {
                setBusy();
                selectedUnit.getMoveAction().Move(mouseGridPosition, clearBusy);
            }
        }
    }

    //Signature for the delegate needs to match the called method signature
    private void SpinUnit()
    {
        if (Input.GetMouseButtonDown(1))
        {
            setBusy();
            selectedUnit.getSpinAction().StartSpinning(clearBusy);
        }
    }

    private bool handleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool unitHit = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask);
        if (unitHit)
        {
            //return raycastHit.collider.gameObject.GetComponent<Unit>();

            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                setSelectedUnit(unit);
                return true;
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
    }

    public Unit getSelectedUnit()
    {
        return selectedUnit;
    }

    private void setBusy()
    {
        isBusy = true;
    }

    private void clearBusy()
    {
        isBusy = false;
    }
}
