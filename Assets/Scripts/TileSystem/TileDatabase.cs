using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TileDatabase", menuName = "Map/TileDatabase")]
public class TileDatabase : ScriptableObject
{
    public List<TileTypeConfig> tileConfigs;

    public TileTypeConfig GetConfig(string code)
    {
        return tileConfigs.Find(cfg => cfg.code == code);
    }
}
