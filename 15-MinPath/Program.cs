public class Day15
{
    private int[,] data;
    private int Height { get; init; }
    private int Width { get; init; }
    private bool[,] visited;
    private int[,] dist;
    private int currentRow;
    private int currentCol;

    public static void Main()
    {
        string[] dataRaw = File.ReadAllLines("data.txt");

        Day15 day15 = new Day15(dataRaw);
        int result = day15.Part1();
        Console.WriteLine(result);
        //PrintMatrix(data);
    }

    public Day15(string[] dataRaw)
    {
        // Let's assume all rows are equal length. 
        Height = dataRaw.Length;
        Width = dataRaw.Max(x => x.Length);
        this.data = new int[Height, Width];
        this.visited = new bool[Height, Width];
        for (int i = 0; i < Height; i++)
            for (int j = 0; j < Width; j++)
                data[i, j] = int.Parse(dataRaw[i][j] + "");
    }

    public int Part1()
    {
        currentCol = 0;
        currentRow = 0;
        int minDistance = Dijkstra(currentRow, currentCol);

        return minDistance;
    }

    private int Dijkstra(int currRow, int currCol)
    {
        //bool top = (currRow - 1 < 0) ? false : true;
        //bool bottom = currRow + 1 > this.data.GetLength(0) - 1 ? false : true;
        //bool left = currCol - 1 < 0 ? false : true;
        //bool right = currCol + 1 > this.data.GetLength(1) - 1 ? false : true;
        //if (top)
        //    if (!visited[currRow - 1, currRow])
        //        if (dist[currRow, currCol] + 1 < dist[currRow - 1, currCol])
        //            dist[currRow - 1, currCol] = dist[currRow, currCol] + 1;

        //for (double d = 0; d <= 3 * Math.PI / 4; d += Math.PI / 2)
        //{

        //}

        int minRow = -1;
        int minCol = -1;
        int minNeighbourDist = int.MaxValue;
        for (int i = 0; i <= 3; i++)
        {
            int rowOffset = (int)Math.Cos(i * Math.PI / 2);
            int colOffset = (int)Math.Sin(i * Math.PI / 2);
            if (currentRow + rowOffset < 0 || currentRow + rowOffset > Height - 1) continue;
            if (currentCol + colOffset < 0 || currentCol + colOffset > Width - 1) continue;
            if (visited[currRow + rowOffset, currCol + colOffset]) continue;
            if (data[currRow + rowOffset, currCol + colOffset] < minNeighbourDist) { minRow = currRow + rowOffset; minCol = currCol + colOffset; }
        }

        Console.WriteLine("===");

        throw new NotImplementedException();
    }
}