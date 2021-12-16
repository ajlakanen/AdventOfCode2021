using System.Collections.Immutable;

public class Day12
{
    private List<ImmutableList<int>> allPaths;
    private string[] nodes;
    private int[,] graph;
    private List<int> onlyOnce;

    public static void Main()
    {
        string[] dataRaw = File.ReadAllLines("data.txt");

        Day12 day12 = new Day12(dataRaw);
        day12.Part1(dataRaw);
        day12.Part2(dataRaw);
    }

    public Day12(string[] dataRaw)
    {
        nodes = new[] { "start" }.Concat(dataRaw.Select(x => x.Split('-')).Aggregate((a, b) => a.Concat(b).ToArray()).Distinct().Select(x => x).Where(x => x != "start" && x != "end")).Concat(new[] { "end" }).ToArray();
        graph = new int[nodes.Length, nodes.Length];
        onlyOnce = new List<int>();
        foreach (var item in dataRaw)
        {
            // This is an undirected graph, so we'll make a symmetrical matrix.
            int start = Array.IndexOf(nodes, item.Split('-')[0]);
            int end = Array.IndexOf(nodes, item.Split('-')[1]);
            graph[start, end]++;
            graph[end, start]++;

            // We also have to take care which nodes can be visited only once.
            if (Char.IsLower(nodes[start][0])) onlyOnce.Add(start);
            if (Char.IsLower(nodes[end][0])) onlyOnce.Add(end);
        }
        onlyOnce = onlyOnce.Distinct().ToList();
    }

    private void Part1(string[] dataRaw)
    {
        allPaths = new List<ImmutableList<int>>();

        ImmutableList<int> path = ImmutableList.Create<int>();
        path = path.Add(0);
        CreatePaths1(graph, onlyOnce, path);

        // Uncomment this if you want to print all the paths. 
        // AllPaths.ForEach(x => { Console.WriteLine(String.Join("-", Array.ConvertAll(x.ToArray(), y => nodes[y]))); });
        Console.WriteLine(allPaths.Count());
    }

    private void Part2(string[] dataRaw)
    {
        allPaths = new List<ImmutableList<int>>();
        ImmutableList<int> path = ImmutableList.Create<int>();
        path = path.Add(0);
        CreatePaths2(graph, onlyOnce, path);

        // Uncomment this if you want to print all the paths. 
        // AllPaths.ForEach(x => { Console.WriteLine(String.Join("-", Array.ConvertAll(x.ToArray(), y => nodes[y]))); });
        Console.WriteLine(allPaths.Count());
    }

    private void CreatePaths1(int[,] graph, List<int> onlyOnce, ImmutableList<int> path)
    {
        for (int col = 0; col < graph.GetLength(1); col++)
        {
            if (graph[path.Last(), col] == 0) { continue; }
            if (onlyOnce.IndexOf(col) >= 0)
            {
                if (path.Exists(x => x == col)) continue;
            }

            path = path.Add(col);
            if (col == graph.GetLength(1) - 1)
            {
                allPaths.Add(path);
                continue;
            }

            CreatePaths1(graph, onlyOnce, path);
            path = path.RemoveAt(path.Count - 1);
        }
    }

    private void CreatePaths2(int[,] graph, List<int> onlyOnce, ImmutableList<int> path)
    {
        for (int col = 0; col < graph.GetLength(1); col++)
        {
            if (graph[path.Last(), col] == 0) continue;
            if (onlyOnce.IndexOf(col) >= 0)
            {
                if (col == 0 || col == graph.GetLength(1)) continue;
                if (path.Exists(x => x == col))
                {
                    // This was a horror show...
                    var frCounts = from i in path
                                   where onlyOnce.Any(x => i == x && i != 0)// && i != 6)
                                   group i by i into gr
                                   let cnt = gr.Count()
                                   select new KeyValuePair<int, int>(gr.Key, cnt);
                    var max = frCounts.Aggregate(0, (max, next) => next.Value > max ? next.Value : max);
                    if (max == 2) continue;
                }
            }

            path = path.Add(col);
            if (col == graph.GetLength(1) - 1)
            {
                allPaths.Add(path);
                continue;
            }

            CreatePaths2(graph, onlyOnce, path);
            path = path.RemoveAt(path.Count - 1);
        }
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