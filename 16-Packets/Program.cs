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
    }

    private int ParseVersion(string bits, int i, bool debug = false)
    {
        if (debug) Console.Write(bits[i..(i + 3)] + " ");
        return Convert.ToInt32(bits[i..(i + 3)], 2);
    }

    private int ParseType(string bits, int i, bool debug = false)
    {
        if (debug) Console.Write(bits[(i + 3)..(i + 6)] + " ");
        return Convert.ToInt32(bits[(i + 3)..(i + 6)], 2);
    }
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
        return (sLiteral, bits[j..]);
    }

    private int Solve(string bits)
    {
        Packet packet;
        var (p, remaining) = ReadNext(bits);
        packet = p;

        var result = Eval(packet);
        return packet.VersionSum();
    }

    /// <summary>
    /// Evaluate the expression tree.
    /// </summary>
    /// <param name="packet">Packet.</param>
    /// <returns>Result of the evaluation.</returns>
    private long Eval(Packet packet)
    {
        if (packet is Literal) return (packet as Literal).Value;
        OperatorFunction func = (packet as Operator).Function;
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

    private (Packet packet, string remainingBits) ReadNext(string bits, bool debug = false)
    {
        if (bits.All(x => x == '0')) return (null, "");
        int i = 0;
        int version = ParseVersion(bits, i, debug);
        int typeID = ParseType(bits, i, debug);
        i += 6;

        if (typeID == 4) // Literal
        {
            (string value, string remaining) = Literal(bits, i);
            Literal literal = new(version, typeID, value);
            if (debug) Console.WriteLine(value + " // Literal " + literal.Value);
            return (literal, remaining);
        }
        else // Operator
        {
            int lenTypeID = int.Parse(bits[i++] + "");
            Operator oper = new(version, typeID, Operators.Function((PacketType)typeID));
            int lenTypeBits = lenTypeID == 0 ? 15 : 11;
            int lenOrNumber = Convert.ToInt32(bits[i..(i + lenTypeBits)], 2);

            #region Debug
            if (debug) Console.Write(lenTypeID + " "); 
            if (debug) Console.Write(bits[i..(i + lenTypeBits)] + " ");
            if (debug) Console.Write(" // ");
            if (debug) Console.Write((PacketType)typeID + " ");
            if (debug) Console.Write(lenOrNumber + " ");
            if (debug) if (lenTypeID == 0) Console.Write(lenOrNumber + " bits \n"); else Console.Write(lenOrNumber + " packets \n");
            #endregion

            i += lenTypeBits;
            bits = bits[i..];
            int j = 0;
            while (j < lenOrNumber)
            {
                var (p, remaining) = ReadNext(bits, true);
                if (remaining.Length == 0) j = lenOrNumber;
                else j = lenTypeID == 0 ? j += bits.Length - remaining.Length : j + 1;
                bits = remaining;
                if (p != null) oper.AddPacket(p);
            }
            return (oper, bits);
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