public delegate long OperatorFunction(List<long> items);

static class Operators
{
    public static OperatorFunction Function(this PacketType pt)
    {
        switch (pt)
        {
            case PacketType.Sum:
                return x => x.Sum();
            case PacketType.Product:
                return x => x.Aggregate<long, long>(1, (prev, next) => prev * next);
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
