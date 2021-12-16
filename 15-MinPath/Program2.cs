public class Day15_2
{
    //public static void Main()
    //{
    //    string[] dataRaw = File.ReadAllLines("data.txt");

    //    // Let's assume all rows are equal length. 
    //    int[,] data = new int[dataRaw.Length, dataRaw.Max(x => x.Length)];
    //    for (int i = 0; i < dataRaw.Length; i++)
    //        for (int j = 0; j < dataRaw[i].Length; j++)
    //            data[i, j] = int.Parse(dataRaw[i][j] + "");

    //    int min = Part1(data) - data[0, 0];
    //    Console.WriteLine(min);
    //    //PrintMatrix(data);
    //}



    //(int, int) ClosestNeighbour(int currRow, int currCol)
    //{
    //    int minRow = -1;
    //    int minCol = -1;
    //    int minNeighbourDist = int.MaxValue;
    //    for (int i = 0; i <= 3; i++)
    //    {
    //        int rowOffset = (int)Math.Cos(i * Math.PI / 2);
    //        int colOffset = (int)Math.Sin(i * Math.PI / 2);
    //        if (currRow + rowOffset < 0 || currRow + rowOffset > Height - 1) continue;
    //        if (currCol + colOffset < 0 || currCol + colOffset > Width - 1) continue;
    //        if (visited[currRow + rowOffset, currCol + colOffset]) continue;
    //        if (dist[currRow + rowOffset, currCol + colOffset] < minNeighbourDist)
    //        {
    //            minNeighbourDist = dist[currRow, currCol] + data[currRow + rowOffset, currCol + colOffset];
    //            minRow = currRow + rowOffset;
    //            minCol = currCol + colOffset;
    //        }
    //    }
    //    if (minRow == -1)
    //    {
    //        PrintMatrix(data);
    //        Console.WriteLine("===");
    //        PrintMatrix(dist);
    //        Console.WriteLine("===");
    //        //PrintMatrix(visited);
    //    }
    //    return (minRow, minCol);
    //}



    private static int Part1(int[,] data)
    {
        int[,] costs = Costs(data);

        //int minCost = MinCost(data, costs, 0, 0);

        return costs[costs.GetLength(0) - 1, costs.GetLength(1) - 1];
    }

    private static int MinCost(int[,] data, int[,] costs, int row, int col)
    {
        int min = 0;
        int i = 0;
        int j = 0;
        while (i < data.GetLength(0) && j < data.GetLength(1))
        {
            min += data[i, j];
            if (costs[Math.Min(row + 1, data.GetLength(0)), col] <= costs[row, Math.Min(col + 1, data.GetLength(1))]) i++;
            else j++;
        }
        //min += costs[i, j];
        return min;
        //if (row == data.GetLength(0) - 1 && col == data.GetLength(1) - 1) return data[row, col] + costs[row, col];
        //return data[row, col] + Math.Min(costs[Math.Max(row - 1, 0), col], costs[row, Math.Max(col - 1, 0)]);
    }

    public static int[,] Costs(int[,] data)
    {
        int[,] costs = new int[data.GetLength(0), data.GetLength(1)];
        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                costs[i, j] = Cost(data, costs, i, j);
            }
        }
        //PrintMatrix(costs);
        return costs;
    }

    public static int Cost(int[,] data, int[,] costs, int row, int col)
    {
        // Start / top left
        if (row == 0 && col == 0) return data[row, col];
        // End / bottom right
        if (row == data.GetLength(0) - 1 && col == data.GetLength(1) - 1) return data[row, col] + Math.Min(costs[row, col - 1], costs[row - 1, col]);
        if (row == 0) return data[row, col] + costs[row, col - 1];
        if (col == 0) return data[row, col] + costs[row - 1, col];
        return data[row, col] + Math.Min(costs[row, col - 1], costs[row - 1, col]);
    }

    public static void PrintMatrix(int[,] arr)
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