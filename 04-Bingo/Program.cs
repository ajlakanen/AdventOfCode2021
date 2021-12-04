string[] input = File.ReadAllLines("data.txt");
int[] drawn = Array.ConvertAll(input[0].Split(','), str => int.Parse(str));
List<int[,]> boards = new List<int[,]>();

int i = 2;
while (i < input.Length)
{
    int[,] board = new int[5, 5];

    for (int j = 0; j < 5; j++)
    {
        int[] row = Array.ConvertAll(input[i + j].Split(' ', StringSplitOptions.RemoveEmptyEntries), str => int.Parse(str));
        for (int k = 0; k < 5; k++) board[j, k] = row[k];
    }

    boards.Add(board);
    i += 6;
}

int numbersDrawn = 0;
int winnerIndex = -1;
while (true)
{
    int whichNumber = drawn[numbersDrawn];
    winnerIndex = MarkAndCheck(boards, whichNumber);
    if (winnerIndex != -1) break;
    numbersDrawn++;
}

int sum = (from int item in boards[winnerIndex]
           where item != -1
           select item).Sum();

Console.WriteLine(sum * drawn[numbersDrawn]);

/// <summary>
/// When correct number is found, mark "-1".
/// Then check if this board is a winning board.
/// </summary>
int MarkAndCheck(List<int[,]> boards, int whichNumber)
{
    for (int i = 0; i < boards.Count; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            for (int k = 0; k < 5; k++)
            {
                if (boards[i][j, k] == whichNumber)
                {
                    boards[i][j, k] = -1;
                    if (CheckRowAndColumn(boards[i]))
                        return i;

                }
            }
        }
    }
    return -1;
}

bool CheckRowAndColumn(int[,] board)
{
    // Rows
    for (int i_row = 0; i_row < 5; i_row++)
    {
        bool rowWins = true;
        for (int j_col = 0; j_col < 5; j_col++)
        {
            if (board[i_row, j_col] != -1)
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
            if (board[j_row, i_col] != -1)
            {
                colWins = false;
                break;
            }
        }
        if (colWins) return true;
    }

    return false;
}
