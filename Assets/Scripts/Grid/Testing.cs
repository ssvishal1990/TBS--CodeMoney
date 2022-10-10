using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;



    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            unit.getMoveAction().GetValidActionGridPosition();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (GridSystemVisual.Instance.checkIfGridIsHidden())
            {
                GridSystemVisual.Instance.HideAllGridPosition();
            }else
            {
                GridSystemVisual.Instance.showGridPositionList(unit.getMoveAction().GetValidActionGridPosition());
            }
        }
    }

}
