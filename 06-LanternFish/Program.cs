/// <summary>
/// See, https://adventofcode.com/2021/day/6
/// </summary>
public class Day6
{
    public static void Main()
    {
        int[] initialState = Array.ConvertAll(File.ReadAllLines("data.txt")[0].Split(','), x => int.Parse(x));
        List<int> states = new List<int>(initialState);

        Console.WriteLine(PartA(states));
        Console.WriteLine(PartB(states));
    }

    static long PartB(List<int> states)
    {
        Dictionary<long, long> statesAndCounts = new Dictionary<long, long>();

        foreach (var item in states)
            if (statesAndCounts.ContainsKey(item))
                statesAndCounts[item]++;
            else
                statesAndCounts.Add(item, 1);

        for (int i = 0; i <= 8; i++)
            if (!statesAndCounts.ContainsKey(i))
                statesAndCounts.Add(i, 0);

        int days = 0;
        Dictionary<long, long> toBeAdded = new Dictionary<long, long>();
        while (days < 256)
        {
            statesAndCounts[0] = 0;

            for (int i = 1; i <= 8; i++)
            {
                long temp = statesAndCounts[i];
                statesAndCounts[i - 1] += statesAndCounts[i];
                statesAndCounts[i] -= temp;
            }

            if (toBeAdded.ContainsKey(6)) statesAndCounts[6] += toBeAdded[6];
            if (toBeAdded.ContainsKey(8)) statesAndCounts[8] += toBeAdded[8];
            toBeAdded.Clear();

            toBeAdded.Add(6, statesAndCounts[0]);
            toBeAdded.Add(8, statesAndCounts[0]);
            days++;
        }
        return statesAndCounts.Sum(x => x.Value);
    }

    static long PartA(List<int> states)
    {
        List<int> modifiedStates = new List<int>(states);
        int days = 0;
        while (days < 18)
        {
            int i = 0;
            int countNow = modifiedStates.Count;
            while (i < countNow)
            {
                if (states[i] == 0)
                {
                    modifiedStates[i] = 6;
                    modifiedStates.Add(8);
                }
                else modifiedStates[i]--;
                i++;
            }
            days++;
        }
        return modifiedStates.Count;
    }

}