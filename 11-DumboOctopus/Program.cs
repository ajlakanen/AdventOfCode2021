public class Day11
{
    public static void Main()
    {
        string[] dataRaw = File.ReadAllLines("data.txt");
        
        // Let's assume all rows are equal length. 
        int[,] data = new int[dataRaw.Length, dataRaw.Max(x => x.Length)];
        for (int i = 0; i < dataRaw.Length; i++)
            for (int j = 0; j < dataRaw[i].Length; j++)
                data[i, j] = int.Parse(dataRaw[i][j] + "");

        int flashes = Part1(data, 100);
        int count = Part2(data);
    }

    private static int Part2(int[,] data)
    {
        int count = 0;
        while (true)
        {
            data = NewGeneration(data);

            for (int i = 0; i < data.GetLength(0); i++)
                for (int j = 0; j < data.GetLength(1); j++)
                    if (data[i, j] > 9) { data[i, j] = 0; }

            if ((from int val in data select val).Sum() == 0) return ++count;
            else count++;
        }
    }

    private static int[,] NewGeneration(int[,] array)
    {
        int height = array.GetLength(0);
        int width = array.GetLength(1);

        int[,] flashed = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                array[i, j]++;
                if (array[i, j] > 9)
                {
                    flashed[i, j]++;
                    Adjacents(i, j, flashed);
                }
            }
        }

        // I couldn't do this recursively :(
        bool foundNonFlashed = true;
        while (foundNonFlashed)
        {
            foundNonFlashed = false;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (array[i, j] <= 9) continue;
                    if (flashed[i, j] > 0) continue;
                    foundNonFlashed = true;
                    Adjacents(i, j, flashed);
                }
            }
        }

        return array;

        // Make adjacent cells flash
        void Adjacents(int i, int j, int[,] flashed)
        {
            for (int adj_row = Math.Max(0, i - 1); adj_row <= Math.Min(i + 1, height - 1); adj_row++)
            {
                for (int adj_col = Math.Max(0, j - 1); adj_col <= Math.Min(j + 1, width - 1); adj_col++)
                {
                    if (adj_row == i && adj_col == j) { flashed[adj_row, adj_col]++; continue; }
                    else array[adj_row, adj_col]++;
                }
            }
        }
    }

    private static int Part1(int[,] arrayOrig, int stepsMax)
    {
        int height = arrayOrig.GetLength(0);
        int width = arrayOrig.GetLength(1);
        int[,] array = new int[height, width];
        
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                array[i, j] = arrayOrig[i, j];

        int flashes = 0;

        for (int steps = 0; steps < stepsMax; steps++)
        {
            array = NewGeneration(array);

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    if (array[i, j] > 9) { flashes++; array[i, j] = 0; }

        }
        return flashes;
    }


    public static void PrintMatrix(int[,] arr)
    {
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                Console.Write(string.Format("{0} ", arr[i, j]));
            }
            Console.Write(Environment.NewLine);
        }
    }
}

