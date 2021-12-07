public class Day7
{
    public static void Main()
    {
        int[] initialStates = Array.ConvertAll(File.ReadAllLines("data.txt")[0].Split(','), x => int.Parse(x));

        Part1(initialStates);
        Part2(initialStates);
    }

    private static void Part2(int[] initialStates)
    {
        int minState = initialStates.Min();
        int maxState = initialStates.Max();

        long minCost = int.MaxValue;
        for (int i = minState; i <= maxState; i++)
        {
            long costNow = 0;
            for (int j = 0; j < initialStates.Length; j++)
            {
                int n = Math.Abs(initialStates[j] - i);
                costNow += (n * (1 + n)) / 2;
            }
            if (costNow < minCost) minCost = costNow;
        }
        Console.WriteLine(minCost);

    }

    private static void Part1(int[] initialStates)
    {
        int minCost = int.MaxValue;
        for (int i = 0; i < initialStates.Length; i++)
        {
            int costNow = 0;
            for (int j = 0; j < initialStates.Length; j++)
            {
                if (i == j) continue;
                costNow += Math.Abs(initialStates[j] - initialStates[i]);
            }
            if (costNow < minCost) minCost = costNow;
        }
        Console.WriteLine(minCost);
    }


    /// <summary>
    /// This was an unfortunate triumph on premature optimization. It doesn't work, for now. 
    /// </summary>
    /// <param name="initialStates"></param>
    public static void Part1B(int[] initialStates)
    {
        int minCost = int.MaxValue;
        int minState = initialStates.Min();
        int maxState = initialStates.Max();
        int targetState = maxState - ((maxState - minState) / 2);

        while (true)
        {
            int targetCandidateA = targetState - ((targetState - minState) / 2);
            if (targetState % 2 != 0) targetCandidateA -= 1;
            int targetCandidateB = targetState + ((maxState - targetState) / 2);
            //if (targetState % 2 != 0) targetCandidateB += 1;

            int costCandidateA = Cost(initialStates, targetCandidateA);
            int costCandidateB = Cost(initialStates, targetCandidateB);

            targetState = costCandidateA < costCandidateB ? targetCandidateA : targetCandidateB;
            if (costCandidateA < minCost)
            {
                minCost = costCandidateA;
                maxState = maxState - ((maxState - minState) / 2);
            }
            else if (costCandidateB < minCost)
            {
                minCost = costCandidateB;
                minState = minState + ((maxState - minState) / 2);
            }
            else
            {
                
                minState = minState + ((maxState - minState) / 2);
                maxState = maxState - ((maxState - minState) / 2);
                if (Math.Abs(maxState - minState) == 1) 
                    break;
            }
            // minState = costCandidateA < costCandidateB ? minState : maxState - ((maxState - minState) / 2);
            // maxState = costCandidateA < costCandidateB ? minState + ((maxState - minState) / 2) : maxState;
        }
        Console.WriteLine(minCost);


        int Cost(int[] states, int target)
        {
            int cost = 0;
            for (int i = 0; i < states.Length; i++)
                cost += Math.Abs(states[i] - target);
            return cost;
        }
    }


    
}
