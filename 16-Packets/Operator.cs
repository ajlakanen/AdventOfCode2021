

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

    public override int VersionSum()
    {
        int sum = 0;
        sum += Version;
        Packets.ForEach(x => sum += x.VersionSum());
        return sum;
    }
}
