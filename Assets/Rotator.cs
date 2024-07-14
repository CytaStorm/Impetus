using UnityEngine;
using UnityEngine.Tilemaps;

public class RotateTilemap : MonoBehaviour
{
    public GameObject originalTilemapObject; // The original tilemap GameObject
    public GameObject rotatedTilemapObject;  // The new tilemap GameObject to hold the rotated tiles

    public void Rotate()
    {
        Tilemap originalTilemap = originalTilemapObject.GetComponent<Tilemap>();
        Tilemap rotatedTilemap = rotatedTilemapObject.GetComponent<Tilemap>();

        BoundsInt bounds = originalTilemap.cellBounds;
        TileBase[] allTiles = originalTilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                int originalIndex = x + y * bounds.size.x;
                TileBase tile = allTiles[originalIndex];

                if (tile != null)
                {
                    // Calculate the new position for the tile
                    Vector3Int originalPosition = new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0);
                    Vector3Int newPosition = new Vector3Int(bounds.yMax - 1 - y, bounds.xMin + x, 0);

                    // Set the tile in the rotated tilemap
                    rotatedTilemap.SetTile(newPosition, tile);
                }
            }
        }

        rotatedTilemap.RefreshAllTiles();
    }
}
