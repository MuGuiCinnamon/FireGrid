using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FireUtility
{
    public static bool IsFireActiveAt(Vector2Int pos)
    {
        var fires = Object.FindObjectsByType<Fire>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var fire in fires)
        {
            if (fire.GridPos == pos && fire.IsActive())
                return true;
        }
        return false;
    }
}
