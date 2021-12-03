static char MostCommonDigitInPosition(string[] values, int position)
{
    int ones = CountInstances(values, '1', position);
    if (values.Length - ones <= ones) return '1';
    else return '0';
}

static char LeastCommonDigitInPosition(string[] values, int position)
{
    if (MostCommonDigitInPosition(values, position) == '0') return '1';
    else return '0';
}

static int CountInstances(string[] values, char searched, int position)
{
    int numberOfInstances = 0;
    for (int i = 0; i < values.Length; i++) if (values[i][position] == searched) numberOfInstances++;
    return numberOfInstances;
}

string[] inputs = File.ReadAllLines("data.txt");

int numbersLength = inputs[0].Length; // Let's assume that all numbers are equal length. 
string gamma = "";
string epsilon = "";

for (int i = 0; i < numbersLength; i++)
{
    int count = CountInstances(inputs, '1', i);
    if (count > inputs.Length / 2)
    {
        gamma += "1";
        epsilon += "0";
    }

    else
    {
        gamma += "0";
        epsilon += "1";
    }
}

long g = Convert.ToInt64(gamma, 2);
long e = Convert.ToInt64(epsilon, 2);
Console.WriteLine(g*e);

// The leftmost bit/character is in position zero. 

int position = 0;
string[] modifiedInputs = new string[inputs.Length];
Array.Copy(inputs, modifiedInputs, inputs.Length);

while (modifiedInputs.Length>1 && position < numbersLength)
{
    char mostCommon = MostCommonDigitInPosition(modifiedInputs, position);
    modifiedInputs = modifiedInputs.Select(x => x).Where(x => x[position] == mostCommon).ToArray();
    position++;
}

Console.WriteLine(modifiedInputs[0]);
long oxygenRating = Convert.ToInt64(modifiedInputs[0], 2);
Console.WriteLine(oxygenRating);

modifiedInputs = new string[inputs.Length];
Array.Copy(inputs, modifiedInputs, inputs.Length);
position = 0;

// Note: This is a quick and dirty way.
// We could also sort the numbers and start 
// investigating the powers of two (or messing with
// bitwise operations) from there. 

while (modifiedInputs.Length > 1 && position < numbersLength)
{
    char leastCommon = LeastCommonDigitInPosition(modifiedInputs, position);
    modifiedInputs = modifiedInputs.Select(x => x).Where(x => x[position] == leastCommon).ToArray();
    position++;
}

Console.WriteLine(modifiedInputs[0]);
long co2Rating = Convert.ToInt64(modifiedInputs[0], 2);
Console.WriteLine(co2Rating);

Console.WriteLine(oxygenRating * co2Rating);