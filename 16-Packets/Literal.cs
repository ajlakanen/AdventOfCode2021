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

    public Literal(int version, int typeID, string value) : base(version, typeID)
    {
        StrValue = value;
    }
}
