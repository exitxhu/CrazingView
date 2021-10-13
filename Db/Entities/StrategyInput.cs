using System.Text.Json.Serialization;

namespace CrazingView.Db.Entities
{
    public class StrategyInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long StrategyId { get; set; }
        public double IncreaseStep { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }

        [JsonIgnore]
        public Strategy Strategy { get; set; }
    }
}

