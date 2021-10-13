using System;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CrazingView.Db.Entities
{
    public class SessionLog
    {
        public long Id { get; set; }
        public long SessionId { get; set; }
        public DateTime LogTime { get; set; }
        public string Details { get; set; }
        public LogType LogType { get; set; }

        public Session Session { get; set; }

    }
    public class SessionLogConfig : IEntityTypeConfiguration<SessionLog>
    {
        public void Configure(EntityTypeBuilder<SessionLog> builder)
        {
            builder.Property(n => n.LogType).HasConversion<int>();
        }
    }
}

