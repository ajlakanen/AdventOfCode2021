public class Day9
{
    public static void Main()
    {
        string[] dataRaw = File.ReadAllLines("data.txt");
        int[,] points = new int[dataRaw.Length, dataRaw.Max(x => x.Length)];
        for (int i = 0; i < dataRaw.Length; i++)
            for (int j = 0; j < points.GetLength(1); j++)
                points[i, j] = int.Parse(dataRaw[i][j] + "");

        int sum1 = Part1(points);
        int sum2 = Part2(points);
    }

    private static int Part2(int[,] points)
    {
        int height = points.GetLength(0);
        int width = points.GetLength(1);
        int[,] basins = new int[height, width];
        int[,] counted = new int[height, width];
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                if (points[i, j] != 9) basins[i, j] = 1;

        List<int> basinSums = new List<int>();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int sum = BasinSum(i, j);
                if (sum > 0) basinSums.Add(sum);
            }
        }

        int BasinSum(int row, int col)
        {
            if (row < 0 || col < 0) return 0;
            if (row > height - 1 || col > width - 1) return 0;
            if (basins[row, col] == 0) return 0;
            if (counted[row, col] == 1) return 0;
            counted[row, col] = 1;
            return basins[row, col] + BasinSum(row - 1, col) + BasinSum(row, col - 1) + BasinSum(row + 1, col) + BasinSum(row, col + 1);
        }

        return basinSums.OrderByDescending(x => x).Take(3).Aggregate((prev, next) => prev * next);
    }


    private static int Part1(int[,] points)
    {
        int lowSum = 0;
        int height = points.GetLength(0);
        int width = points.GetLength(1);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int curr = points[i, j];
                int top = i - 1 < 0 ? int.MaxValue : points[i - 1, j];
                int left = j - 1 < 0 ? int.MaxValue : points[i, j - 1];
                int right = j + 1 >= width ? int.MaxValue : points[i, j + 1];
                int bottom = i + 1 >= height ? int.MaxValue : points[i + 1, j];
                if (new[] { top, left, right, bottom }.All(x => x > curr)) lowSum += curr + 1;
            }
        }
        return lowSum;
    }

    public static void PrintMatrix(int[,] arr)
    {
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                Console.Write(string.Format("{0} ", arr[i, j]));
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }
}