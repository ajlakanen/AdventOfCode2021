public class Day15
{
    private int[,] data;
    private bool[,] visited;
    private int[,] dist;

    /// <summary>
    /// Data height
    /// </summary>
    private int Height { get; init; }
    
    /// <summary>
    /// Data width
    /// </summary>
    private int Width { get; init; }
    
    
    // Offsets for east, south, west, north positions.
    static int[] RowOffsets = { 0, 1, 0, -1 };
    static int[] ColOffsets = { 1, 0, -1, 0 };

    /// <summary>
    /// Destination node row
    /// </summary>
    private int TargetRow { get; init; }

    /// <summary>
    /// Destination node column
    /// </summary>
    private int TargetCol { get; init; }

    public static void Main()
    {
        string[] dataRaw = File.ReadAllLines("data.txt");

        Day15 day15 = new Day15(dataRaw);
        int result = day15.Part1();
        Console.WriteLine(result);
    }

    public Day15(string[] dataRaw)
    {
        // Let's assume all rows are equal length. 
        Height = dataRaw.Length;
        Width = dataRaw.Max(x => x.Length);
        TargetRow = Height - 1;
        TargetCol = Width - 1;
        this.data = new int[Height, Width];
        this.dist = new int[Height, Width];
        this.visited = new bool[Height, Width];
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                data[i, j] = int.Parse(dataRaw[i][j] + "");
                dist[i, j] = 9999;
            }
        }
    }

    public int Part1()
    {
        dist[0, 0] = 0;
        int minDistance = Dijkstra(0, 0);
        return minDistance;
    }

    private int Dijkstra(int currRow, int currCol)
    {
        while (!visited[TargetRow, TargetCol])
        {
            for (int i = 0; i <= 3; i++)
            {
                // This was a brilliant idea which was later found stupid :-)
                // int rowOffset = (int)Math.Cos(i * Math.PI / 2);
                // int colOffset = (int)Math.Sin(i * Math.PI / 2);

                int rowOffset = RowOffsets[i];
                int colOffset = ColOffsets[i];
                if (currRow + rowOffset < 0 || currRow + rowOffset > Height - 1) continue;
                if (currCol + colOffset < 0 || currCol + colOffset > Width - 1) continue;
                if (visited[currRow + rowOffset, currCol + colOffset]) continue;
                if (dist[currRow, currCol] + data[currRow + rowOffset, currCol + colOffset] < dist[currRow + rowOffset, currCol + colOffset])
                {
                    dist[currRow + rowOffset, currCol + colOffset] = dist[currRow, currCol] + data[currRow + rowOffset, currCol + colOffset];
                }
            }

            visited[currRow, currCol] = true;

            (int closestRow, int closestCol) = ClosestUnvisited();
            currRow = closestRow;
            currCol = closestCol;
        }
        return dist[TargetRow, TargetCol];
    }

    /// <summary>
    /// Unvisited node closest to the starting point.
    /// C# can't do linq on 2d arrays :-(.
    /// </summary>
    /// <returns>Closest (row, col)</returns>
    (int, int) ClosestUnvisited()
    {
        int minRow = -1;
        int minCol = -1;
        int minNeighbourDist = int.MaxValue;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (visited[i, j]) continue;
                if (dist[i, j] < minNeighbourDist)
                {
                    minRow = i;
                    minCol = j;
                    minNeighbourDist = dist[i, j];
                }
            }
        }
        return (minRow, minCol);
    }

    public static void PrintMatrix<T>(T[,] arr)
    {
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                Console.Write(string.Format("{0,3} ", arr[i, j]));
            }
            Console.Write(Environment.NewLine);
        }
    }
}