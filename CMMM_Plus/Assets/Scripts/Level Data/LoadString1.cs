using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace load_og
{
    public static class LoadString_og
    {
        private const string cellKey = "0123456789abcdefgh⒜⒝⒞⒟⒠⒡⒢⒣⒤⒥⒦⒧⒨⒩⒪⒫⒬⒭⒮⒯⒰⒱⒲⒳⒴⒵<>[]ijklmnopqrstuvwxyz⓪①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳㉑㉒㉓㉔㉕㉖㉗㉘㉙ABCDEFGHIJKLMNOPQRⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏ#@~`STUVWXYZ!$%&+-.=?^ⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ*_¯˜{}";
        private static Dictionary<char, int> decode = new Dictionary<char, int>();
        private static bool debounce;

        private static void initDict()
        {
            for (int i = 0; i < 194; i++)
            {
                decode.Add(cellKey[i], i);
            }
        }


        private static int DecodeString(string str)
        {
            int output = 0;
            foreach (char c in str)
            {
                output *= 194;
                output += decode[c];
            }
            return output;
        }


        private static void SetCell(int c, int i)
        {
            //c is celldata index, i is level position index
            if (c % 2 == 1)
                GridManager.instance.tilemap.SetTile(new Vector3Int(i % CellFunctions.gridWidth, i / CellFunctions.gridWidth, 0), GridManager.instance.placebleTile);
            if (c >= 192)
                return;
            GridManager.instance.SpawnCell(
                (CellType_e)((c / 2) % 24),
                new Vector2(i % CellFunctions.gridWidth, i / CellFunctions.gridWidth),
                (Direction_e)(c / 48),
                false);
        }

        public static bool Load(string str)
        {
            if (!debounce)
            {
                initDict();
                debounce = true;
            }

            string[] arguments = str.Split(';');

            string levelName = "";
            string tutorialText = "";

            //used in V2/V3/V4
            int length;
            int dataIndex = 0;
            int gridIndex = 0;
            string temp;

            switch (arguments[0])
            {

                case "V1":
                    CellFunctions.gridWidth = int.Parse(arguments[1]);
                    CellFunctions.gridHeight = int.Parse(arguments[2]);
                    GridManager.instance.InitGridSize();

                    string[] placementCellLocationsStr = arguments[3].Split(',');
                    if (placementCellLocationsStr[0] != "")
                        foreach (string String in placementCellLocationsStr)
                        {
                            int x = int.Parse(String.Split('.')[0]);
                            int y = int.Parse(String.Split('.')[1]);
                            GridManager.instance.tilemap.SetTile(new Vector3Int(x, y, 0), GridManager.instance.placebleTile);
                        }

                    string[] cellStr = arguments[4].Split(',');
                    if (cellStr[0] != "")
                        foreach (string String in cellStr)
                        {
                            GridManager.instance.SpawnCell(
                                (CellType_e)int.Parse(String.Split('.')[0]),
                                new Vector2(
                                    int.Parse(String.Split('.')[2]),
                                    int.Parse(String.Split('.')[3])),
                                (Direction_e)int.Parse(String.Split('.')[1]),
                                false);
                        }

                    tutorialText = arguments[5];
                    levelName = arguments[6];
                    break;

                case "V2":
                    CellFunctions.gridWidth = DecodeString(arguments[1]);
                    CellFunctions.gridHeight = DecodeString(arguments[2]);
                    GridManager.instance.InitGridSize();
                    int cellData;

                    while (dataIndex < arguments[3].Length)
                    {
                        if (arguments[3][dataIndex] == ')' || arguments[3][dataIndex] == '(')
                        {

                            cellData = decode[arguments[3][dataIndex - 1]];
                            if (arguments[3][dataIndex] == ')')
                            {
                                dataIndex++;
                                length = decode[arguments[3][dataIndex]];
                            }
                            else
                            {
                                dataIndex++;
                                temp = "";
                                while (arguments[3][dataIndex] != ')')
                                {
                                    temp += arguments[3][dataIndex];
                                    dataIndex++;
                                }
                                length = DecodeString(temp);
                            }

                            if (cellData != 192)
                            {
                                for (int i = 0; i < length; i++)
                                {
                                    SetCell(cellData, gridIndex + i);
                                }
                            }
                            gridIndex += length;
                        }
                        else
                        {
                            SetCell(decode[arguments[3][dataIndex]], gridIndex);
                            gridIndex++;
                        }
                        dataIndex++;

                    }

                    tutorialText = arguments[4];
                    levelName = arguments[5];
                    break;

                case "V3":
                    CellFunctions.gridWidth = DecodeString(arguments[1]);
                    CellFunctions.gridHeight = DecodeString(arguments[2]);
                    GridManager.instance.InitGridSize();
                    int[] cellDataHistory = new int[CellFunctions.gridWidth * CellFunctions.gridHeight];
                    int offset;

                    while (dataIndex < arguments[3].Length)
                    {
                        if (arguments[3][dataIndex] == ')' || arguments[3][dataIndex] == '(')
                        {
                            if (arguments[3][dataIndex] == ')')
                            {
                                dataIndex += 2;
                                offset = decode[arguments[3][dataIndex - 1]];
                                length = decode[arguments[3][dataIndex]];

                            }
                            else
                            {
                                dataIndex++;
                                temp = "";
                                while (arguments[3][dataIndex] != ')' && arguments[3][dataIndex] != '(')
                                {
                                    temp += arguments[3][dataIndex];
                                    dataIndex++;
                                }
                                offset = DecodeString(temp);
                                if (arguments[3][dataIndex] == ')')
                                {
                                    dataIndex++;
                                    length = decode[arguments[3][dataIndex]];
                                }
                                else
                                {
                                    dataIndex++;
                                    temp = "";
                                    while (arguments[3][dataIndex] != ')')
                                    {
                                        temp += arguments[3][dataIndex];
                                        dataIndex++;
                                    }
                                    length = DecodeString(temp);
                                }
                            }
                            for (int i = 0; i < length; i++)
                            {
                                SetCell(cellDataHistory[gridIndex - offset - 1], gridIndex);
                                cellDataHistory[gridIndex] = cellDataHistory[gridIndex - offset - 1];
                                gridIndex++;
                            }
                        }
                        else
                        {
                            SetCell(decode[arguments[3][dataIndex]], gridIndex);
                            cellDataHistory[gridIndex] = decode[arguments[3][dataIndex]];
                            gridIndex++;
                        }

                        dataIndex++;
                    }

                    tutorialText = arguments[4];
                    levelName = arguments[5];
                    break;
            }
            GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>().text = tutorialText;
            return true;
        }
    }
}
