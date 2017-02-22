using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GridMeshGenerator
{
    // constructor parameters
    [SerializeField]
    private int numberOfCellsToFill;

    [SerializeField]
    private bool closePiecesHavePriority;

    [SerializeField]
    private float cellSize;

    [SerializeField]
    private float sideHeight;

    //precalculated values to speed up
    private Vector3 up;
    private Vector3 right;
    private Vector3 height;

    // results
    private bool[,] matrixOcupation;
    private Mesh gridMesh;



    /**
     * numberOfCellsToFill: Numero minim de caselles que tindra el que generi
     * cellSize: Mida dels costats de les caselles que genri
     * sideHeight: Mida de l'alçada del mapa
     * closePiecesHavePriority: cert genera mapes mes propers, fals genera mapes mes separats
    **/
    /*public GridMeshGenerator(int numberOfCellsToFill, float cellSize, float sideHeight, bool closePiecesHavePriority)
    {
        this.numberOfCellsToFill = numberOfCellsToFill;
        this.cellSize = cellSize;
        this.sideHeight = sideHeight;
        this.closePiecesHavePriority = closePiecesHavePriority;

        up = new Vector3(0, 0, cellSize / 2f);
        right = new Vector3(cellSize / 2f, 0, 0);
        height = new Vector3(0, sideHeight, 0);
    }*/

    public Mesh generateMap(int seed)
    {
        up = new Vector3(0, 0, cellSize / 2f);
        right = new Vector3(cellSize / 2f, 0, 0);
        height = new Vector3(0, sideHeight, 0);

        Random.InitState(seed);
        generateMatrix();
        generateGeometry();
        return gridMesh;
    }

    void generateMatrix()
    {
        matrixOcupation = new bool[numberOfCellsToFill * 2 + 1, numberOfCellsToFill * 2 + 1];
        List<Vector2> validPositions = new List<Vector2>();
        // com que hem fet un array de mapSize*2 +1 la casella del mig queda a la posicio escrita
        Vector2 startingPosition = new Vector2(numberOfCellsToFill + 1, numberOfCellsToFill + 1);
        validPositions.Add(startingPosition);

        // generem les peces
        for (int piece = 0; piece < numberOfCellsToFill; piece++)
        {
            if (validPositions.Count > 0)
            {
                Vector2 matrixPosition = getRandomValidPosition(validPositions);
                occupyPosition(validPositions, matrixPosition);
            }
            else
            {
                Debug.LogError("No more pieces can be placed when generating map"); // TODO make a sistem to try again
            }
        }

        //omplim els espais en blanc
        bool trobat = true;
        while (trobat)
        {
            trobat = false;
            for (int x = 1; x < matrixOcupation.GetLength(0) - 1; x++)
                for (int z = 1; z < matrixOcupation.GetLength(1) - 1; z++)
                {

                    //si es un cas de forat
                    if (!matrixOcupation[x, z])
                    {
                        int n = 0;
                        n += System.Convert.ToInt32(matrixOcupation[x - 1, z]);
                        n += System.Convert.ToInt32(matrixOcupation[x + 1, z]);
                        n += System.Convert.ToInt32(matrixOcupation[x, z - 1]);
                        n += System.Convert.ToInt32(matrixOcupation[x, z + 1]);

                        if (n >= 3)
                        {
                            occupyPosition(validPositions, new Vector2(x, z));
                            trobat = true;
                        }
                    }
                }

        };
    }

    void debugMatrix()
    {
        string s = "";
        for (int z = 0; z < matrixOcupation.GetLength(0); z++)
        {

            for (int x = 0; x < matrixOcupation.GetLength(1); x++)
            {
                s += matrixOcupation[x, z] ? "█" : "░";
            }
            s += "\n";
        }
        Debug.Log(s);
    }


    private void generateGeometry()
    {
        gridMesh = new Mesh();
        gridMesh.name = "GridMesh";

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        int vertexCount = 0;
        for (int x = 0; x < matrixOcupation.GetLength(0); x++)
            for (int z = 0; z < matrixOcupation.GetLength(1); z++)
                generateMatrixCell(vertices, triangles, x, z, ref vertexCount);

        gridMesh.SetVertices(vertices);
        gridMesh.SetTriangles(triangles, 0, true);
        gridMesh.RecalculateNormals();
    }


    private void generateMatrixCell(List<Vector3> vertices, List<int> triangles, int x, int z, ref int vertexCount)
    {
        if (matrixOcupation[x, z])
        {
            Vector3[] face;

            face = new Vector3[4];

            face[0] = getNorthWestAtFloorLevel(x, z);
            face[1] = getNorthEastAtFloorLevel(x, z);
            face[2] = getSouthWestAtFloorLevel(x, z);
            face[3] = getSouthEastAtFloorLevel(x, z);
            generateSquareFace(face, vertices, triangles, ref vertexCount);

            face[0] = getNorthEastAtPitLevel(x, z);
            face[1] = getNorthWestAtPitLevel(x, z);
            face[2] = getSouthEastAtPitLevel(x, z);
            face[3] = getSouthWestAtPitLevel(x, z);
            generateSquareFace(face, vertices, triangles, ref vertexCount);

            bool isTopEdge = z > 0 && !matrixOcupation[x, z - 1];
            bool isBottomEdge = z < matrixOcupation.GetLength(1) - 1 && !matrixOcupation[x, z + 1];
            bool isLeftEdge = x > 0 && !matrixOcupation[x - 1, z];
            bool isRightEdge = x < matrixOcupation.GetLength(0) - 1 && !matrixOcupation[x + 1, z];



            if (isTopEdge)
            {
                face[0] = getNorthEastAtFloorLevel(x, z);
                face[1] = getNorthWestAtFloorLevel(x, z);
                face[2] = getNorthEastAtPitLevel(x, z);
                face[3] = getNorthWestAtPitLevel(x, z);

                generateSquareFace(face, vertices, triangles, ref vertexCount);

            }

            if (isBottomEdge)
            {
                face[0] = getSouthWestAtFloorLevel(x, z);
                face[1] = getSouthEastAtFloorLevel(x, z);
                face[2] = getSouthWestAtPitLevel(x, z);
                face[3] = getSouthEastAtPitLevel(x, z);

                generateSquareFace(face, vertices, triangles, ref vertexCount);
            }

            if (isLeftEdge)
            {
                face[0] = getNorthWestAtFloorLevel(x, z);
                face[1] = getSouthWestAtFloorLevel(x, z);
                face[2] = getNorthWestAtPitLevel(x, z);
                face[3] = getSouthWestAtPitLevel(x, z);
                generateSquareFace(face, vertices, triangles, ref vertexCount);
            }

            if (isRightEdge)
            {
                face[0] = getSouthEastAtFloorLevel(x, z);
                face[1] = getNorthEastAtFloorLevel(x, z);
                face[2] = getSouthEastAtPitLevel(x, z);
                face[3] = getNorthEastAtPitLevel(x, z);
                generateSquareFace(face, vertices, triangles, ref vertexCount);
            }
        }
    }

    private void generateSquareFace(Vector3[] faceVertices, List<Vector3> vertices, List<int> triangles, ref int vertexCount)
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
        {
            vertices.Add(vertex);
        }
        vertexCount += 4;
    }

    private void addSquareTriangles(List<int> triangles, Dictionary<Vector3, int> vertexIndex, int x, int z)
    {
        if (matrixOcupation[x, z])
        {
            Vector3 squareCenter = getCenterAtMatrixPosition(x, z);
            int floorTopLeftIndex = vertexIndex[squareCenter - right + up];
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

            bool isTopEdge = z > 0 && !matrixOcupation[x, z - 1];
            bool isBottomEdge = z < matrixOcupation.GetLength(1) - 1 && !matrixOcupation[x, z + 1];
            bool isLeftEdge = x > 0 && !matrixOcupation[x - 1, z];
            bool isRightEdge = x < matrixOcupation.GetLength(0) - 1 && !matrixOcupation[x + 1, z];

            int bottom1, bottom2;
            if (isTopEdge)
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

            if (isBottomEdge)
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

            if (isLeftEdge)
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

            if (isRightEdge)
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

    private void addSquareVertices(List<Vector3> vertices, Dictionary<Vector3, int> verticesIndex, int x, int z)
    {
        if (matrixOcupation[x, z])
        {
            Vector3 squareCenter = getCenterAtMatrixPosition(x, z);
            List<Vector3> squareVertices = new List<Vector3>()
            {
                squareCenter - right + up,
                squareCenter + right + up,
                squareCenter - right - up,
                squareCenter + right - up
            };



            bool isTopEdge = z > 0 && !matrixOcupation[x, z - 1];
            bool isBottomEdge = z < matrixOcupation.GetLength(1) - 1 && !matrixOcupation[x, z + 1];
            bool isLeftEdge = x > 0 && !matrixOcupation[x - 1, z];
            bool isRightEdge = x < matrixOcupation.GetLength(0) - 1 && !matrixOcupation[x + 1, z];

            if (isTopEdge)
            {
                squareVertices.Add(squareVertices[0] - height);
                squareVertices.Add(squareVertices[1] - height);
            }

            if (isBottomEdge)
            {
                squareVertices.Add(squareVertices[2] - height);
                squareVertices.Add(squareVertices[3] - height);
            }

            if (isLeftEdge)
            {
                squareVertices.Add(squareVertices[0] - height);
                squareVertices.Add(squareVertices[2] - height);
            }

            if (isRightEdge)
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


    Vector3 getCenterAtMatrixPosition(int x, int z)
    {
        Vector3 position = new Vector3(x - numberOfCellsToFill - 1, 0, -z + numberOfCellsToFill + 1);
        position *= cellSize;
        return position;
    }

    // Floor Level
    Vector3 getNorthWestAtFloorLevel(int x, int z)
    {
        return getCenterAtMatrixPosition(x, z) + up - right;
    }

    Vector3 getNorthEastAtFloorLevel(int x, int z)
    {
        return getCenterAtMatrixPosition(x, z) + up + right;
    }

    Vector3 getSouthWestAtFloorLevel(int x, int z)
    {
        return getCenterAtMatrixPosition(x, z) - up - right;
    }

    Vector3 getSouthEastAtFloorLevel(int x, int z)
    {
        return getCenterAtMatrixPosition(x, z) - up + right;
    }

    // pit level
    Vector3 getNorthWestAtPitLevel(int x, int z)
    {
        return getCenterAtMatrixPosition(x, z) + up - right - height;
    }

    Vector3 getNorthEastAtPitLevel(int x, int z)
    {
        return getCenterAtMatrixPosition(x, z) + up + right - height;
    }

    Vector3 getSouthWestAtPitLevel(int x, int z)
    {
        return getCenterAtMatrixPosition(x, z) - up - right - height;
    }

    Vector3 getSouthEastAtPitLevel(int x, int z)
    {
        return getCenterAtMatrixPosition(x, z) - up + right - height;
    }

    private void occupyPosition(List<Vector2> validPositions, Vector2 position)
    {
        removeValidPositions(validPositions, position);
        matrixOcupation[(int)position.x, (int)position.y] = true;
        addValidAdjacentPositions(validPositions, position);
    }

    private Vector2 getRandomValidPosition(List<Vector2> validPositions)
    {
        return validPositions[Random.Range(0, validPositions.Count)];
    }

    private void removeValidPositions(List<Vector2> validPositions, Vector2 position)
    {
        validPositions.RemoveAll(x => x.Equals(position));
    }

    private void addValidAdjacentPositions(List<Vector2> validPositions, Vector2 position)
    {
        addValidPosition(validPositions, position + Vector2.up);
        addValidPosition(validPositions, position + Vector2.down);
        addValidPosition(validPositions, position + Vector2.left);
        addValidPosition(validPositions, position + Vector2.right);
    }

    private void addValidPosition(List<Vector2> validPositions, Vector2 newPosition)
    {
        if (isValid(newPosition) && !matrixOcupation[(int)newPosition.x, (int)newPosition.y])
            if (!closePiecesHavePriority && !validPositions.Contains(newPosition) || closePiecesHavePriority)
                validPositions.Add(newPosition);
    }

    private bool isValid(Vector2 position)
    {
        return position.x >= 0 && position.x < numberOfCellsToFill * 2 + 1 && position.y >= 0 && position.y < numberOfCellsToFill * 2 + 1;
    }
}
 
 