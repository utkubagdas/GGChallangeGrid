using UnityEngine;
using TMPro;
using System;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private GridCell gridPrefab;
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI matchCountText;

    public GridCell[,] grid;
    private bool buttonTextisChanged;
    private int matchCount;
    
    public int Column { get; private set; }

    public void CreateGrid()
    {
        if (string.IsNullOrEmpty(inputText.text) || Convert.ToInt32(inputText.text) <= 0)
            return;
        
        if (grid != null)
        {
            matchCount = 0;
            UpdateMatchCountText(0);
            ClearGridCells();
        }
        
        Column = Convert.ToInt32(inputText.text);

        grid = new GridCell[Column, Column];
        GenerateGrid();

        if (!buttonTextisChanged)
        {
            buttonText.SetText("Rebuild");
            buttonTextisChanged = true;
        }
        
    }

    private void GenerateGrid()
    {
        var cam = Camera.main;
        Vector3 topLeft = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, cam.nearClipPlane));
        Vector3 bottomRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, cam.nearClipPlane));

        float panelXSize = Mathf.Abs(topLeft.x - bottomRight.x);
        float panelYSize = Mathf.Abs(topLeft.y - bottomRight.y);

        float width = panelXSize / Column;
        float height = panelYSize / Column / 1.25f;

        width = Mathf.Min(width, height);
        height = width;

        float defaultGridScaleX = gridPrefab.transform.lossyScale.x;
        float defaultGridScaleY = gridPrefab.transform.lossyScale.y;

        float defaultWidth = gridPrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.x;// / 1.3f;
        float defaultHeight = gridPrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.y;// / 1.3f;

        float scaleX = defaultGridScaleX * width / defaultWidth;
        float scaleY = defaultGridScaleY * height / defaultHeight;

        float offsetFromTopInWorld = 0.0f;
        Vector3 startPos = topLeft + new Vector3(width * 0.5f, -(offsetFromTopInWorld + (height * 0.5f)) , 5);
        Vector3 spawnPosition = startPos;

        for (int i = 0; i < Column; i++)
        {
            spawnPosition.y = startPos.y - i * height;
            for (int j = 0; j < Column; j++)
            {
                spawnPosition.x = startPos.x + j * width;
                var gridObj = Instantiate(gridPrefab);
                grid[i, j] = gridObj;
                gridObj.InitializeGrid(i, j);
                gridObj.transform.localScale = new Vector3(scaleX, scaleY, gridObj.transform.localScale.z);
                gridObj.transform.position = spawnPosition;
                
            }
        }
    }

    private void UpdateMatchCountText(int count)
    {
        matchCountText.SetText("Match Count : " + count);
    }

    public void IncreaseMatchCount(int count)
    {
        matchCount += count;
        UpdateMatchCountText(matchCount);
    }
    
    public void ClearGridCells()
    {
        for (int i = 0; i < Column; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                if (grid[i, j] == null) continue;
                Destroy(grid[i, j].gameObject);
            }
        }
    }
}
