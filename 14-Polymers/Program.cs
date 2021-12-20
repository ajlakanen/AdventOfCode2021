using System.Text;

public class Day14
{
    private string Start = "NNCB";
    private Dictionary<string, string> Rules;
    private Dictionary<StringBuilder, string> RulesSB;
    // private (string, string)[] Rules;

    public static void Main()
    {
        string[][] splitted = Array.ConvertAll(File.ReadAllLines("dataSmall.txt"), x => x.Split("->", StringSplitOptions.RemoveEmptyEntries));
        Day14 day14 = new Day14(splitted);
        long count = day14.Apply4(day14.Start, 10);
        Console.WriteLine(count);
    }

    private int Apply(int steps)
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

    private int Apply4(string start, int steps)
    {
        Dictionary<string, long> pairCounts = new Dictionary<string, long>();
        Dictionary<char, long> charCounts = new Dictionary<char, long>();
        Dictionary<string, long> pairPool = new Dictionary<string, long>();

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

                if (!pairPool.TryAdd(pair1, 1)) pairPool[pair1]++;
                if (!pairPool.TryAdd(pair2, 1)) pairPool[pair2]++;
                pairCounts[item.Key] = Math.Max(0, pairCounts[item.Key] - 1);
                if (!charCounts.TryAdd(output[0], 1)) charCounts[output[0]]++;
            }

            foreach (var item in pairPool)
            {
                if (!pairCounts.TryAdd(item.Key, item.Value)) pairCounts[item.Key] += item.Value;
            }
            pairPool.Clear();
        }

        return 0;
    }

    private int Apply3(int steps)
    {
        StringBuilder result = new StringBuilder(Start);
        StringBuilder temp = new StringBuilder();
        StringBuilder ruleTemp = new StringBuilder();
        for (int i = 0; i < steps; i++)
        {
            int j = 0;
            while (j < result.Length - 1)
            {
                // var rule = Rules.Select(x => x).Where(y => y.Item1[0] == result[j] && y.Item1[1] == result[j + 1]);
                //if(j == result.Length-1) temp.Append(result[j]);

                var rule = Rules[result[j] + "" + result[j + 1]];
                if (rule.Count() != 0)
                {
                    // result = result.Insert(j + 1, rule.First().Item2);
                    // result.Insert(j + 1, rule.First().Item2);
                    //result.Insert(j + 1, rule);
                    temp.Append(result[j]);
                    temp.Append(rule);
                }
                else
                {
                    temp.Append(result[j]);
                }
                j++;
            }
            temp.Append(result[j]);
            result = temp;
            temp = new StringBuilder();

            Console.WriteLine($"Step {i}: {result.Length}");
        }

        var freqCounts = result.ToString().Select(x => x).GroupBy(x => x).OrderByDescending(x => x.Count());
        var count = freqCounts.First().Count() - freqCounts.Last().Count();
        return count;
    }

    private long Apply2(string s, int steps)
    {
        //string result = s;
        ////for (int i = 0; i < steps; i++)
        ////{
        ////    for (int j = 0; j < result.Length; j++)
        ////    {

        ////    }
        ////}
        ////if (steps==1)
        ////{
        ////    for (int i = 0; i < steps; i++)
        ////    {
        ////        int j = 0;
        ////        while (j < result.Length - 1)
        ////        {
        ////            var rule = Rules.Select(x => x).Where(y => y.Item1 == result[j] + "" + result[j + 1]);
        ////            if (rule.Count() != 0)
        ////            {
        ////                result = result.Insert(j + 1, rule.First().Item2);
        ////                j++;
        ////            }
        ////            j++;
        ////        }
        ////    }
        ////    return result.Length;
        ////}
        ////return (Apply2(s[0] + "" + s[1], steps - 1) + Apply2(s.Substring(2), steps-1));

        //if (steps == 1) return s;

        //for (int i = 0; i < 3; i++)
        //{

        //}
        return 0;
    }

    /// <summary>
    /// Prepare the data
    /// </summary>
    /// <param name="dataRaw"></param>
    public Day14(string[][] dataRaw)
    {
        Start = dataRaw[0][0];
        //Start = "NNCB";
        Rules = new Dictionary<string, string>();
        // RulesSB = new Dictionary<StringBuilder, string>();
        var r = (from s in dataRaw
                 where s.Length > 1
                 select (s[0].Trim(), s[1].Trim())).ToArray();
        // Rules = r;
        foreach (var rule in r)
        {
            Rules.Add(rule.Item1, rule.Item2);
        }

    }
}