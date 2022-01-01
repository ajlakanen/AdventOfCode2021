public class Day17
{
    public static void Main()
    {
        Day17 day17 = new Day17();
        (int highest, int count) = day17.Solve(xmin: 143, xmax: 177, ymin: -106, ymax: -71);
        Console.WriteLine($"Count: {count}. Highest: {highest}");
    }

    private (int, int) Solve(int xmin, int xmax, int ymin, int ymax)
    {
        int count = 0;
        int highest = 0;
        for (int xSpeed = 0; xSpeed <= xmax; xSpeed++)
        {
            for (int ySpeed = -106; ySpeed < 9500; ySpeed++)
            {
                var (hitsTarget, highestNow) = Probe(xmin, xmax, ymin, ymax, xSpeed, ySpeed);
                if (hitsTarget)
                {
                    if (highestNow > highest) highest = highestNow;
                    count++;
                    // Console.WriteLine($"{count}: xSpeed: {xSpeed}, ySpeed: {ySpeed}, highest: {highestNow}");
                }
            }
        }
        return (highest, count);
    }

    private (bool, int) Probe(int xmin, int xmax, int ymin, int ymax, int xSpeed, int ySpeed)
    {
        int xpos = 0;
        int ypos = 0;
        int highest = 0;

        while (true)
        {
            xpos += xSpeed;
            ypos += ySpeed;
            if (ypos > highest) highest = ypos;

            if (xpos >= xmin && xpos <= xmax &&
                ypos >= ymin && ypos <= ymax)
            {
                return (true, highest);
            }
            xSpeed = Math.Max(xSpeed - 1, 0);
            ySpeed -= 1;
            if (xpos >= xmax || ypos <= ymin) return (false, 0);
        }
    }
}