using System;
using System.Collections.Generic;

class Graph
{
    public Dictionary<int, List<int>> AdjacencyList { get; set; }

    public Graph()
    {
        AdjacencyList = new Dictionary<int, List<int>>();
    }

    public void AddEdge(int v1, int v2)
    {
        if (!AdjacencyList.ContainsKey(v1))
            AdjacencyList[v1] = new List<int>();
        if (!AdjacencyList.ContainsKey(v2))
            AdjacencyList[v2] = new List<int>();

        AdjacencyList[v1].Add(v2);
        AdjacencyList[v2].Add(v1);
    }

    public void DisplayAdjacencyList()
    {
        foreach (var vertex in AdjacencyList)
        {
            Console.Write(vertex.Key + ": ");
            foreach (var edge in vertex.Value)
            {
                Console.Write(edge + " ");
            }
            Console.WriteLine();
        }
    }
}
class GraphMatrix
{
    private int[,] adjacencyMatrix;
    private int size;

    public GraphMatrix(int size)
    {
        this.size = size;
        adjacencyMatrix = new int[size, size];
    }

    public void AddEdge(int v1, int v2)
    {
        adjacencyMatrix[v1 - 1, v2 - 1] = 1;
        adjacencyMatrix[v2 - 1, v1 - 1] = 1;
    }

    public void DisplayAdjacencyMatrix()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Console.Write(adjacencyMatrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}

class GraphIncidenceMatrix
{
    private int[,] incidenceMatrix;
    private int vertexCount, edgeCount;
    private int edgeIndex;

    public GraphIncidenceMatrix(int vertexCount, int edgeCount)
    {
        this.vertexCount = vertexCount;
        this.edgeCount = edgeCount;
        incidenceMatrix = new int[vertexCount, edgeCount];
        edgeIndex = 0;
    }

    public void AddEdge(int v1, int v2)
    {
        incidenceMatrix[v1 - 1, edgeIndex] = 1;
        incidenceMatrix[v2 - 1, edgeIndex] = 1;
        edgeIndex++;
    }

    public void DisplayIncidenceMatrix()
    {
        for (int i = 0; i < vertexCount; i++)
        {
            for (int j = 0; j < edgeCount; j++)
            {
                Console.Write(incidenceMatrix[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}

class GraphColoring
{
    public int[] ColorGraph(Dictionary<int, List<int>> graph)
    {
        int vertexCount = graph.Keys.Count;
        int[] result = new int[vertexCount];

        for (int i = 0; i < vertexCount; i++)
            result[i] = -1;

        result[0] = 0;

        bool[] available = new bool[vertexCount];

        for (int i = 1; i < vertexCount; i++)
        {
            foreach (int neighbor in graph[i + 1])
            {
                if (result[neighbor - 1] != -1)
                    available[result[neighbor - 1]] = true;
            }

            int color;
            for (color = 0; color < vertexCount; color++)
            {
                if (!available[color])
                    break;
            }

            result[i] = color;

            foreach (int neighbor in graph[i + 1])
            {
                if (result[neighbor - 1] != -1)
                    available[result[neighbor - 1]] = false;
            }
        }
        return result;
    }
}

class GraphColoringMatrix
{
    public int[] ColorGraph(int[,] graph)
    {
        int vertexCount = graph.GetLength(0);
        int[] result = new int[vertexCount];

        for (int i = 0; i < vertexCount; i++)
            result[i] = -1;

        result[0] = 0;

        bool[] available = new bool[vertexCount];

        for (int i = 1; i < vertexCount; i++)
        {
            for (int j = 0; j < vertexCount; j++)
            {
                if (graph[i, j] == 1 && result[j] != -1)
                    available[result[j]] = true;
            }

            int color;
            for (color = 0; color < vertexCount; color++)
            {
                if (!available[color])
                    break;
            }

            result[i] = color;

            for (int j = 0; j < vertexCount; j++)
            {
                if (graph[i, j] == 1 && result[j] != -1)
                    available[result[j]] = false;
            }
        }
        return result;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Структура смежности
        Graph graph = new Graph();
        int[][] edges = new int[][]
        {
            new int[] {1, 2},
            new int[] {1, 5},
            new int[] {2, 5},
            new int[] {3, 4},
            new int[] {3, 5},
            new int[] {3, 7},
            new int[] {4, 5},
            new int[] {5, 6},
            new int[] {6, 7}
        };

        foreach (var edge in edges)
        {
            graph.AddEdge(edge[0], edge[1]);
        }

        Console.WriteLine("Структура смежности:");
        graph.DisplayAdjacencyList();

        // Матрица смежности
        GraphMatrix matrix = new GraphMatrix(7);
        foreach (var edge in edges)
        {
            matrix.AddEdge(edge[0], edge[1]);
        }

        Console.WriteLine("Матрица смежности:");
        matrix.DisplayAdjacencyMatrix();

        // Матрица инцидентности
        GraphIncidenceMatrix incidenceMatrix = new GraphIncidenceMatrix(7, edges.Length);
        foreach (var edge in edges)
        {
            incidenceMatrix.AddEdge(edge[0], edge[1]);
        }

        Console.WriteLine("Матрица инцидентности:");
        incidenceMatrix.DisplayIncidenceMatrix();

        // "Жадный" алгоритм раскраски
        GraphColoring coloring = new GraphColoring();
        int[] colors = coloring.ColorGraph(graph.AdjacencyList);

        Console.WriteLine("Раскраска вершин (жадный алгоритм):");
        for (int i = 0; i < colors.Length; i++)
        {
            Console.WriteLine($"Вершина {i + 1} -> Цвет {colors[i]}");
        }

        // Алгоритм раскраски на основе матрицы смежности
        GraphColoringMatrix matrixColoring = new GraphColoringMatrix();
        int[,] adjMatrix = new int[7, 7];
        foreach (var edge in edges)
        {
            adjMatrix[edge[0] - 1, edge[1] - 1] = 1;
            adjMatrix[edge[1] - 1, edge[0] - 1] = 1;
        }

        int[] matrixColors = matrixColoring.ColorGraph(adjMatrix);

        Console.WriteLine("Раскраска вершин (алгоритм на основе матрицы смежности):");
        for (int i = 0; i < matrixColors.Length; i++)
        {
            Console.WriteLine($"Вершина {i + 1} -> Цвет {matrixColors[i]}");
        }
    }
}
