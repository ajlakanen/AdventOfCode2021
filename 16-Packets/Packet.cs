public enum PacketType { 
    Sum = 0, 
    Product = 1, 
    Minimum = 2, 
    Maximum = 3, 
    Literal = 4,
    Gt = 5, 
    Lt = 6, 
    Equal = 7 
}


public class Literal : Packet
{
    private string _value = "";
    public string StrValue
    {
        get { return _value; }
        set
        {
            _value = value;
            iValue = Convert.ToInt64(value, 2);
        }
    }

    public long iValue;

    public Literal(int version) : base(version) { }
}

public class Operator : Packet
{
    public PacketFunction Function { get; set; }
    public List<Packet> Packets = new List<Packet>();

    public Operator(int version) : base(version) { }

    public void AddPacket(Packet packet)
    {
        Packets ??= new List<Packet>();
        Packets.Add(packet);
    }
}

public class Packet
{
    public int TypeID;
    public int Version;
    public List<Packet> Packets = new List<Packet>();

    public Packet(int version)
    {
        Version = version;
    }

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
