using System.Text;
using UnityEngine;

public class Save1 : MonoBehaviour
{
    public GameObject saveText;
    private const string cellKey = "0123456789abcdefgh⒜⒝⒞⒟⒠⒡⒢⒣⒤⒥⒦⒧⒨⒩⒪⒫⒬⒭⒮⒯⒰⒱⒲⒳⒴⒵<>[]ijklmnopqrstuvwxyz⓪①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳㉑㉒㉓㉔㉕㉖㉗㉘㉙ABCDEFGHIJKLMNOPQRⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏ#@~`STUVWXYZ!$%&+-.=?^ⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ*_¯˜{}";
    public GameObject canvas;

    public void Awake()
    {
        saveText.gameObject.SetActive(false);
    }

    private string EncodeInt(int num)
    {
        if (num < 194)
            return cellKey[num % 194].ToString();

        string output = "";
        while (num > 0)
        {
            output = cellKey[num % 194] + output;
            num /= 194;
        }
        return output;
    }

    public void SaveString() {
        SaveString(new Vector2Int(0, CellFunctions.gridHeight - 1), new Vector2Int(CellFunctions.gridWidth - 1, 0));
    }

    public void SaveString(Vector2Int topLeft, Vector2Int bottomRight)
    {
        int format = PlayerPrefs.GetInt("ExportFormat", 2) + 1;
        StringBuilder output = new StringBuilder();
        output.Append("V" + format + ";");

        if (format == 1) output.Append(((bottomRight.x + 1) - topLeft.x) + ";" + ((topLeft.y + 1) - bottomRight.y) + ";");
        else output.Append(EncodeInt(((bottomRight.x + 1) - topLeft.x)) + ";" + EncodeInt(((topLeft.y + 1) - bottomRight.y)) + ";");

        int[] cellData;
        int dataIndex = 0;

        switch (PlayerPrefs.GetInt("ExportFormat", 2))
        {
            case 0:
                bool debounce = false;
                for (int y = bottomRight.y; y <= topLeft.y; y++)
                {
                    for (int x = topLeft.x; x <= bottomRight.x; x++)
                    {
                        if (GridManager.instance.tilemap.GetTile(new Vector3Int(x, y, 0)) == GridManager.instance.placebleTile)
                        {
                            if (debounce)
                                output.Append(",");
                            debounce = true;
                            output.Append((x - topLeft.x) + "." + (y - bottomRight.y));
                        }
                    }
                }
                output.Append(";");

                debounce = false;
                foreach (Cell cell in CellFunctions.cellList)
                {
                    if (debounce)
                        output.Append(",");
                    debounce = true;
                    output.Append((int)cell.cellType + "." + (int)cell.spawnRotation + "." + (int)(cell.spawnPosition.x - topLeft.x) + "." + (int)(cell.spawnPosition.y - bottomRight.y));
                }
                break;

            case 1:
                cellData = new int[((bottomRight.x + 1) - topLeft.x) * ((topLeft.y + 1) - bottomRight.y)];

                for (int y = bottomRight.y; y <= topLeft.y; y++)
                {
                    for (int x = topLeft.x; x <= bottomRight.x; x++)
                    {
                        cellData[(x - topLeft.x) + ((y - bottomRight.y) * (bottomRight.x + 1 - topLeft.x))] = GridManager.instance.tilemap.GetTile(new Vector3Int(x, y, 0)) == GridManager.instance.placebleTile ? 193 : 192;
                    }
                }
                foreach (Cell cell in CellFunctions.cellList)
                {
                    cellData[(int)(cell.spawnPosition.x - topLeft.x) + ((int)(cell.spawnPosition.y - bottomRight.y) * ((bottomRight.x + 1) - topLeft.x))] += (2 * (int)cell.cellType) + (48 * cell.spawnRotation) - 192;
                }

                int runLength = 1;

                while (dataIndex < cellData.Length)
                {

                    if (dataIndex + 1 < cellData.Length && cellData[dataIndex] == cellData[dataIndex + 1])
                        runLength++;
                    else
                    {
                        if (runLength > 3)
                        {
                            if (EncodeInt(runLength - 1).Length > 1)
                                output.Append(cellKey[cellData[dataIndex]] + "(" + EncodeInt(runLength - 1) + ")");
                            else
                                output.Append(cellKey[cellData[dataIndex]] + ")" + EncodeInt(runLength - 1));
                        }
                        else
                            output.Append(new string(cellKey[cellData[dataIndex]], runLength));
                        runLength = 1;
                    }
                    dataIndex++;
                }
                break;

            case 2:
                cellData = new int[((bottomRight.x + 1) - topLeft.x) * ((topLeft.y + 1) - bottomRight.y)];

                for (int y = bottomRight.y; y <= topLeft.y; y++)
                {
                    for (int x = topLeft.x; x <= bottomRight.x; x++)
                    {
                        cellData[(x - topLeft.x) + ((y - bottomRight.y) * (bottomRight.x + 1 - topLeft.x))] = GridManager.instance.tilemap.GetTile(new Vector3Int(x, y, 0)) == GridManager.instance.placebleTile ? 193 : 192;
                    }
                }
                foreach (Cell cell in CellFunctions.cellList)
                {
                    cellData[(int)(cell.spawnPosition.x - topLeft.x) + ((int)(cell.spawnPosition.y - bottomRight.y) * ((bottomRight.x + 1) - topLeft.x))] += (2 * (int)cell.cellType) + (48 * cell.spawnRotation) - 192;
                }

                int matchLength;
                int maxMatchLength;
                int maxMatchOffset = 0;

                while (dataIndex < cellData.Length)
                {
                    maxMatchLength = 0;
                    for (int matchOffset = 1; matchOffset <= dataIndex; matchOffset++)
                    {
                        matchLength = 0;
                        while (dataIndex + matchLength < cellData.Length && cellData[dataIndex + matchLength] == cellData[dataIndex + matchLength - matchOffset])
                        {
                            matchLength++;
                            if (matchLength > maxMatchLength)
                            {
                                maxMatchLength = matchLength;
                                maxMatchOffset = matchOffset - 1;
                            }
                        }
                    }
                    if (maxMatchLength > 3)
                    {
                        if (EncodeInt(maxMatchLength).Length == 1)
                        {
                            if (EncodeInt(maxMatchOffset).Length == 1)
                            {
                                if (maxMatchLength > 3)
                                {
                                    output.Append(")" + EncodeInt(maxMatchOffset) + EncodeInt(maxMatchLength));
                                    dataIndex += maxMatchLength - 1;
                                }
                                else
                                    output.Append(cellKey[cellData[dataIndex]]);
                            }
                            else
                            {
                                if (maxMatchLength > 3 + EncodeInt(maxMatchOffset).Length)
                                {
                                    output.Append("(" + EncodeInt(maxMatchOffset) + ")" + EncodeInt(maxMatchLength));
                                    dataIndex += maxMatchLength - 1;
                                }
                                else
                                    output.Append(cellKey[cellData[dataIndex]]);
                            }
                        }
                        else
                        {
                            output.Append("(" + EncodeInt(maxMatchOffset) + "(" + EncodeInt(maxMatchLength) + ")");
                            dataIndex += maxMatchLength - 1;
                        }
                    }
                    else
                        output.Append(cellKey[cellData[dataIndex]]);

                    maxMatchLength = 0;
                    dataIndex += 1;
                }
                break;
                //case 3:
                //    output = "V4;" + EncodeInt(CellFunctions.gridWidth) + ";" + EncodeInt(CellFunctions.gridHeight) + ";";
                //    StringBuilder rawOut = new StringBuilder();

                //    for (int y = 0; y < CellFunctions.gridHeight; y++)
                //    {
                //        for (int x = 0; x < CellFunctions.gridWidth; x++)
                //        {
                //            bool placable = GridManager.instance.tilemap.GetTile(new Vector3Int(x, y, 0)) == GridManager.instance.placebleTile;
                //            Cell currentCell = CellFunctions.cellGrid[x, y];
                //            if (currentCell == null)
                //            {
                //                rawOut.Append(EncodeInt(placable ? 73 : 72)); continue;
                //            }
                //            rawOut.Append(EncodeInt((2 * (int)currentCell.cellType) + (18 * currentCell.spawnRotation) + (placable ? 1 : 0)));
                //        }
                //    }

                //    output += Compression.BrotliString(rawOut.ToString()) + ";;";
                //    rawOut = null;
                //    break;
        }

        cellData = null;
        output.Append(";;");

        GridManager.hasSaved = true;
        GUIUtility.systemCopyBuffer = output.ToString();
        GameObject go = Instantiate(saveText, canvas.GetComponent<Transform>(), true);
        go.SetActive(true);
        Destroy(go, 3);
    }
}
