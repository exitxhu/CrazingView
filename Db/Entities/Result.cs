using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrazingView.Db.Entities
{
    public class Result
    {
        public long Id { get; set; }
        public long SessionId { get; set; }
        public long RecordId { get; set; }
        public long StrategyId { get; set; }
        public DateTime RegisterDate { get; set; }
        public string RawValue { get; set; }

        public double NetProfit { get; set; }
        public double GrossProfit { get; set; }
        public double GrossLoss { get; set; }
        public double MaxDrawdown { get; set; }

        public Session Session { get; set; }
        public Strategy Strategy { get; set; }
        public Record Record { get; set; }

    }
    public class ResultConfig : IEntityTypeConfiguration<Result>
    {
        public void Configure(EntityTypeBuilder<Result> builder)
        {
            builder.HasOne(n => n.Strategy).WithMany().HasForeignKey(n => n.StrategyId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(n => n.Session).WithMany().HasForeignKey(n => n.SessionId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(n => n.Record).WithMany().HasForeignKey(n => n.RecordId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}

