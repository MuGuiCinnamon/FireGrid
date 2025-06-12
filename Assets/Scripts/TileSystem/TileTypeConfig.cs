using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileTypeConfig
{
    public string code; // "G", "D", "R"
    public List<string> activeLayers;
    public bool isFlammable;
    public bool isWalkable;
}
