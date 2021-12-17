public class Day13
{
    public static void Main()
    {
        string[][] splitted = Array.ConvertAll(File.ReadAllLines("dataSmall.txt"), x => x.Split(new[] { ',', '=' }));
        List<(int x, int y)> coords = new List<(int x, int y)>();
        List<(bool vertical, int foldRow)> foldInstructions = new List<(bool, int)>();
        int i = 0;
        while (splitted[i].Length > 1)
        {
            coords.Add(
                (int.Parse(splitted[i][0]), int.Parse(splitted[i][1]))
                );
            i++;
        }
        i = splitted.Length - 1;
        while (splitted[i].Length > 1)
        {
            bool vertical = (splitted[i][0].Last() == 'x') ? true : false;
            foldInstructions.Add(
                (vertical, int.Parse(splitted[i][1]))
                );
            i--;
        }
        foldInstructions.Reverse();

        Day13 day13 = new Day13();
        int sum = day13.Part1(coords.ToArray(), foldInstructions.ToArray());
    }

    private int Part1((int x, int y)[] coords, (bool vertical, int foldRow)[] foldInstructions)
    {
        int maxX = coords.Max(x => x.x);
        int maxY = coords.Max(x => x.y);
        int[,] data = new int[maxY+1, maxX+1];
        int i = 0;
        while (i < foldInstructions.Length)
        {
            i++;

        }
        throw new NotImplementedException();
    }
}