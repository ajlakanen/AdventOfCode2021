using System.Text;

public class Day14
{
    private readonly string Start = "NNCB";
    private readonly Dictionary<string, string> Rules;

    public static void Main()
    {
        string[][] splitted = Array.ConvertAll(File.ReadAllLines("data.txt"), x => x.Split("->", StringSplitOptions.RemoveEmptyEntries));
        Day14 day14 = new Day14(splitted);
        long count = day14.Solve(day14.Start, 40);
        Console.WriteLine(count);
    }

    /// <summary>
    /// Pathetic brute force attempt.
    /// </summary>
    /// <param name="steps"></param>
    /// <returns></returns>
    private int SolveBruteForce(int steps)
    {
        StringBuilder result = new StringBuilder(Start);
        for (int i = 0; i < steps; i++)
        {
            int j = 0;
            while (j < result.Length - 1)
            {
                // var rule = Rules.Select(x => x).Where(y => y.Item1[0] == result[j] && y.Item1[1] == result[j + 1]);
                var rule = Rules[result[j] + "" + result[j + 1]];
                if (rule.Count() != 0)
                {
                    // result = result.Insert(j + 1, rule.First().Item2);
                    // result.Insert(j + 1, rule.First().Item2);
                    result.Insert(j + 1, rule);
                    j++;
                }
                j++;
            }
            Console.WriteLine($"Step {i}: {result.Length}");
        }

        var freqCounts = result.ToString().Select(x => x).GroupBy(x => x).OrderByDescending(x => x.Count());
        var count = freqCounts.First().Count() - freqCounts.Last().Count();
        return count;
    }

    private long Solve(string start, int steps)
    {
        Dictionary<string, long> pairCounts = new();
        Dictionary<char, long> charCounts = new();
        Dictionary<string, long> pairPool = new();

        for (int i = 0; i < start.Length - 1; i++)
        {
            string pair = start[i] + "" + start[i + 1];
            if (!pairPool.TryAdd(pair, 1)) pairPool[pair]++;
            if (!charCounts.TryAdd(start[i], 1)) charCounts[start[i]]++;
        }

        if (charCounts.ContainsKey(start[start.Length - 1])) charCounts[start[start.Length - 1]]++;
        else charCounts.Add(start[start.Length - 1], 1);
        foreach (var item in pairPool)
        {
            if (!pairCounts.TryAdd(item.Key, item.Value)) pairCounts[item.Key] += item.Value;
        }

        pairPool.Clear();

        for (int i = 0; i < steps; i++)
        {
            foreach (var item in pairCounts)
            {
                string input = item.Key;
                string output = Rules[input];
                string pair1 = input[0] + output;
                string pair2 = output + input[1];

                if (!pairPool.TryAdd(pair1, item.Value)) pairPool[pair1] += item.Value;
                if (!pairPool.TryAdd(pair2, item.Value)) pairPool[pair2] += item.Value;
                pairCounts[input] = 0;
                if (!charCounts.TryAdd(output[0], item.Value)) charCounts[output[0]] += item.Value;
            }

            foreach (var item in pairPool)
            {
                if (!pairCounts.TryAdd(item.Key, item.Value)) pairCounts[item.Key] += item.Value;
            }
            pairPool.Clear();
        }

        var cc = charCounts.OrderByDescending(x => x.Value);
        return cc.First().Value - cc.Last().Value;
    }

    /// <summary>
    /// Prepare the data
    /// </summary>
    /// <param name="dataRaw"></param>
    public Day14(string[][] dataRaw)
    {
        Start = dataRaw[0][0];
        Rules = new Dictionary<string, string>();
        var r = (from s in dataRaw
                 where s.Length > 1
                 select (s[0].Trim(), s[1].Trim())).ToArray();
        
        foreach (var rule in r)
            Rules.Add(rule.Item1, rule.Item2);
    }
}