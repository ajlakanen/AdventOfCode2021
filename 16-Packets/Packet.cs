public class Packet
{

    private int _typeID;

    /// <summary>
    /// Type ID for debugging purposes. 
    /// </summary>
    public int TypeID
    {
        get { return _typeID; }
        set
        {
            _typeID = value;
            Type = (PacketType)_typeID;
        }
    }

    public PacketType Type;
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
