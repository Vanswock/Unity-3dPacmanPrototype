using System;
using UnityEngine;

public class TwoDtoThreeDMaze : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Texture2D mazeTexture;
    int ppu = 1; // pixels per unit
    float pixelmovelength = 0.5f;
    private bool[,] walkable;

    private int width;
    private int height;
    int wallWidth = 0;
    int wallThickness = 1;
    int wallHeight = 3;

    private void Awake()
    {
        width = mazeTexture.width / ppu;
        height = mazeTexture.height / ppu;
        walkable = new bool[width, height];

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Color pixel = mazeTexture.GetPixel(x * ppu, z * ppu);

                walkable[x, z] = pixel.a < 0.5f;
            }
        }

        for (int i = 0; i < height; i++)
        {
            int startX = 0;
            for (int j = 1; j <= width; j++)
            {

                if (j == width || walkable[startX, i] != walkable[j, i])
                {
                    wallWidth = j - startX;



                    if (mazeTexture.GetPixel(startX, i).a >= 0.5f)
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                        wallWidth = j - startX;
                        cube.transform.localScale = new Vector3(wallWidth, wallHeight, wallThickness);
                        cube.transform.localPosition = new Vector3(startX + wallWidth / 2f, wallHeight / 2f, i + wallThickness / 2f);

                    }


                    startX = j;
                }
            }
        }

    }
    public bool IsWalkable(Vector3Int cell)
    {
        // Outside the image is considered a wall.
        if (cell.x < 0 || cell.x >= width ||
            cell.z < 0 || cell.z >= height)
        {
            return false;
        }

        return walkable[cell.x, cell.z];
    }

    public Vector3 CellToWorld(Vector3Int cell)
    {
        // Center of the cell.
        Vector3 localPosition = new Vector3(cell.x + pixelmovelength, 0.5f, cell.z + pixelmovelength);

        return transform.TransformPoint(localPosition);
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        Vector3 localPosition = transform.InverseTransformPoint(worldPosition);

        return new Vector3Int(Mathf.FloorToInt(localPosition.x), 0, Mathf.FloorToInt(localPosition.z));
    }

}