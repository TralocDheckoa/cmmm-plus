using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CellFunctions
{
    //HARD CODED VARIABLES
    //HARD CODED VARIABLES
    //HARD CODED VARIABLES
    //HARD CODED VARIABLES
    //HARD CODED VARIABLES
    public static Direction_e[] directionUpdateOrder = { Direction_e.RIGHT, Direction_e.LEFT, Direction_e.UP, Direction_e.DOWN };
    public static CellType_e[] cellUpdateOrder = { CellType_e.INPUTENEMY, CellType_e.CONVERTER, CellType_e.INPUTGENERATOR, CellType_e.GENERATOR, CellType_e.CWROTATER, CellType_e.CCWROTATER, CellType_e.RANDOMROTATER, CellType_e.FIXEDROTATER, CellType_e.FLIPPER, CellType_e.TELEPORTER, CellType_e.PACMAN, CellType_e.PLAYER, CellType_e.PULLER, CellType_e.INPUTMOVER, CellType_e.MOVER, CellType_e.NUDGE, CellType_e.FALL};
    public static Dictionary<CellType_e, CellUpdateType_e> cellUpdateTypeDictionary = new Dictionary<CellType_e, CellUpdateType_e>
    {
        [CellType_e.GENERATOR] = CellUpdateType_e.TRACKED,
        [CellType_e.CONVERTER] = CellUpdateType_e.TRACKED,
        [CellType_e.TELEPORTER] = CellUpdateType_e.TRACKED,
        [CellType_e.CWROTATER] = CellUpdateType_e.TICKED,
        [CellType_e.CCWROTATER] = CellUpdateType_e.TICKED,
        [CellType_e.RANDOMROTATER] = CellUpdateType_e.TICKED,
        [CellType_e.FIXEDROTATER] = CellUpdateType_e.TICKED,
        [CellType_e.FLIPPER] = CellUpdateType_e.TICKED,
        [CellType_e.FALL] = CellUpdateType_e.TRACKED,
        [CellType_e.MOVER] = CellUpdateType_e.TRACKED,
        [CellType_e.NUDGE] = CellUpdateType_e.TRACKED,
        [CellType_e.PULLER] = CellUpdateType_e.TRACKED,
        [CellType_e.PACMAN] = CellUpdateType_e.TRACKED,
        [CellType_e.PLAYER] = CellUpdateType_e.TRACKED,
        [CellType_e.SLIDE] = CellUpdateType_e.BASE,
        [CellType_e.DIRECTIONAL] = CellUpdateType_e.BASE,
        [CellType_e.BLOCK] = CellUpdateType_e.BASE,
        [CellType_e.WALL] = CellUpdateType_e.BASE,
        [CellType_e.ENEMY] = CellUpdateType_e.BASE,
        [CellType_e.PRESENT] = CellUpdateType_e.BASE,
        [CellType_e.VIPPRESENT] = CellUpdateType_e.BASE,
        [CellType_e.TRASH] = CellUpdateType_e.BASE,

        [CellType_e.INPUTMOVER] = CellUpdateType_e.TRACKED,
        [CellType_e.INPUTGENERATOR] = CellUpdateType_e.TRACKED,
        [CellType_e.INPUTENEMY] = CellUpdateType_e.TICKED,
    };
    //HARD CODED VARIABLES
    //HARD CODED VARIABLES
    //HARD CODED VARIABLES
    //HARD CODED VARIABLES
    //HARD CODED VARIABLES

    //An disctionary defining the specialized ID's used in sorting, for tracked and 
    public static Dictionary<CellType_e, int> steppedCellIdDictionary = new Dictionary<CellType_e, int>();

    public static int gridWidth = 1;
    public static int gridHeight = 1;
    //Used to check which cell might be at location x, y.
    public static Cell[,] cellGrid;
    //Used to check if x, y is considered a placeable tile.
    public static bool[,] placeableTiles;

    public static LinkedList<Cell> cellList;
    //Cells made during the simulation
    public static LinkedList<Cell> generatedCellList;

    //Cells that need to be updated but not in a specific order.
    //tickedCellList[CellType];
    public static LinkedList<Cell>[] tickedCellList;

    //Cells that need to be updated but in a specific order (Depending on direction).
    //trackedCells[TrackedCell ID][Direction, Distince];
    //public static LinkedList<Cell>[][,] trackedCells;
    // changed to [,][]
    // the jagged array is 2 dimensional
    public static LinkedList<Cell>[,][] trackedCells;

    //trackedCellRotationUpdateQueue[CellType] Cell type must be sorted into a new direction queue if it has been rotated since it was last sorted
    public static LinkedList<Cell>[] trackedCellRotationUpdateQueue;
    //trackedCellPositionUpdateQueue[CellType, Cell Direction] Cell type "X" facing direction "Y" must be sorted before Cells of type X facing direction Y are updated
    public static LinkedList<Cell>[,] trackedCellPositionUpdateQueue;

    public static int GetSteppedCellId(CellType_e type) {
        return steppedCellIdDictionary[type];
    }

    public static CellUpdateType_e GetUpdateType(CellType_e type) {
        return cellUpdateTypeDictionary[type];
    }
}
