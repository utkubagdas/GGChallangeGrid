using UnityEngine;
using TMPro;
using System;

public class GridCreator : MonoBehaviour
{
    #region Serialized
    [SerializeField] private GridCell gridPrefab;
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI matchCountText;
    #endregion
    
    #region Local
    private GridCell[,] gridCellArray;
    private bool[,] visitHistory;
    private bool buttonTextisChanged;
    private int matchCount;
    private GameObject gridsParent;
    #endregion
    
    #region Property
    public int Column { get; private set; }
    public int Row { get; private set; }
    #endregion
    
    #region Singleton
    private static GridCreator _instance;
    public static GridCreator Instance => _instance;
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        gridsParent = new GameObject("GridsParent");
    }

    public void CreateGrid()
    {
        if (string.IsNullOrEmpty(inputText.text) || Convert.ToInt32(inputText.text) <= 0)
            return;
        
        if (gridCellArray != null)
        {
            matchCount = 0;
            UpdateMatchCountText(0);
            ClearGridCells();
        }
        
        Column = Convert.ToInt32(inputText.text);

        gridCellArray = new GridCell[Column, Column];
        GenerateGrid();

        if (!buttonTextisChanged)
        {
            buttonText.SetText("Rebuild");
            buttonTextisChanged = true;
        }

        visitHistory = new bool[Column, Column];
        ClearVisitHistory();
    }
    public bool HasMark(int row, int col)
    {
        return gridCellArray[row, col].HasMark();
    }
    public bool IsVisited(int row, int col)
    {
        return visitHistory[row, col];
    }
    public void SetVisited(int row, int col)
    {
        visitHistory[row, col] = true;
    }
    public void ClearVisitHistory()
    {
        for(int i = 0; i < Column; i++)
        {
            for(int j = 0; j < Column; j++)
            {
                visitHistory[i, j] = false;
            }
        }
    }
    public GridCell ElementAt(int row, int col)
    {
        return gridCellArray[row, col];
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
                var gridObj = Instantiate(gridPrefab, gridsParent.transform, true);
                gridCellArray[i, j] = gridObj;
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
                if (gridCellArray[i, j] == null) continue;
                Destroy(gridCellArray[i, j].gameObject);
            }
        }
    }
}
