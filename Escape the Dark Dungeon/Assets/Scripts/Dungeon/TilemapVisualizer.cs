using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap, wallsTilemap;
    [SerializeField]
    private List<TileBase> walls;
    [SerializeField]
    private List<TileBase> tiles;
    public void paintTiles(IEnumerable<Vector2Int> tilePos)
    {
        paintTiles(tilePos, tilemap, tiles);
    }

    private void paintTiles(IEnumerable<Vector2Int> tilePos, Tilemap tilemap, List<TileBase> tiles)
    {
        foreach(var pos in tilePos)
        {
            PaintSingleTile(tiles, tilemap, pos);
        }
    }

    private void PaintSingleTile(List<TileBase> tiles, Tilemap tilemap, Vector2Int tilePos)
    {
        var tileCoords = tilemap.WorldToCell((Vector3Int)tilePos);
        var tile = tiles[UnityEngine.Random.Range(0, tiles.Count)];
        tilemap.SetTile(tileCoords, tile);
    }

    public void Clear()
    {
        tilemap.ClearAllTiles();
        wallsTilemap.ClearAllTiles();
    }

    internal void paintSingleWall(object pos)
    {
        PaintSingleTile(walls, wallsTilemap, (Vector2Int)pos); 
    }
}
