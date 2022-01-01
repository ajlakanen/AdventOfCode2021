public class Literal : Packet
{
    private string _value = "";
    public string StrValue
    {
        get { return _value; }
        set
        {
            _value = value;
            Value = Convert.ToInt64(value, 2);
        }
    }

    public long Value;

    public Literal(int version, int typeID, string strValue) : base(version, typeID)
    {
        StrValue = strValue;
    }
}
