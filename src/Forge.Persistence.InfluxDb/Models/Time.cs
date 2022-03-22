namespace Forge.Persistence.InfluxDb.Models
{
    public struct Time
    {
        public int Value { get; }
        public TimeType Type { get; }

        public Time(int value, TimeType type)
        {
            Value = value;
            Type = type;
        }

        public override string ToString() => $"{Value}{GetTimeTypeAsString(Type)}";

        private static string GetTimeTypeAsString(TimeType timeType) => timeType switch
        {
            TimeType.Seconds => "s",
            TimeType.Minutes => "m",
            TimeType.Hours => "h",
            _ => string.Empty,
        };
    }
}
