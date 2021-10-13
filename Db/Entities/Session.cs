using System;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CrazingView.Db.Entities
{
    public class Session
    {
        public long Id { get; set; }
        public string TabId { get; set; }
        public long StrategyId { get; set; }
        public string PairName { get; set; }
        public int ChunkCount { get; set; }
        public long ChunkStartId { get; set; }
        public long ChunkEndId { get; set; }
        public SessionStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }


        public Record ChunkStart { get; set; }
        public Record ChunkEnd { get; set; }
        public Strategy Strategy { get; set; }
    }
    public class SessionConfig : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.Property(n => n.Status).HasConversion<int>();

            builder.HasOne(n => n.ChunkEnd).WithMany().HasForeignKey(n=>n.ChunkEndId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(n => n.ChunkStart).WithMany().HasForeignKey(n=>n.ChunkStartId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(n => n.Strategy).WithMany().HasForeignKey(n=>n.StrategyId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}

