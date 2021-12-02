
int[] measurements = Array.ConvertAll(File.ReadAllLines("data.txt"), s => int.Parse(s));

int previous = measurements[0];
int increased = 0;

for (int i = 0; i < measurements.Length; i++)
{
    if (measurements[i] > previous) increased++;
    previous = measurements[i];
}

Console.WriteLine(increased);

increased = 0;
previous = measurements[0] + measurements[1] + measurements[2];

for (int i = 2; i < measurements.Length - 1; i++)
{
    int thisSum = measurements[i - 1] + measurements[i] + measurements[i + 1];
    if (thisSum > previous)
    {
        increased++;
    }
    previous = thisSum;
}

Console.WriteLine(increased);