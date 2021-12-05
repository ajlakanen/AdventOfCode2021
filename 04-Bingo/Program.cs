public class Board
{
    public int[,] Numbers;
    public bool IsBingo;

    public Board(int[,] numbers)
    {
        Numbers = numbers;
    }

    public bool Mark(int drawn)
    {
        for (int i = 0; i < Numbers.GetLength(0); i++)
        {
            for (int j = 0; j < Numbers.GetLength(1); j++)
            {
                if (Numbers[i, j] == drawn)
                {
                    Numbers[i, j] = -1;
                }
            }
        }
        bool hasWon = CheckBingo();
        if (hasWon) IsBingo = true;
        return IsBingo;
    }

    bool CheckBingo()
    {
        // Rows
        for (int i_row = 0; i_row < 5; i_row++)
        {
            bool rowWins = true;
            for (int j_col = 0; j_col < 5; j_col++)
            {
                if (Numbers[i_row, j_col] != -1)
                {
                    rowWins = false;
                    break;
                }
            }
            if (rowWins) return true;
        }

        // Columns
        for (int i_col = 0; i_col < 5; i_col++)
        {
            bool colWins = true;
            for (int j_row = 0; j_row < 5; j_row++)
            {
                if (Numbers[j_row, i_col] != -1)
                {
                    colWins = false;
                    break;
                }
            }
            if (colWins) return true;
        }

        return false;
    }

}

public class Day4
{
    public static void Main()
    {
        string[] input = File.ReadAllLines("data.txt");
        int[] drawn = Array.ConvertAll(input[0].Split(','), str => int.Parse(str));
        List<Board> boards = new List<Board>();

        int i = 2;
        while (i < input.Length)
        {
            int[,] board = new int[5, 5];

            for (int j = 0; j < 5; j++)
            {
                int[] row = Array.ConvertAll(input[i + j].Split(' ', StringSplitOptions.RemoveEmptyEntries), str => int.Parse(str));
                for (int k = 0; k < 5; k++) board[j, k] = row[k];
            }

            boards.Add(new Board(board));
            i += 6;
        }

        int numbersDrawn = 0;
        Board winningBoard = null;
        while (true)
        {
            int whichNumber = drawn[numbersDrawn];
            boards.ForEach(board => board.Mark(whichNumber));

            // Part A:
            // if (boards.Select(x => x).Where(x => x.IsWinner).Count() == 1)
            // {
            //     winningBoard = boards.Select(x => x).Where(x => x.IsWinner).First();
            //     break;
            // }

            // Part B:
            if (winningBoard != null && winningBoard.IsBingo) break;
            
            if (boards.Select(x => x).Where(x => !x.IsBingo).Count() == 1)
            {
                winningBoard = boards.Select(x => x).Where(x => !x.IsBingo).First();
            }
            numbersDrawn++;
        }


        int sum = (from int item in winningBoard.Numbers
                   where item != -1
                   select item).Sum();
        int lastNumber = drawn[numbersDrawn];

        Console.WriteLine(sum * lastNumber);
    }
}