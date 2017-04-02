using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MapMeshGenerator {

    [SerializeField]
    public float cellSize;

    [SerializeField]
    public float sideHeight;

    private Vector3 up;
    private Vector3 right;
    private Vector3 height;

    public Mesh GenerateGeometryFromMesh(bool[,] matrixOcupation)
    {
        up = new Vector3(0, 0, cellSize / 2f);
        right = new Vector3(cellSize / 2f, 0, 0);
        height = new Vector3(0, sideHeight, 0);

        Mesh gridMesh = new Mesh();
        gridMesh.name = "GridMesh";

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        int vertexCount = 0;
        for (int x = 0; x < matrixOcupation.GetLength(0); x++)
            for (int z = 0; z < matrixOcupation.GetLength(1); z++)
                generateMatrixCell(vertices, triangles, matrixOcupation, x, z, ref vertexCount);

        gridMesh.SetVertices(vertices);
        gridMesh.SetTriangles(triangles, 0, true);
        gridMesh.RecalculateNormals();

        return gridMesh;
    }


    private void generateMatrixCell(List<Vector3> vertices, List<int> triangles, bool[,] matrixOcupation, int x, int z, ref int vertexCount)
    {
        if (matrixOcupation[x, z])
        {
            Vector3[] face = new Vector3[4];

            face[0] = getNorthWestAtFloorLevel(matrixOcupation, x, z);
            face[1] = getNorthEastAtFloorLevel(matrixOcupation, x, z);
            face[2] = getSouthWestAtFloorLevel(matrixOcupation, x, z);
            face[3] = getSouthEastAtFloorLevel(matrixOcupation, x, z);
            generateSquareFace(face, vertices, triangles, matrixOcupation, ref vertexCount);

            face[0] = getNorthEastAtPitLevel(matrixOcupation, x, z);
            face[1] = getNorthWestAtPitLevel(matrixOcupation, x, z);
            face[2] = getSouthEastAtPitLevel(matrixOcupation, x, z);
            face[3] = getSouthWestAtPitLevel(matrixOcupation, x, z);
            generateSquareFace(face, vertices, triangles, matrixOcupation, ref vertexCount);

            /*bool isTopEdge = z > 0 && !matrixOcupation[x, z - 1];
            bool isBottomEdge = z < matrixOcupation.GetLength(1) - 1 && !matrixOcupation[x, z + 1];
            bool isLeftEdge = x > 0 && !matrixOcupation[x - 1, z];
            bool isRightEdge = x < matrixOcupation.GetLength(0) - 1 && !matrixOcupation[x + 1, z];
            */
            if (isTopEdge(matrixOcupation,x,z))
            {
                face[0] = getNorthEastAtFloorLevel(matrixOcupation, x, z);
                face[1] = getNorthWestAtFloorLevel(matrixOcupation, x, z);
                face[2] = getNorthEastAtPitLevel(matrixOcupation, x, z);
                face[3] = getNorthWestAtPitLevel(matrixOcupation, x, z);
                generateSquareFace(face, vertices, triangles, matrixOcupation, ref vertexCount);
            }

            if (isBottomEdge(matrixOcupation, x, z))
            {
                face[0] = getSouthWestAtFloorLevel(matrixOcupation, x, z);
                face[1] = getSouthEastAtFloorLevel(matrixOcupation, x, z);
                face[2] = getSouthWestAtPitLevel(matrixOcupation, x, z);
                face[3] = getSouthEastAtPitLevel(matrixOcupation, x, z);
                generateSquareFace(face, vertices, triangles, matrixOcupation, ref vertexCount);
            }

            if (isLeftEdge(matrixOcupation, x, z))
            {
                face[0] = getNorthWestAtFloorLevel(matrixOcupation, x, z);
                face[1] = getSouthWestAtFloorLevel(matrixOcupation, x, z);
                face[2] = getNorthWestAtPitLevel(matrixOcupation, x, z);
                face[3] = getSouthWestAtPitLevel(matrixOcupation, x, z);
                generateSquareFace(face, vertices, triangles, matrixOcupation, ref vertexCount);
            }

            if (isRightEdge(matrixOcupation, x, z))
            {
                face[0] = getSouthEastAtFloorLevel(matrixOcupation, x, z);
                face[1] = getNorthEastAtFloorLevel(matrixOcupation, x, z);
                face[2] = getSouthEastAtPitLevel(matrixOcupation, x, z);
                face[3] = getNorthEastAtPitLevel(matrixOcupation, x, z);
                generateSquareFace(face, vertices, triangles, matrixOcupation, ref vertexCount);
            }
        }
    }

    private void generateSquareFace(Vector3[] faceVertices, List<Vector3> vertices, List<int> triangles, bool[,] matrixOcupation, ref int vertexCount)
    {
        //faceVertices must be ordered like this
        // 0 ------------ 1
        // -     -        -
        // -         -    -
        // 2 - - - - - -  3

        // top triangle
        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 1);
        triangles.Add(vertexCount + 3);

        // bottom triangle
        triangles.Add(vertexCount);
        triangles.Add(vertexCount + 3);
        triangles.Add(vertexCount + 2);

        foreach (Vector3 vertex in faceVertices)
            vertices.Add(vertex);
        
        vertexCount += 4;
    }

    bool isTopEdge(bool[,] matrix, int x, int z)
    {
        return z > 0 && !matrix[x, z - 1] || z == 0;
    }
    
    bool isBottomEdge(bool[,] matrix, int x, int z)
    {
        return z < matrix.GetLength(1) - 1 && !matrix[x, z + 1] || z == matrix.GetLength(1) - 1;
    }

    bool isLeftEdge(bool[,] matrix, int x, int z)
    {
        return x > 0 && !matrix[x - 1, z] || x == 0;
    }

    bool isRightEdge(bool[,] matrix, int x, int z)
    {
        return x < matrix.GetLength(0) - 1 && !matrix[x + 1, z] || x == matrix.GetLength(0) -1;
    }

    private void addSquareTriangles(List<int> triangles, Dictionary<Vector3, int> vertexIndex, bool[,] matrixOcupation, int x, int z)
    {
        if (matrixOcupation[x, z])
        {
            Vector3 squareCenter = getCenterAtMatrixPosition(matrixOcupation, x, z);
            int floorTopLeftIndex = vertexIndex[getNorthWestAtFloorLevel(matrixOcupation, x, z)];
            int floorTopRightIndex = vertexIndex[squareCenter + right + up];
            int floorBottomLeftIndex = vertexIndex[squareCenter - right - up];
            int floorBottomRightIndex = vertexIndex[squareCenter + right - up];
            //bottomTriangle
            triangles.Add(floorTopLeftIndex);
            triangles.Add(floorBottomRightIndex);
            triangles.Add(floorBottomLeftIndex);
            //topTriangle
            triangles.Add(floorTopLeftIndex);
            triangles.Add(floorTopRightIndex);
            triangles.Add(floorBottomRightIndex);

            /*bool isTopEdge = z > 0 && !matrixOcupation[x, z - 1];
            bool isBottomEdge = z < matrixOcupation.GetLength(1) - 1 && !matrixOcupation[x, z + 1];
            bool isLeftEdge = x > 0 && !matrixOcupation[x - 1, z];
            bool isRightEdge = x < matrixOcupation.GetLength(0) - 1 && !matrixOcupation[x + 1, z];
            */
            int bottom1, bottom2;
            if (isTopEdge(matrixOcupation,x,z))
            {
                bottom1 = vertexIndex[squareCenter - right + up - height];
                bottom2 = vertexIndex[squareCenter + right + up - height];
                //bottomTriangle
                triangles.Add(floorTopLeftIndex);
                triangles.Add(bottom1);
                triangles.Add(bottom2);
                //topTriangle
                triangles.Add(floorTopLeftIndex);
                triangles.Add(bottom2);
                triangles.Add(floorTopRightIndex);

            }

            if (isBottomEdge(matrixOcupation, x, z))
            {
                bottom1 = vertexIndex[squareCenter - right - up - height];
                bottom2 = vertexIndex[squareCenter + right - up - height];
                //bottomTriangle
                triangles.Add(floorBottomRightIndex);
                triangles.Add(bottom2);
                triangles.Add(bottom1);
                //topTriangle
                triangles.Add(floorBottomRightIndex);
                triangles.Add(bottom1);
                triangles.Add(floorBottomLeftIndex);
            }

            if (isLeftEdge(matrixOcupation, x, z))
            {
                bottom1 = vertexIndex[squareCenter - right + up - height];
                bottom2 = vertexIndex[squareCenter - right - up - height];
                //bottomTriangle
                triangles.Add(floorTopLeftIndex);
                triangles.Add(bottom2);
                triangles.Add(bottom1);
                //topTriangle
                triangles.Add(floorTopLeftIndex);
                triangles.Add(floorBottomLeftIndex);
                triangles.Add(bottom2);
            }

            if (isRightEdge(matrixOcupation, x, z))
            {
                bottom1 = vertexIndex[squareCenter + right + up - height];
                bottom2 = vertexIndex[squareCenter + right - up - height];
                //bottomTriangle
                triangles.Add(floorBottomRightIndex);
                triangles.Add(bottom1);
                triangles.Add(bottom2);
                //topTriangle
                triangles.Add(floorBottomRightIndex);
                triangles.Add(floorTopRightIndex);
                triangles.Add(bottom1);
            }
        }
    }

    private void addSquareVertices(List<Vector3> vertices, Dictionary<Vector3, int> verticesIndex, bool[,] matrixOcupation, int x, int z)
    {
        if (matrixOcupation[x, z])
        {
            Vector3 squareCenter = getCenterAtMatrixPosition(matrixOcupation, x, z);
            List<Vector3> squareVertices = new List<Vector3>()
            {
                squareCenter - right + up,
                squareCenter + right + up,
                squareCenter - right - up,
                squareCenter + right - up
            };
            
            if (isTopEdge(matrixOcupation,x,z))
            {
                Debug.Log("is top edge");
                squareVertices.Add(squareVertices[0] - height);
                squareVertices.Add(squareVertices[1] - height);
            }

            if (isBottomEdge(matrixOcupation, x, z))
            {
                squareVertices.Add(squareVertices[2] - height);
                squareVertices.Add(squareVertices[3] - height);
            }

            if (isLeftEdge(matrixOcupation, x, z))
            {
                squareVertices.Add(squareVertices[0] - height);
                squareVertices.Add(squareVertices[2] - height);
            }

            if (isRightEdge(matrixOcupation, x, z))
            {
                squareVertices.Add(squareVertices[1] - height);
                squareVertices.Add(squareVertices[3] - height);
            }

            foreach (Vector3 vertex in squareVertices)
            {
                if (!verticesIndex.ContainsKey(vertex))
                {
                    vertices.Add(vertex);
                    verticesIndex.Add(vertex, vertices.IndexOf(vertex));
                }
            }
        }
    }


    Vector3 getCenterAtMatrixPosition(bool[,] matrixOcupation, int x, int z)
    {
        return new Vector3(
            (-(matrixOcupation.GetLength(0) * cellSize) / 2) + (cellSize / 2) + (x * cellSize), 
            0,
            ((matrixOcupation.GetLength(1) * cellSize) / 2) - (cellSize / 2) - (z * cellSize)
            );
    }

    // Floor Level
    Vector3 getNorthWestAtFloorLevel(bool[,] matrixOcupation, int x, int z)
    {
        return getCenterAtMatrixPosition(matrixOcupation, x, z) + up - right;
    }

    Vector3 getNorthEastAtFloorLevel(bool[,] matrixOcupation, int x, int z)
    {
        return getCenterAtMatrixPosition(matrixOcupation, x, z) + up + right;
    }

    Vector3 getSouthWestAtFloorLevel(bool[,] matrixOcupation, int x, int z)
    {
        return getCenterAtMatrixPosition(matrixOcupation, x, z) - up - right;
    }

    Vector3 getSouthEastAtFloorLevel(bool[,] matrixOcupation, int x, int z)
    {
        return getCenterAtMatrixPosition(matrixOcupation, x, z) - up + right;
    }

    // pit level
    Vector3 getNorthWestAtPitLevel(bool[,] matrixOcupation, int x, int z)
    {
        return getNorthWestAtFloorLevel(matrixOcupation,x,z) - height;
    }

    Vector3 getNorthEastAtPitLevel(bool[,] matrixOcupation, int x, int z)
    {
        return getNorthEastAtFloorLevel(matrixOcupation, x, z) - height;
    }

    Vector3 getSouthWestAtPitLevel(bool[,] matrixOcupation, int x, int z)
    {
        return getSouthWestAtFloorLevel(matrixOcupation, x, z) - height;
    }

    Vector3 getSouthEastAtPitLevel(bool[,] matrixOcupation, int x, int z)
    {
        return getSouthEastAtFloorLevel(matrixOcupation,x,z) - height;
    }

    public List<Vector3> GenerateSpawns(bool[,] mapMatrix)
    {
        List<Vector3> spawns = new List<Vector3>();
        for (int x = 0; x < mapMatrix.GetLength(0); x++)
            for (int z = 0; z < mapMatrix.GetLength(1); z++)
                if (mapMatrix[x, z])
                {
                    Vector3 newSpawnPosition = getCenterAtMatrixPosition(mapMatrix, x, z);
                    spawns.Add(newSpawnPosition);
                }
        return spawns;
    }
}
