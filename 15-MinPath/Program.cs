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
        day15 = new Day15(dataRaw, 5);
        result = day15.Part1();
        Console.WriteLine(result);

    }

    public Day15(string[] dataRaw, int factor = 1)
    {
        // Let's assume all rows are equal length. 
        Height = dataRaw.Length * factor;
        Width = dataRaw.Max(x => x.Length) * factor;
        TargetRow = Height - 1;
        TargetCol = Width - 1;
        this.data = new int[Height, Width];
        this.dist = new int[Height, Width];
        this.visited = new bool[Height, Width];
        for (int i = 0; i < Height / factor; i++)
        {
            for (int j = 0; j < Width / factor; j++)
            {
                data[i, j] = int.Parse(dataRaw[i][j] + "");
            }
        }

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j< Width; j++)
            {
                dist[i, j] = 99999;
            }
        }

        if (factor == 1) return;

        //for (int i = 0; i < Height / factor; i++)
        //{
        //    for (int j = 0; j < Width / factor; j++)
        //    {
        //        for (int k = 0; k < 5; k++)
        //        {
        //            for (int l = 0; l < 5; l++)
        //            {
        //                if (i + k * Height / factor < 10 && j + l * Width / factor < 10) continue;
        //                data[i + k * Height / factor, j + l * Width / factor] = (data[i, j]+ i + k * Height / factor + j + l * Width / factor)%10;
        //            }
        //        }
        //    }
        //}

        //int[,] test = new int[5, 5];
        //test[0, 0] = 8;

        //for (int i = 0; i < 1; i++)
        //{
        //    for (int j = 1; j < 5; j++)
        //    {
        //        if (i == 0 && j == 0) continue;
        //        test[i, j] = test[i, j - 1] + 1;
        //        if (test[i, j] > 9) test[i, j] = 1;
        //    }
        //}

        for (int i = 0; i < Height/factor; i++)
        {
            for (int j = Width/factor; j < Width; j++)
            {
                data[i, j] = data[i, j - Width / factor] + 1;
                if (data[i, j] > 9) data[i, j] = 1;
            }
        }

        for (int i = Height/factor; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                data[i, j] = data[i - Width/factor, j] + 1;
                if (data[i, j] > 9) data[i, j] = 1;
            }
        }

        //PrintMatrix(data);
        //PrintMatrix(data);
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