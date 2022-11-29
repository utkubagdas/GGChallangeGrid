using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] private TextMeshPro xText;
    
    #region Local
    // State
    private int myRow;
    private int myCol;
    private int markedNeighborCount;
    private GridCreator gridCreator;
    public List<GridCell> myNeighborsList = new List<GridCell>();
    #endregion
    
    #region Global
    [HideInInspector] public bool isMarked;
    #endregion

    private void Start()
    {
        gridCreator = FindObjectOfType<GridCreator>();
    }
    private void OnMouseDown()
    {
        ChangeTextState(true);
        isMarked = true;
        CheckNeighbors();
    }
    public void InitializeGrid(int row, int col)
    {
        myRow = row;
        myCol = col;
    }


    private void ChangeTextState(bool isActive)
    {
        xText.enabled = isActive;
    }

    public void CheckNeighbors()
    {
        for (int i = myRow - 2; i <= myRow + 2; i++)
        {
            if (gridCreator.grid[i, myCol].isMarked && i < gridCreator.Column)
            {
                myNeighborsList.Add(gridCreator.grid[i, myCol]);
                markedNeighborCount++;
            }
        }
        
        for (int i = myCol - 2; i <= myCol + 2; i++)
        {
            if (gridCreator.grid[myRow, i].isMarked && i < gridCreator.Column)
            {
                myNeighborsList.Add(gridCreator.grid[myRow, i]);
                markedNeighborCount++;
            }
        }

        if (markedNeighborCount >= 4)
        {
            RemoveMarks();
        }
        
        Debug.Log($"neighbors Count : {markedNeighborCount}");
    }

    private void RemoveMarks()
    {
        // First remove our mark.
        isMarked = false; 
        ChangeTextState(false);

        // Remove neighbor marks.
        foreach (var cell in myNeighborsList)
        {
            cell.isMarked = false;
            cell.ChangeTextState(false);
        }
        gridCreator.IncreaseMatchCount(1);
    }
}
