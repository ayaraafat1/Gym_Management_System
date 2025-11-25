using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class HealthRecordConfigurations : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members").HasKey(K=>K.Id);

            builder.HasOne<Member>()
                .WithOne(H => H.HealthRecord)
                .HasForeignKey<HealthRecord>(H=>H.Id);

            builder.Ignore(H=>H.CreatedAt);

        }
    }
}
