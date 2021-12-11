public class Day9
{
    public static void Main()
    {
        string[] dataRaw = File.ReadAllLines("data.txt");

        int errorSum = Part1(dataRaw);
        Console.WriteLine(errorSum);

        long scoreSum = Part2(dataRaw);
        Console.WriteLine(scoreSum);
    }

    private static long Part2(string[] data)
    {
        List<long> scoreSums = new List<long>();
        var brackets = new List<(char Open, char Close, int Score)>{
            ( '(', ')', 1 ),
            ( '[', ']', 2 ),
            ( '{', '}', 3 ),
            ( '<', '>', 4 )
        };

        for (int i = 0; i < data.Length; i++)
            scoreSums.Add(CompleteBrackets(data[i]));

        var s = scoreSums.Select(x => x).Where(x => x != 0).OrderBy(x => x);
        return s.Take(s.Count() / 2 + 1).Last(); // Take the middle one. Note: Odd number of sums is assumed. 

        long CompleteBrackets(string data)
        {
            long score = 0;
            List<char> characters = new List<char>();

            for (int i = 0; i < data.Length; i++)
            {
                if (brackets.Any(x => x.Open == data[i])) characters.Add(data[i]);
                else if (brackets.Select(x => x).Where(x => x.Close == data[i]).Select(x => x.Open).First() == characters.Last())
                    characters.RemoveAt(characters.Count - 1);
                else return 0;
            }

            while (characters.Count > 0)
            {
                score *= 5;
                score += brackets.Select(x => x).Where(x => x.Item1 == characters.Last()).Select(x => x.Score).First();
                characters.RemoveAt(characters.Count - 1);
            }

            return score;
        }
    }

    private static int Part1(string[] dataRaw)
    {
        int errorSum = 0;

        var brackets = new List<(char, char, int)>{
            ( '(', ')', 3 ),
            ( '[', ']', 57 ),
            ( '{', '}', 1197 ),
            ( '<', '>', 25137 )
        };

        for (int i = 0; i < dataRaw.Length; i++) errorSum += LineError(dataRaw[i]);
        return errorSum;

        int LineError(string data)
        {
            List<char> characters = new List<char>();

            for (int i = 0; i < data.Length; i++)
            {
                if (brackets.Any(x => x.Item1 == data[i])) characters.Add(data[i]);
                else if (brackets.Select(x => x).Where(x => x.Item2 == data[i]).Select(x => x.Item1).First() == characters.Last())
                    characters.RemoveAt(characters.Count - 1);
                else return brackets.Select(x => x).Where(x => x.Item2 == data[i]).Select(x => x.Item3).First();
            }
            return 0;
        }
    }
}