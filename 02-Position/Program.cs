
static Tuple<string, int> Parse(string input)
{
    string command = input.Split(' ')[0];
    int amount = int.Parse(input.Split(' ')[1]);
    return Tuple.Create(command, amount);
}

string[] _commands = File.ReadAllLines("data.txt");

Tuple<string, int>[] commands = Array.ConvertAll<string, Tuple<string, int>>(_commands, s => Parse(s));

int horizontal = 0;
int depth = 0;

for (int i = 0; i < commands.Length; i++)
{
    (string command, int amount) = commands[i];
    switch (command)
    {
        case "forward":
            horizontal += amount;
            break;

        case "up":
            depth -= amount;
            break;
        case "down":
            depth += amount;
            break;
        default:
            break;
    }
}

Console.WriteLine(horizontal * depth);

horizontal = 0;
depth = 0;
int aim = 0;

for (int i = 0; i < commands.Length; i++)
{
    (string command, int amount) = commands[i];
    switch (command)
    {
        case "forward":
            horizontal += amount;
            depth += aim * amount;
            break;

        case "up":
            aim -= amount;
            break;
        case "down":
            aim += amount;
            break;
        default:
            break;
    }
}

Console.WriteLine(horizontal * depth);
