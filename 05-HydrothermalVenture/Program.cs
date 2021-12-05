public struct Line
{
    public Point Start;
    public Point End;
    public Line(Point start, Point end)
    {
        Start = start;
        End = end;
    }
}

public struct Point
{
    public int X;
    public int Y;
}

public class Day5
{
    public static void Main()
    {
        string[] input = File.ReadAllLines("data.txt");
        Line[] lines = new Line[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            Line line = ParseLine(input[i]);
            lines[i] = line;
        }

        int maxX = lines.Max(x => Math.Max(x.Start.X, x.End.X));
        int maxY = lines.Max(x => Math.Max(x.Start.Y, x.End.Y));
        int[,] diagram = new int[maxY + 1, maxX + 1];

        for (int i = 0; i < lines.Length; i++)
        {
            // For straight lines, it does not matter in which 
            // orientation we draw them. Taking the min and max values 
            // of each dimension makes writing for-loops a bit easier. 
            int lineStartCol = Math.Min(lines[i].Start.X, lines[i].End.X);
            int lineEndCol = Math.Max(lines[i].Start.X, lines[i].End.X);
            int lineStartRow = Math.Min(lines[i].Start.Y, lines[i].End.Y);
            int lineEndRow = Math.Max(lines[i].Start.Y, lines[i].End.Y);

            int verticalDirection = Math.Sign(lines[i].End.Y - lines[i].Start.Y);
            int horizontalDiretion = Math.Sign(lines[i].End.X - lines[i].Start.X);

            if (lines[i].Start.X != lines[i].End.X && lines[i].Start.Y != lines[i].End.Y)
            {
                // Part A:
                // continue;

                // Part B:
                int length = lineEndRow - lineStartRow;
                int diagonal = 0;
                while(diagonal <= length)
                {
                    diagram[lines[i].Start.Y + diagonal * verticalDirection, lines[i].Start.X + diagonal * horizontalDiretion]++;
                    diagonal++;
                }
            }
            else
            {
                for (int lineRow = lineStartRow; lineRow <= lineEndRow; lineRow++)
                    for (int lineCol = lineStartCol; lineCol <= lineEndCol; lineCol++)
                        diagram[lineRow, lineCol]++;
            }
        }

        // PrintMatrix(diagram);

        int overlappingPoints = (from int item in diagram
                                 where item > 1
                                 select item).Count();
        Console.WriteLine(overlappingPoints);
    }

    public static void PrintMatrix(int[,] arr)
    {
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                Console.Write(string.Format("{0} ", arr[i, j]));
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }

    public static Line ParseLine(string input)
    {
        string[] _startAndEnd = input.Split(" -> ");
        string[] _start = _startAndEnd[0].Split(',');
        string[] _end = _startAndEnd[1].Split(',');

        Point start = new Point()
        {
            X = int.Parse(_start[0]),
            Y = int.Parse(_start[1])
        };

        Point end = new Point()
        {
            X = int.Parse(_end[0]),
            Y = int.Parse(_end[1])
        };
        return new Line(start, end);
    }
}