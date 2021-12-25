using System.Collections;
using System.Text;


public class Day16
{
    private static readonly Dictionary<char, string> _hexToBin = new()
    {
        { '0', "0000" },
        { '1', "0001" },
        { '2', "0010" },
        { '3', "0011" },
        { '4', "0100" },
        { '5', "0101" },
        { '6', "0110" },
        { '7', "0111" },
        { '8', "1000" },
        { '9', "1001" },
        { 'A', "1010" },
        { 'B', "1011" },
        { 'C', "1100" },
        { 'D', "1101" },
        { 'E', "1110" },
        { 'F', "1111" }
    };

    private readonly string _bits = "";

    public static void Main()
    {
        string dataRaw = File.ReadAllLines("data.txt")[0];
        Day16 day16 = new Day16(dataRaw);
        var result = day16.Solve(day16._bits);
        //Console.WriteLine(count);
    }

    private int Version(string bits, int i) => Convert.ToInt32(bits[i..(i + 3)], 2);
    private PacketType Type(string bits, int i) => (PacketType)Convert.ToInt32(bits[(i + 3)..(i + 6)], 2);

    private (string value, string remainingBits) Literal(string bits, int i)
    {
        int j = i;
        string sLiteral = "";
        while (true)
        {
            string group = bits[j..(j + 5)];
            sLiteral += bits[(j + 1)..(j + 5)];
            j += 5;
            if (bits[j - 5] == '0') break;
        }
        return (sLiteral, bits.Substring(j));
    }

    private int Solve(string bits)
    {
        Packet packet = new Packet();
        var (p, remaining) = ReadNext(bits);
        packet = p;

        return packet.VersionSum();
    }

    private (Packet packet, string remainingBits) ReadNext(string bits)
    {
        if (bits.All(x => x == '0'))
            return (null, "");
        int i = 0;
        Packet packet = new Packet();

        packet.Version = Version(bits, i);
        packet.Type = Type(bits, i);
        i += 6;

        if (packet.Type == PacketType.Literal)
        {
            (string value, string remaining) = Literal(bits, i);
            packet.StrLiteral = value;
            return (packet, remaining);
        }
        else // Operator
        {
            int lTypeID = int.Parse(bits[i++] + "");
            int lTypeBits = lTypeID == 0 ? 15 : 11;

            int lenOrNumber = Convert.ToInt32(bits[i..(i + lTypeBits)], 2);
            i += lTypeBits;
            bits = bits.Substring(i);
            int j = 0;
            while (j < lenOrNumber)
            {
                var (p, remaining) = ReadNext(bits);
                if (remaining.Length == 0) j = lenOrNumber;
                else j = lTypeID == 0 ? bits.Length - remaining.Length : j + 1;
                bits = remaining;
                packet.AddPacket(p);
            }
            return (packet, bits);
        }
    }

    /// <summary>
    /// Prepare the data.
    /// </summary>
    /// <param name="dataRaw"></param>
    public Day16(string dataRaw)
    {
        StringBuilder sb = new();
        for (int i = 0; i < dataRaw.Length; i++)
            sb.Append(_hexToBin[dataRaw[i]]);
        _bits = sb.ToString();
    }
}