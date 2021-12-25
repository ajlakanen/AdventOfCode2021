public enum PacketType { Sum = 0, Product = 1, Minimum = 2, Maximum = 3, Literal = 4, Gt = 5, Lt = 6, Equal = 7 }


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
