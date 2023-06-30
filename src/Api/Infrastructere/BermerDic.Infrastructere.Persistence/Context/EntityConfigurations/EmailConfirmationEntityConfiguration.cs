using BermerDic.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BermerDic.Infrastructere.Persistence.Context.EntityConfigurations
{
    public class EmailConfirmationEntityConfiguration : BaseEntityConfiguration<EmailConfirmation>
    {
        public override void Configure(EntityTypeBuilder<EmailConfirmation> builder)
        {
            base.Configure(builder);

            builder.ToTable("EmailConfirmations", BermerDicContext.DEFAULT_SCHEMA);
        }
    }
}