using InfluxDB.Client.Core;

namespace Forge.Application.Data.Measurements;

[Measurement("temperature")]
public class Temperature
{
    [Column(IsTag = true)]
    public bool Cold { get; set; }
    [Column("temp-value")]
    public decimal Value { get; set; }
}
