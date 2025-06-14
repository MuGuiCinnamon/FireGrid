using System.Collections.Generic;
using UnityEngine;
using System;


public class Tile : MonoBehaviour
{
    public enum TileType { Grass, Dirt, Rock }

    public TileType tileType;
    public bool isFlammable;
    //public bool isWalkable;

    public List<GameObject> removableLayers = new();
    public List<GameObject> permanentLayers = new();

    private Dictionary<GameObject, SpriteRenderer> removableRenderers = new();

    public Vector2Int gridPosition;
    public bool isWalkable = true;
    public bool isBurnable = true;
    public bool hasFire = false;
    private int fireStartStep = -1;


    void Awake()
    {
        // if (TileMapManager.Instance != null)
        // {
        //     TileMapManager.Instance.RegisterTile(gridPosition, this);
        // }
        foreach (var obj in removableLayers)
        {
            var sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
                removableRenderers[obj] = sr;
        }
    }
    public virtual void OnPlayerInteract()
    {
        if (!isBurnable)
    {
        Debug.Log("Tile is not burnable");
        return;
    }

    if (hasFire)
    {
        Debug.Log("Tile already has fire");
        return;
    }

    if (TileFireManager.Instance == null)
    {
        Debug.LogError("TileFireManager.Instance is null!");
        return;
    }
        if (isBurnable && !hasFire)
        {
            TileFireManager.Instance.CreateFireAt(gridPosition);
            hasFire = true;

        }
        Debug.Log("Player interacted with tile: " + gameObject.name);
    }

    public void Initialize(TileType type, Vector2Int pos, int sortingBase = 0)
    {
        tileType = type;
        gridPosition = pos;
        transform.position = new Vector3(pos.x * 0.96f, -pos.y * 0.96f, 0f);

        int baseOrder = gridPosition.y * 10 + sortingBase;

        foreach (Transform child in transform)
        {
            var sr = child.GetComponent<SpriteRenderer>();
            if (sr == null) continue;

            if (child.name.Contains("Grass"))
                sr.sortingOrder = baseOrder + 2; // 最上层
            else if (child.name.Contains("Dirt"))
                sr.sortingOrder = baseOrder + 1;
            else if (child.name.Contains("Rock"))
                sr.sortingOrder = baseOrder + 1; // 和 Dirt 同层
            else
                sr.sortingOrder = baseOrder;
        }
        if (TileMapManager.Instance != null)
        {
            TileMapManager.Instance.RegisterTile(gridPosition, this);
        }
    }
    


    public void ApplyConfig(TileTypeConfig config)
    {
        tileType = Enum.TryParse(config.code, out TileType result) ? result : TileType.Dirt;
        isFlammable = config.isFlammable;
        isWalkable = config.isWalkable;
        isBurnable = isFlammable;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(config.activeLayers.Contains(child.name));
        }

        removableLayers.Clear();
        permanentLayers.Clear();

        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeSelf) continue;

            if (child.name.Contains("Grass"))
                removableLayers.Add(child.gameObject);
            else
                permanentLayers.Add(child.gameObject);
        }
    }

    public void AttachFire(int playerStep)
    {
        if (!isFlammable) return;
        fireStartStep = playerStep;
        Debug.Log($"✅ Tile {gridPosition} 开始燃烧于 Step {playerStep}");

    }

    public void UpdateFireTransparency(int currentStep)
    {
        // if (fireStartStep < 0) return;

        // int age = currentStep - fireStartStep;
        // float t = Mathf.Clamp01(age / 6f);
        float t = Mathf.Clamp01(currentStep / 6f);

        foreach (var kvp in removableRenderers)
        {
            var obj = kvp.Key;
            var sr = kvp.Value;
            if (obj.name.Contains("Grass"))
            {
                // 从白色逐渐变黑色（Lerp）
                sr.color = Color.Lerp(Color.white, Color.black, t);

            }
        }
        Debug.Log($"🔥 Tile at {gridPosition} burning. Step={currentStep}, Lerp={t}");


        if (currentStep >= 6)
        {
            ClearFireEffect();
        }
    }

    public void ClearFireEffect()
    {
        fireStartStep = -1;
        isFlammable = false;

        foreach (var kvp in removableRenderers)
        {
            var obj = kvp.Key;
            var sr = kvp.Value;

            if (obj.name.Contains("Grass"))
            {
                sr.color = Color.black;
            }
        }

        // foreach (var obj in removableLayers)
        // {
        //     obj.SetActive(false);
        // }

        tileType = TileType.Dirt;
        Debug.Log($"🔥 清理完火焰，隐藏图层并标记为 Dirt：{gridPosition}");

    }
}
