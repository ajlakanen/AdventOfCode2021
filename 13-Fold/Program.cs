public class Day13
{
    private List<(int x, int y)> coords;
    private List<(bool vertical, int foldRow)> foldInstructions;

    public static void Main()
    {
        string[][] splitted = Array.ConvertAll(File.ReadAllLines("data.txt"), x => x.Split(new[] { ',', '=' }));

        Day13 day13 = new Day13(splitted);
        int sum = day13.Solve(1);
        Console.WriteLine($"Part 1: {sum}");
        day13.Solve(2);

    }

    /// <summary>
    /// Prepare the data.
    /// </summary>
    /// <param name="rawData"></param>
    public Day13(string[][] rawData)
    {
        coords = new List<(int x, int y)>();
        foldInstructions = new List<(bool, int)>();
        int i = 0;
        while (rawData[i].Length > 1)
        {
            coords.Add(
                (int.Parse(rawData[i][0]), int.Parse(rawData[i][1]))
                );
            i++;
        }
        i = rawData.Length - 1;
        while (rawData[i].Length > 1)
        {
            bool vertical = (rawData[i][0].Last() == 'x') ? true : false;
            foldInstructions.Add(
                (vertical, int.Parse(rawData[i][1]))
                );
            i--;
        }
        foldInstructions.Reverse();
    }

    private int Solve(int length = 1)
    {
        int maxX = coords.Max(x => x.x);
        int maxY = coords.Max(x => x.y);

        int[,] data = new int[maxY + 1, maxX + 1];
        foreach (var item in coords) data[item.y, item.x] = 1;

        int height = data.GetLength(0); // Just to make some code more readable. 
        int width = data.GetLength(1);
        int i = 0;
        if (length == 2) length = foldInstructions.Count;
        while (i < length)
        {   // This is ugly :(
            if (foldInstructions[i].vertical)
            {
                for (int j = 0; j < height; j++)
                    for (int k = 0, l = width - 1; k < foldInstructions[i].foldRow; k++, l--)
                        data[j, k] += data[j, l];
            }
            else
            {
                for (int j = 0; j < width; j++)
                    for (int k = 0, l = height - 1; k < foldInstructions[i].foldRow; k++, l--)
                        data[k, j] += data[l, j];
            }
            height = foldInstructions[i].vertical ? height : foldInstructions[i].foldRow;
            width = foldInstructions[i].vertical ? foldInstructions[i].foldRow : width;
            i++;
        }

        int count = 0;
        for (int row = 0; row < height; row++)
            for (int col = 0; col < width; col++)
                if (data[row, col] != 0) count++;
        if (length == 1) return count; // Part 1

        char[,] finalData = new char[height, width];
        for (int row = 0; row < height; row++)
            for (int col = 0; col < width; col++)
                finalData[row, col] = data[row, col] > 0 ? '#' : ' '; // Part 2

        PrintMatrix(finalData);
        return count;
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