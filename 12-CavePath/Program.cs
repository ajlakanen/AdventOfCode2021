
using System.Collections.Immutable;

public class Day12
{
    static List<ImmutableList<int>> AllPaths;
    static string[] nodes;
    static bool OneVisitedTwice = false;

    public static void Main()
    {

        string[] dataRaw = File.ReadAllLines("data.txt");
        Part1(dataRaw);
        Part2(dataRaw);
    }

    private static void Part1(string[] dataRaw)
    {
        OneVisitedTwice = true;
        AllPaths = new List<ImmutableList<int>>();
        nodes = new[] { "start" }.Concat(dataRaw.Select(x => x.Split('-')).Aggregate((a, b) => a.Concat(b).ToArray()).Distinct().Select(x => x).Where(x => x != "start" && x != "end")).Concat(new[] { "end" }).ToArray();

        int[,] graph = new int[nodes.Length, nodes.Length];
        var onlyOnce = new List<int>();

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

        ImmutableList<int> path = ImmutableList.Create<int>();
        path = path.Add(0);
        //CreatePaths(graph, onlyOnce, path, 0);
        CreatePaths(graph, onlyOnce, path);
        Console.WriteLine("===");
        //AllPaths.ForEach(x => { Console.WriteLine(String.Join("-", Array.ConvertAll(x.ToArray(), y => nodes[y]))); });
        Console.WriteLine(AllPaths.Count());
    }

    private static void Part2(string[] dataRaw)
    {
        OneVisitedTwice = false;
        AllPaths = new List<ImmutableList<int>>();
        nodes = new[] { "start" }.Concat(dataRaw.Select(x => x.Split('-')).Aggregate((a, b) => a.Concat(b).ToArray()).Distinct().Select(x => x).Where(x => x != "start" && x != "end")).Concat(new[] { "end" }).ToArray();

        int[,] graph = new int[nodes.Length, nodes.Length];
        var onlyOnce = new List<int>();

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

        ImmutableList<int> path = ImmutableList.Create<int>();
        path = path.Add(0);
        //CreatePaths(graph, onlyOnce, path, 0);
        CreatePaths2(graph, onlyOnce, path);
        Console.WriteLine("===");
        AllPaths.ForEach(x => { Console.WriteLine(String.Join("-", Array.ConvertAll(x.ToArray(), y => nodes[y]))); });
        Console.WriteLine(AllPaths.Count());
    }

    private static void CreatePaths2(int[,] graph, List<int> onlyOnce, ImmutableList<int> path)
    {
        for (int col = 0; col < graph.GetLength(1); col++)
        {
            if (graph[path.Last(), col] == 0) continue;

            if (onlyOnce.IndexOf(col) >= 0)
            {
                if (col == 0 || col == graph.GetLength(1)) continue;
                //if (path.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToArray().Count() > 1) continue;
                if (path.Exists(x => x == col))
                {
                    //continue;
                //}

                //var except = new[] { 0, 5 };
                ////var l = path.Where(item => onlyOnce.Any(item2 => item == item2 && item != 0 && item != 6)).GroupBy(x => x);
                ////var cnt = l.Count()
                var frCounts = from i in path
                               where onlyOnce.Any(x => i == x && i != 0)// && i != 6)
                               group i by i into gr
                               let cnt = gr.Count()
                               select new KeyValuePair<int, int>(gr.Key, cnt);
                var max = frCounts.Aggregate(0, (max, next) => next.Value > max ? next.Value : max);
                if (max == 2) continue;
                //if (l != null)
                //{
                //    var ll = l.Select(x => x).Count();
                //}
                //l.Values.MaxBy(y => y.Count);

                }
            }

            path = path.Add(col);
            //Console.WriteLine(String.Join("-", Array.ConvertAll(path.ToArray(), y => nodes[y])));
            if (col == graph.GetLength(1) - 1)
            {
                AllPaths.Add(path);
                continue;
            }

            CreatePaths2(graph, onlyOnce, path);
            path = path.RemoveAt(path.Count - 1);
        }
    }

    private static void CreatePaths(int[,] graph, List<int> onlyOnce, ImmutableList<int> path)
    {
        for (int col = 0; col < graph.GetLength(1); col++)
        {
            if (graph[path.Last(), col] == 0) continue;

            if (onlyOnce.IndexOf(col) >= 0)
            {
                if (path.Exists(x => x == col)) continue;
            }

            path = path.Add(col);
            //Console.WriteLine(String.Join("-", Array.ConvertAll(path.ToArray(), y => nodes[y])));
            if (col == graph.GetLength(1) - 1)
            {
                AllPaths.Add(path);
                continue;
            }

            CreatePaths(graph, onlyOnce, path);
            path = path.RemoveAt(path.Count - 1);
        }
    }



    //private static void CreatePaths(int[,] graph, List<int> onlyOnce, ImmutableList<int> path, int newVal)
    //{
    //    for (int col = 1; col < graph.GetLength(1); col++)
    //    {
    //        if (graph[newVal, col] == 0) continue;
    //        if (!IsSafe(graph, onlyOnce, path, col)) continue;
    //        path = path.Add(col);
    //        if (col == graph.GetLength(1) - 1)
    //        {
    //            AllPaths.Add(path);
    //        }
    //        Console.WriteLine("Added: " + String.Join("->", path.ToArray()));
    //        CreatePaths(graph, onlyOnce, path, col);
    //    }

    //}


    /// <summary>
    /// Check if new value is safe to be added to path. 
    /// New value is safe if
    ///  - an edge exists between the path's last value and new value,
    ///  - new value is not already added into the path in case it's lower case (max 1 visits allowed).
    /// </summary>
    /// <param name="graph"></param>
    /// <param name="onlyOnce"></param>
    /// <param name="path"></param>
    /// <param name="newVal">Candidate position to be added to path.</param>
    /// <returns>New value is a valid new node for the path. </returns>
    /// <exception cref="NotImplementedException"></exception>
    private static bool IsSafe(int[,] graph, List<int> onlyOnce, ImmutableList<int> path, int newVal)
    {
        //Console.Clear();
        //Console.WriteLine(String.Join("->", path.ToArray()));
        //Console.WriteLine(path.Last() + ", " + newVal);
        //Console.WriteLine("---");
        //PrintMatrix(graph);
        //Console.WriteLine("===");

        if (graph[path.Last(), newVal] != 1) return false;
        if (onlyOnce.IndexOf(newVal) >= 0)
        {
            if (path.Exists(x => x == newVal)) return false;
        }
        return true;
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