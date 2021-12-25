using System.Collections;
using System.Text;

public enum PacketType { Sum = 0, Product = 1, Minimum = 2, Maximum = 3, Literal = 4, Gt = 5, Lt = 6, Equal = 7 }

public delegate int PacketCalculation(List<int> items);

static class PacketTypeMethods
{
    public static PacketCalculation Method(this PacketType pt)
    {
        switch (pt)
        {
            case PacketType.Sum:
                return x => x.Sum();
            case PacketType.Product:
                return x => x.Aggregate(1, (prev, next) => prev * next);
            case PacketType.Minimum:
                return x => x.Min();
            case PacketType.Maximum:
                return x => x.Max();
            case PacketType.Gt:
                return x => x.First() > x.Last() ? 1 : 0;
            case PacketType.Lt:
                return x => x.First() < x.Last() ? 1 : 0;
            case PacketType.Equal:
                return x => x.First() == x.Last() ? 1 : 0;
            default:
                throw new ArgumentException();
        }
    }
}

public class Packet
{
    public PacketType Type;
    public int Version;
    public List<Packet> Packets = new List<Packet>();
    private string sLiteral = "";
    public string StrLiteral
    {
        get { return sLiteral; }
        set
        {
            sLiteral = value;
            iLiteral = Convert.ToInt64(value, 2);
        }
    }



    public long iLiteral;

    public void AddPacket(Packet packet)
    {
        Packets ??= new List<Packet>();
        Packets.Add(packet);
    }

    public int VersionSum()
    {
        int sum = 0;
        sum += Version;
        if (Packets != null) Packets.ForEach(packet => { if (packet != null) { sum += packet.VersionSum(); } });
        return sum;
    }
}

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
        //int literal = Convert.ToInt32(sLiteral, 2);

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
        // packet.Type = typeID ==4 ? PacketType.Literal : PacketType.Operator;
        // packet.Type = (PacketType)typeID;
        
        i += 6;

        if (packet.Type == PacketType.Literal)
        {
            (string value, string remaining) = Literal(bits, i);
            packet.StrLiteral = value;
            return (packet, remaining);
        }
        else // Operator
        {
            if (bits[i++] == '0') // the next 15 bits contain the total length in bits of the sub-packets contained by this packet
            {
                int length = Convert.ToInt32(bits[i..(i + 15)], 2);
                i += 15;
                bits = bits.Substring(i);
                int j = 0;
                while (j < length)
                {
                    var (p, remaining) = ReadNext(bits);
                    if (remaining.Length == 0) j = length;
                    else j = bits.Length - remaining.Length;
                    bits = remaining;
                    packet.AddPacket(p);
                }
                i += length; // TODO: Do we need this?
                return (packet, bits);
            }
            else // the next 11 bits are a number that represents the number of sub-packets immediately contained by this packet
            {
                int noOfSubPackets = Convert.ToInt32(bits[i..(i + 11)], 2);
                i += 11;
                bits = bits.Substring(i);
                int j = 0;
                while (j < noOfSubPackets)
                {
                    var (p, remaining) = ReadNext(bits);
                    packet.AddPacket(p);
                    bits = remaining;
                    j++;
                }
                return (packet, bits);
            }
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