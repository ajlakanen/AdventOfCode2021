public class Packet
{
    public int TypeID;
    public int Version;

    public Packet(int version)
    {
        Version = version;
    }

    public virtual int VersionSum()
    {
        return Version;
    }
}
