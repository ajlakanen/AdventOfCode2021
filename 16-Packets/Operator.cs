public class Operator : Packet
{
    public OperatorFunction Function { get; set; }
    public List<Packet> Packets = new List<Packet>();

    public Operator(int version, int typeID, OperatorFunction function) : base(version, typeID)
    {
        Function = function;
    }

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
