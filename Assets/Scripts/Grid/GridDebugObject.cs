using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    private GridObject gridObject;
    [SerializeField] TextMeshPro textMeshPro;

    private void Start()
    {
        updateGridText();
    }
    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    private void updateGridText()
    {
        textMeshPro.text = gridObject.getGridObjectPosition();
    }

}
