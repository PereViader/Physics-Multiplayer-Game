using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class MapMatrixGenerator {

    [SerializeField]
    private int minNumberOfCells;

    [SerializeField]
    private bool closeCellsHavePriority;


    public bool[,] GenerateMatrix(int seed)
    {
        UnityEngine.Random.InitState(seed);
        return generateMatrix();
    }

    private bool[,] generateMatrix()
    {
        if (minNumberOfCells <= 0)
            Debug.LogError("No pot ser que map matrix generator sigui: " + minNumberOfCells + " hauria de ser > 0");
        bool[,] matrixOcupation = new bool[minNumberOfCells * 2 + 1, minNumberOfCells * 2 + 1];
        List<Vector2> validPositions = new List<Vector2>();
        // com que hem fet un array de mapSize*2 +1 la casella del mig queda a la posicio escrita
        Vector2 startingPosition = new Vector2(minNumberOfCells + 1, minNumberOfCells + 1);
        validPositions.Add(startingPosition);

        // generem les peces
        for (int piece = 0; piece < minNumberOfCells; piece++)
        {
            if (validPositions.Count > 0)
            {
                Vector2 matrixPosition = getRandomValidPosition(validPositions);
                occupyPosition(validPositions, matrixOcupation, matrixPosition);
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
                            occupyPosition(validPositions, matrixOcupation, new Vector2(x, z));
                            trobat = true;
                        }
                    }
                }

        };
        return trimMatrix(matrixOcupation);
    }

    /*Vector2 calculateMatrixCenter(bool[,] matrixOcupation)
    {
        
        return new Vector2(maxx - sizex / 2, maxy - sizey / 2);
    }*/

    bool[,] trimMatrix(bool[,] matrix)
    {
        int minx = matrix.GetLength(0);
        int miny = matrix.GetLength(1);

        int maxx = 0;
        int maxy = 0;

        for (int x = 0; x < matrix.GetLength(0); x++)
            for (int y = 0; y < matrix.GetLength(1); y++)
                if (matrix[x, y])
                {
                    if (minx > x) minx = x;
                    if (miny > y) miny = y;
                    if (maxx < x) maxx = x;
                    if (maxy < y) maxy = y;
                }

        // calculate y 
        int sizex = maxx - minx +1;
        int sizey = maxy - miny +1;
        bool[,] trimmedMatrix = new bool[sizex, sizey];

        for (int x = 0; x <= maxx-minx; x++)
            for (int y = 0; y <= maxy-miny; y++)
                trimmedMatrix[x,y] = matrix[x+minx, y+miny];
        return trimmedMatrix;
    }


    void debugMatrix(bool[,] matrix)
    {
        string s = "";
        for (int z = 0; z < matrix.GetLength(1); z++)
        {

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                s += matrix[x, z] ? "█" : "░";
            }
            s += "\n";
        }
        Debug.Log(s);
    }

    private Vector2 getRandomValidPosition(List<Vector2> validPositions)
    {
        return validPositions[UnityEngine.Random.Range(0, validPositions.Count)];
    }

    private void occupyPosition(List<Vector2> validPositions, bool[,] matrixOcupation, Vector2 position)
    {
        removeValidPositions(validPositions, position);
        matrixOcupation[(int)position.x, (int)position.y] = true;
        addValidAdjacentPositions(validPositions, matrixOcupation, position);
    }

    private void removeValidPositions(List<Vector2> validPositions, Vector2 position)
    {
        validPositions.RemoveAll(x => x.Equals(position));
    }

    private void addValidAdjacentPositions(List<Vector2> validPositions, bool[,] matrixOcupation, Vector2 position)
    {
        addValidPosition(validPositions, matrixOcupation, position + Vector2.up);
        addValidPosition(validPositions, matrixOcupation, position + Vector2.down);
        addValidPosition(validPositions, matrixOcupation, position + Vector2.left);
        addValidPosition(validPositions, matrixOcupation, position + Vector2.right);
    }

    private void addValidPosition(List<Vector2> validPositions, bool[,] matrixOcupation, Vector2 newPosition)
    {
        if (isValid(newPosition) && !matrixOcupation[(int)newPosition.x, (int)newPosition.y])
            if (!closeCellsHavePriority && !validPositions.Contains(newPosition) || closeCellsHavePriority)
                validPositions.Add(newPosition);
    }

    private bool isValid(Vector2 position)
    {
        return position.x >= 0 && position.x < minNumberOfCells * 2 + 1 && position.y >= 0 && position.y < minNumberOfCells * 2 + 1;
    }
}
