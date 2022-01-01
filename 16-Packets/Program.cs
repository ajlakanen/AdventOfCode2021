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
    private int Type(string bits, int i) => Convert.ToInt32(bits[(i + 3)..(i + 6)], 2);

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
        Packet packet;
        var (p, remaining) = ReadNext(bits);
        packet = p;

        var result = Eval(packet);
        return packet.VersionSum();
    }

    private long Eval(Packet packet)
    {
        if (packet is Literal) return (packet as Literal).iValue;
        packet = (Operator)packet;
        PacketFunction func = (packet as Operator).Function;
        var packets = (packet as Operator).Packets;
        List<long> values = new List<long>();

        foreach (Packet p in packets)
        {
            var result = Eval(p);
            values.Add(result);
        }

        var invoked = func.Invoke(values);
        return invoked;
    }

    private (Packet packet, string remainingBits) ReadNext(string bits)
    {
        if (bits.All(x => x == '0'))
            return (null, "");
        int i = 0;
        int version = Version(bits, i);
        int typeID = Type(bits, i);
        Packet packet;
        if (typeID == 4) packet = new Literal(version);
        else packet = new Operator(version);
        i += 6;

        if (packet is Literal)
        {
            (string value, string remaining) = Literal(bits, i);
            (packet as Literal).StrValue = value;
            return (packet, remaining);
        }
        else // Operator
        {
            int lenTypeID = int.Parse(bits[i++] + "");
            (packet as Operator).Function = Operators.Function((PacketType)packet.TypeID);
            int lTypeBits = lenTypeID == 0 ? 15 : 11;

            int lenOrNumber = Convert.ToInt32(bits[i..(i + lTypeBits)], 2);
            i += lTypeBits;
            bits = bits.Substring(i);
            int j = 0;
            while (j < lenOrNumber)
            {
                var (p, remaining) = ReadNext(bits);
                if (remaining.Length == 0) j = lenOrNumber;
                else j = lenTypeID == 0 ? bits.Length - remaining.Length : j + 1;
                bits = remaining;
                if (p != null) (packet as Operator).AddPacket(p);
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