using Assets.Scripts.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    public static GridSystemVisual Instance { get; private set; }
    [SerializeField] private Transform gridSystemVisualPrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingles;

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterials;

    public enum GridVisualType
    {
        White,
        Blue,
        RedSoft,
        Red,
        Yellow
    }

    private int gridHeight;
    private int gridWidth;

    private bool gridVisible = false;


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

        GameObject GridSystemVisualParent = GameObject.Find("GridSystemVisualParent");
        if (GridSystemVisualParent == null)
        {
            GridSystemVisualParent = new GameObject("GridSystemVisualParent");
        }
        gridHeight = LevelGrid.Instance.getHeight();
        gridWidth = LevelGrid.Instance.getWidth();
        gridSystemVisualSingles = new GridSystemVisualSingle[
            gridWidth,
            gridHeight
            ];
        //Debug.Log("Grid height --> " + LevelGrid.Instance.getHeight() + "  Grid Width --> " + LevelGrid.Instance.getWidth());
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {

                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform =
                Instantiate(gridSystemVisualPrefab, LevelGrid.Instance.getWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleTransform.SetParent(GridSystemVisualParent.transform);
                gridSystemVisualSingles[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();

            }
        }
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.onAnyUnitMovedGridPosition += LevelGrid_onAnyUnitMovedGridPosition;
        updateGridVisual();
    }

    private void LevelGrid_onAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        updateGridVisual();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        updateGridVisual();
    }

    private void Update()
    {
        //updateGridVisual();
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                gridSystemVisualSingles[x, z].Hide();
            }
        }
        gridVisible = false;
    }

    public void showGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            Vector3 gridLocation = LevelGrid.Instance.getWorldPosition(gridPosition);
            gridSystemVisualSingles[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }   
        gridVisible = true;
    }

    public bool checkIfGridIsHidden()
    {
        return gridVisible;
    }

    private void updateGridVisual()
    {
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSystem.Instance.getSelectedUnit();
        BaseAction baseAction = UnitActionSystem.Instance.getSelectedAction();

        GridVisualType gridVisualType = GridVisualType.White;
        switch (baseAction)
        {
            case MoveAction move:
                gridVisualType = GridVisualType.White;
                break;
            case ShootAction shoot:

                gridVisualType = GridVisualType.RedSoft;
                ShowGridPositionRange(selectedUnit.GetGridPosition(),
                                     shoot.getMaxShootDistance(), 
                                     gridVisualType);
                gridVisualType = GridVisualType.Red;
                break;
            case SpinAction spin:
                gridVisualType = GridVisualType.Blue;
                break;
        }
        showGridPositionList(baseAction.GetValidActionGridPosition(), gridVisualType);


    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }

        showGridPositionList(gridPositionList, gridVisualType);
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterials)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }
        Debug.Log("Couldn't find material for GridVisualMaterialType + " + gridVisualType);
        return null;
    }
}
