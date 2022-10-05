using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;


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
                selectedUnit = unit;
                return true;
            }
        }
        return false;
        
    }
}
