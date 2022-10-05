using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }


    public event EventHandler OnSelectedUnitChanged;


    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

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
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if (handleUnitSelection())
            {
                return;
            }
            selectedUnit.Move(MouseWorld.GetPosition());
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

        // Below code line  and if checks are doing the same thing
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        //if (OnSelectedUnitChanged != null)
        //{
        //    OnSelectedUnitChanged(this, EventArgs.Empty);
        //}
    }

    public Unit getSelectedUnit()
    {
        return selectedUnit;
    }
}
