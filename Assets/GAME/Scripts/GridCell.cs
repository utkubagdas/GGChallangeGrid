using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    #region Serialized
    [SerializeField] private TextMeshPro xText;
    #endregion
    
    #region Local
    private int myRow;
    private int myCol;
    private bool isMarked;
    private GridCreator grid;
    private int requiredNeighborCount = 2;
    #endregion
    
    private void Start()
    {
        grid = GridCreator.Instance;
    }
    private void OnMouseDown()
    {
        if (isMarked) return;
        
        ChangeTextState(true);
        isMarked = true;
        grid.ClearVisitHistory();

        var neighbors = GetNeighbors();

        // once they terminate, check win condition

        if (neighbors.Count >= requiredNeighborCount)
        {
            RemoveMarks(neighbors);
        }
    }
    public bool HasMark()
    {
        return isMarked;
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

    private List<GridCell> GetNeighbors()
    {
        List<GridCell> unvisitedNeighbors = new List<GridCell>();

        int lastCol = grid.Column;
        int lastRow = grid.Column;

        // check right cells
        if (myCol+1 < lastCol && grid.HasMark(myRow, myCol+1) && !grid.IsVisited(myRow, myCol + 1))
        {
            unvisitedNeighbors.Add(grid.ElementAt(myRow, myCol + 1));
        }

        //Debug.Log($"Checking for: row,col <{myRow}, {myCol}>");
        string debug = $"Checking for: row,col <{myRow}, {myCol}> . . . ";
        debug += $"Right: {unvisitedNeighbors.Count}, ";

        int testCount = unvisitedNeighbors.Count;

        // check left cells
        if (myCol-1 >= 0 && grid.HasMark(myRow, myCol-1) && !grid.IsVisited(myRow, myCol-1))
        {
            unvisitedNeighbors.Add(grid.ElementAt(myRow, myCol - 1));
        }
        debug += $"Left:{unvisitedNeighbors.Count - testCount}, ";
        testCount = unvisitedNeighbors.Count;

        // check below cells
        if (myRow+1 < lastRow && grid.HasMark(myRow + 1, myCol) && !grid.IsVisited(myRow + 1, myCol))
        {
            unvisitedNeighbors.Add(grid.ElementAt(myRow + 1, myCol));
        }
        debug += $"Below:{unvisitedNeighbors.Count - testCount}, ";
        testCount = unvisitedNeighbors.Count;

        // check above cells
        if (myRow-1 >= 0 && grid.HasMark(myRow - 1, myCol) && !grid.IsVisited(myRow - 1, myCol))
        {
            unvisitedNeighbors.Add(grid.ElementAt(myRow - 1, myCol));
        }

        debug += $"Above:{unvisitedNeighbors.Count - testCount} neighbours.";
        Debug.Log(debug);

        // Set visited.
        grid.SetVisited(myRow, myCol);

        // Recursive calls.

        List<GridCell> allNeighbors = new List<GridCell>(unvisitedNeighbors);

        foreach (var nextNeighbor in unvisitedNeighbors)
        {
            var theirNeighbors = nextNeighbor.GetNeighbors();
            allNeighbors.AddRange(theirNeighbors);
        }
        
        return allNeighbors;
    }

    private void RemoveMarks(List<GridCell> neighbors)
    {
        // First remove our mark.
        isMarked = false; 
        ChangeTextState(false);

        // Remove neighbor marks.
        foreach (var cell in neighbors)
        {
            cell.isMarked = false;
            cell.ChangeTextState(false);
        }
        grid.IncreaseMatchCount(1);
    }
}
