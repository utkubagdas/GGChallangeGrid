using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GridCreator : MonoBehaviour
{
    public GameObject grid;
    private int col;
    private int row;
    public Transform spawnPos;
    float width;
    float height;
    public Transform leftUp;
    public Transform rightDown;
    float panelXSize;
    float panelYSize;
    public TMP_InputField colText;
    public TMP_InputField rowText;
    private List<GameObject> gridList = new List<GameObject>();

    public void CreateGrid()
    {
        col = Convert.ToInt32(colText.text);
        row = Convert.ToInt32(rowText.text);
        

        ClearGrid();
        GenerateGrid();
    }
    

    private void GenerateGrid()
    {
        panelXSize = Mathf.Abs(leftUp.position.x - rightDown.position.x);
        panelYSize = Mathf.Abs(leftUp.position.y - rightDown.position.y);

        float width = panelXSize / row;
        float height = panelYSize / col;

        float defaultGridScaleX = grid.transform.lossyScale.x;
        float defaultGridScaleY = grid.transform.lossyScale.y;

        float defaultWidth = grid.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        float defaultHeight = grid.GetComponentInChildren<SpriteRenderer>().bounds.size.y;

        float scaleX = defaultGridScaleX * width / defaultWidth;
        float scaleY = defaultGridScaleY * height / defaultHeight;

        
        float x = width / 2f;
        float y = height / 2f;
        Vector3 startPos = spawnPos.position + new Vector3(x, -y);
        Vector3 spawnPosition = startPos;

        for (int i = 0; i < col; i++)
        {
            spawnPosition.y = startPos.y - i * height;
            for (int k = 0; k < row; k++)
            {
                spawnPosition.x = startPos.x + k * width;
                var gridObj = Instantiate(grid);
                gridList.Add(gridObj);
                gridObj.transform.localScale = new Vector3(scaleX, scaleY, gridObj.transform.localScale.z);
                gridObj.transform.position = spawnPosition;
                
            }
        }
    }

    private void ClearGrid()
    {

        for (int i = 0; i < gridList.Count; i++)
        {
            Destroy(gridList[i]);
        }
        gridList.Clear();
    }
}
