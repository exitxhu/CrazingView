using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrazingView.Db.Entities
{
    public class Record
    {
        public long Id { get; set; }
        public long StrategyId { get; set; }
        public string Value { get; set; }

        public Strategy Strategy { get; set; }

    }

    public class RecordConfigs : IEntityTypeConfiguration<Record>
    {
        public void Configure(EntityTypeBuilder<Record> builder)
        {
        }
    }
}

