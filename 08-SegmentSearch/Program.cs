public class Day8
{
    public static void Main()
    {
        Tuple<string, string>[] input = Array.ConvertAll(File.ReadAllLines("data.txt"), x => Tuple.Create(x.Split('|')[0], x.Split('|')[1]));

        Part1(input);
        Part2(input);
    }

    private static void Part2(Tuple<string, string>[] input)
    {
        int sum = 0;
        foreach (var item in input)
        {
            string[] numbers = item.Item1.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] outputs = item.Item2.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string n1 = numbers.Select(x => x).Where(x => x.Length == 2).First();
            string n4 = numbers.Select(x => x).Where(x => x.Length == 4).First();
            string n7 = numbers.Select(x => x).Where(x => x.Length == 3).First();
            string n8 = numbers.Select(x => x).Where(x => x.Length == 7).First();

            // Segment A is the segment in 7 that does not appear in number 1. 
            string sa = n7.Except(n1).First().ToString();

            // G appears most often in the numbers
            // with 4 or more segments (numbers 0, 2, 3, 5, 6, 8, 9)
            // that is NOT the segment A which we already know. 
            string sg = String.Join("", numbers.Select(x => x).Where(x => x.Length > 4).ToArray()).Replace(sa, "").GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key.ToString();

            // B = intersect(0, 5, 6, 9).except(sg).except(n7)
            string sb = numbers.Select(x => x).Where(x => x.Length == 6).ToArray().Aggregate((previous, next) => new string(previous.Intersect(next).ToArray())).Except(sg).Except(n7).First().ToString();
            
            string n9 = new string(n4.Union(sa).Union(sg).ToArray());
            string n3 = new string(n9.Except(sb).ToArray());
            string se = new string(n8.Except(n3).Except(sb).ToArray());
            string n0 = new string(sa.Union(sb).Union(se).Union(sg).Union(n1).ToArray());
            string sd = new string(n8.Except(n0).ToArray());
            string n2 = numbers.Select(x => x).Where(x => x.Length == 5).ToArray().Select(x => x).Where(x => x.Contains(se)).First().ToString();
            string sc = new string(n2.Intersect(n1).ToArray());
            string n5 = new string(n2.Union(n1).Except(sc).Except(se).Union(sb).ToArray());
            string n6 = new string(n5.Union(se).ToArray());
            string[] newNumbers = new string[] { n0, n1, n2, n3, n4, n5, n6, n7, n8, n9 };
            int sumNow = 0;
            int i = 0;
            while(true)
            {
                int number = Array.FindIndex(newNumbers, x => String.Concat(x.OrderBy(c=>c)) == String.Concat(outputs[i].OrderBy(c=>c)));
                sumNow+=number;
                i++;
                if (i < outputs.Length) sumNow *= 10;
                else break;
            }
            sum += sumNow;
        }
        Console.WriteLine(sum);
    }

    private static void Part1(Tuple<string, string>[] input)
    {
        int count = 0;
        string lengths = "2347";
        foreach (var item in input)
        {
            string[] outputs = item.Item2.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Array.ForEach(outputs, x => { if (lengths.IndexOf(x.Length.ToString()) >= 0) count++; });
        }
        Console.WriteLine(count);
    }
}
