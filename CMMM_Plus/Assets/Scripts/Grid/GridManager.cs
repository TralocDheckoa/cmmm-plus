using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System;
using static load.LoadString;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    public static string loadString = "";

    public static int currentLevel;
    public static int enemyCount;
    public static int initialEnemyCount;

    public static bool hasSaved = true;

    public static float MSPT;

    public GameObject[] cellPrefabs;

    public static float animationLength = .2f;
    float timeSinceLastUpdate;

    public static bool playSimulation = false;
    public static bool stepSimulation = false;
    public static bool subTick        = false;
    public static bool clean          = true;

    public static Tool_e tool = Tool_e.DRAG;
    public static Mode_e mode = Mode_e.EDITOR;

    //Used for sub ticking
    int cellUpdateIndex = 0;
    int rotationUpdateIndex = 0;

    //Used for determining if the user can place in a tile.
    public Tilemap tilemap;
    public Tile placebleTile;
    public Tile backgroundTile;

    public GameObject nextButton;

    public GameObject tutorialText;

    public GameObject stepCountGO;

    private int stepCount = 0;
    private void setStepCount(int value)
    {
        stepCountGO.GetComponent<Text>().text = value.ToString();
        stepCount = value;
    }

    private void InitBackgroundTiles() {
        tilemap.ClearAllTiles();
        for (int y = 0; y < CellFunctions.gridHeight; y++) {
            for (int x = 0; x < CellFunctions.gridWidth; x++) {
                tilemap.SetTile(new Vector3Int(x, y, 0), backgroundTile);
            }
        }
    }

    private void Awake()
    {
        playSimulation = false;
        stepSimulation = false;
        subTick = false;
        clean = true;
        enemyCount = 0;

        instance = this;

        if (TextureLoader.textures.ContainsKey("BGDefault"))
        {
            backgroundTile.sprite = TextureLoader.textures["BGDefault"];
        }
        else
        {
            print("No key found: " + "BGDefault");
        }

        if (TextureLoader.textures.ContainsKey("0"))
        {
            placebleTile.sprite = TextureLoader.textures["0"];
        } else
        {
            print("No key found: " + "0");
        }

        CellFunctions.cellList = new LinkedList<Cell>();
        CellFunctions.generatedCellList = new LinkedList<Cell>();

        int trackedId = 0;
        int tickedId = 0;

        foreach (CellType_e cellType in CellFunctions.cellUpdateOrder) {
            if (CellFunctions.cellUpdateTypeDictionary[cellType].Equals(CellUpdateType_e.TRACKED))
            {
                CellFunctions.steppedCellIdDictionary[cellType] = trackedId;
                trackedId++;
            } else {
                CellFunctions.steppedCellIdDictionary[cellType] = tickedId;
                tickedId++;
            }
        }

        //GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>().text = "";

    }

    private void InitSteppedCellQueues()
    { 
        int trackedCellClassCount = 0;
        int totalSteppedCellClassCount = 0;

        foreach (CellType_e cellType in CellFunctions.cellUpdateOrder)
        {
            if (CellFunctions.cellUpdateTypeDictionary[cellType].Equals(CellUpdateType_e.TRACKED))
            {
                trackedCellClassCount++;
            }
            totalSteppedCellClassCount++;
        }

        CellFunctions.tickedCellList = new LinkedList<Cell>[totalSteppedCellClassCount - trackedCellClassCount];
        for (int i = 0; i < CellFunctions.tickedCellList.Length; i++)
        {
            CellFunctions.tickedCellList[i] = new LinkedList<Cell>();
        }

        CellFunctions.trackedCellRotationUpdateQueue = new LinkedList<Cell>[trackedCellClassCount];
        CellFunctions.trackedCellPositionUpdateQueue = new LinkedList<Cell>[trackedCellClassCount, 4];
        for (int steppedCellId = 0; steppedCellId < trackedCellClassCount; steppedCellId++) {
            CellFunctions.trackedCellRotationUpdateQueue[steppedCellId] = new LinkedList<Cell>();
            for (int dir = 0; dir < 4; dir++) {
                CellFunctions.trackedCellPositionUpdateQueue[steppedCellId, dir] = new LinkedList<Cell>();
            }
        }

        CellFunctions.trackedCells = new LinkedList<Cell>[totalSteppedCellClassCount,4][];
        for (int steppedCellId = 0; steppedCellId < trackedCellClassCount; steppedCellId++)
        {
            for (int dir = 0; dir < 4; dir++)
            {
                int maxDistince = dir % 2 == 0 ? CellFunctions.gridWidth : CellFunctions.gridHeight;
                CellFunctions.trackedCells[steppedCellId, dir] = new LinkedList<Cell>[maxDistince];
                for (int distance = 0; distance < maxDistince; distance++)
                {
                    CellFunctions.trackedCells[steppedCellId, dir][distance] = new LinkedList<Cell>();  
                }
            }
        }
    }

    private void Start()
    {
        if (loadString == "")
            InitGridSize();
        else
            Load(loadString);

        initialEnemyCount = enemyCount;
        tool = Tool_e.DRAG;
    }

    public Cell SpawnCell(CellType_e cellType, Vector2 position, Direction_e rotation, bool generated) {
        Cell cell = Instantiate(this.cellPrefabs[(int)cellType]).GetComponent<Cell>();
        cell.transform.position = new Vector3(position.x, position.y, 0);
        cell.Setup(position, rotation, generated);
        cell.oldPosition = position;
        cell.oldRotation = (int)rotation;
        cell.transform.rotation = Quaternion.Euler(0, 0, -90 * (int)rotation);
        cell.name = CellFunctions.cellList.Count + "";

        return cell;
    }

    public void InitGridSize()
    {
        //does everything that is dependent of the size of the grid
        CellFunctions.cellGrid = new Cell[CellFunctions.gridWidth, CellFunctions.gridHeight];
        InitSteppedCellQueues();
        InitBackgroundTiles();
        Camera.main.GetComponent<CameraPan>().PositionCamera();
    }

    private void UpdateTicked(CellType_e cellType) {
        foreach (TickedCell cell in CellFunctions.tickedCellList[CellFunctions.GetSteppedCellId(cellType)]) {
            if (cell.suppresed)
            {
                cell.suppresed = false;
                continue;
            }
            cell.Step();
        }
    }

    private void UpdateTracked(CellType_e cellType, Direction_e dir) {
        
        int maxDistance;
        if ((int)dir % 2 == 0)
        {
            maxDistance = CellFunctions.gridWidth;
        }
        else {
            maxDistance = CellFunctions.gridHeight;
        }
        for (int distance = 0; distance < maxDistance; distance++) {

            LinkedListNode<Cell> selectedNode = CellFunctions.trackedCells[CellFunctions.GetSteppedCellId(cellType), (int)dir][distance].First;
            TrackedCell cell;
            while (selectedNode != null)
            {
                cell = (TrackedCell)selectedNode.Value;
                selectedNode = selectedNode.Next;

                if (cell.suppresed)
                {
                    cell.suppresed = false;
                }
                else
                {
                    cell.Step();
                }
            }
        }
    }

    private void MetaSortTrackedCells(CellType_e cellType)
    {
        int steppedCellId = CellFunctions.GetSteppedCellId(cellType);

        LinkedListNode<Cell> selectedNode = CellFunctions.trackedCellRotationUpdateQueue[steppedCellId].First;
        TrackedCell cell;
        while (selectedNode != null)
        {
            cell = (TrackedCell)selectedNode.Value;
            selectedNode = selectedNode.Next;

            cell.queuedForRotationSorting = false;
            cell.trackedCellNode.List.Remove(cell.trackedCellNode);
            CellFunctions.trackedCellPositionUpdateQueue[steppedCellId, (int)cell.getDirection()].AddLast(cell.trackedCellNode);
        }
    }

    private void SortTrackedCells(CellType_e cellType, Direction_e dir) {
        int steppedCellId = CellFunctions.GetSteppedCellId(cellType);

        LinkedListNode<Cell> selectedNode = CellFunctions.trackedCellPositionUpdateQueue[steppedCellId, (int)dir].First;
        TrackedCell cell;
        while (selectedNode != null)
        {
            cell = (TrackedCell)selectedNode.Value;
            selectedNode = selectedNode.Next;

            cell.queuedForPositionSorting = false;

            cell.trackedCellNode.List.Remove(cell.trackedCellNode);
            CellFunctions.trackedCells[steppedCellId, (int)dir][cell.GetDistanceFromFacingEdge()].AddLast(cell.trackedCellNode);
        }
    }

    public void Reset()
    {
        playSimulation = false;
        stepSimulation = false;
        clean = true;
        cellUpdateIndex = 0;
        rotationUpdateIndex = 0;

        setStepCount(0);
    }
    
    private void printGrid()
    {
        string printVal = "";
        for (int a = CellFunctions.gridHeight - 1; a >= 0 ; a--)
        {
            for (int b = 0; b < CellFunctions.gridWidth; b++)
            {
                if (CellFunctions.cellGrid[b, a] == null)
                {
                    printVal += "-";
                }
                else
                {
                    printVal += (int)CellFunctions.cellGrid[b, a].cellType;
                }
            }
            printVal += "\n";
        }
        print(printVal);
    }


    private void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        //Animate every cells rotation and transformation from last rotation and last position.

        foreach (Cell cell in CellFunctions.cellList) {
            if (cell.animate)
            {
                cell.transform.position = Vector3.Lerp(
                new Vector3(cell.oldPosition.x, cell.oldPosition.y, 0),
                new Vector3(cell.position.x, cell.position.y, 0),
                timeSinceLastUpdate / animationLength
                );
                cell.transform.rotation = Quaternion.Lerp(
                    Quaternion.Euler(0, 0, cell.oldRotation * -90),
                    Quaternion.Euler(0, 0, cell.rotation * -90),
                    timeSinceLastUpdate / animationLength
                    );
            }
        }
        if (timeSinceLastUpdate > animationLength && (playSimulation || stepSimulation)) {

            setStepCount(stepCount + 1);

            timeSinceLastUpdate = 0;
            stepSimulation = false;
            clean = false;
            MSPT = System.DateTime.Now.Ticks;
            foreach (Cell cell in CellFunctions.cellList)
            {
                cell.oldPosition = cell.position;
                cell.oldRotation = cell.rotation;
            }
            if (subTick)
            {
                if (CellFunctions.GetUpdateType((CellType_e)cellUpdateIndex).Equals(CellUpdateType_e.TRACKED))
                {
                    if (rotationUpdateIndex == 0)
                    {
                        MetaSortTrackedCells((CellType_e)cellUpdateIndex);
                    }
                    SortTrackedCells((CellType_e)cellUpdateIndex, CellFunctions.directionUpdateOrder[rotationUpdateIndex]);
                    UpdateTracked((CellType_e)cellUpdateIndex, CellFunctions.directionUpdateOrder[rotationUpdateIndex]);
                    rotationUpdateIndex++;
                    if (rotationUpdateIndex > 3) {
                        cellUpdateIndex++;
                        rotationUpdateIndex = 0;
                    }
                }
                else {
                    UpdateTicked((CellType_e)cellUpdateIndex);
                    cellUpdateIndex++;
                }
                if (cellUpdateIndex >= CellFunctions.cellUpdateOrder.Length) {
                    cellUpdateIndex = 0;
                }
            }
            else {
                foreach (CellType_e cellType in CellFunctions.cellUpdateOrder)
                {
                    if (CellFunctions.GetUpdateType(cellType).Equals(CellUpdateType_e.TRACKED))
                    {
                        MetaSortTrackedCells((CellType_e)cellType);
                        foreach (Direction_e dir in CellFunctions.directionUpdateOrder)
                        {
                            SortTrackedCells(cellType, dir);
                            UpdateTracked(cellType, dir);
                        }
                    }
                    else
                    {
                        UpdateTicked(cellType);
                    }
                }
            }
            if (enemyCount == 0)
            {
                PlayerPrefs.SetInt("Level" + currentLevel, 1);
                if(mode == Mode_e.LEVEL)
                    nextButton.SetActive(true);
            }
            MSPT = System.DateTime.Now.Ticks - MSPT;
        }
    }
}
