public class Packet
{
    public int TypeID;
    public int Version;

    public Packet(int version, int typeID)
    {
        Version = version;
        TypeID = typeID;
    }

    public virtual int VersionSum()
    {
        return Version;
    }
}
