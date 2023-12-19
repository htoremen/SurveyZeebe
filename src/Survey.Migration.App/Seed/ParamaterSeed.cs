using Domain.Entities;
using Infrastructure.Persistence;

namespace Migration.App.Seed;

public static class ParamaterSeed
{
    public static async Task SeedFormParametersAsync(ApplicationDbContext context)
    {
        if (context == null)
            return;

        if (!context.ParameterTypes.Any())
        {
            var parameterTypeId = "4d4b3802-2128-44c8-ad98-47fe3000c100";
            var parameterType = context.ParameterTypes.Add(new ParameterType { ParameterTypeId = parameterTypeId, Name = "Anket Türleri" });


            parameterType.Entity.Parameters.Add(new Parameter { ParameterId = "4d4b3802-2128-44c8-ad98-47fe3000c101", ParameterTypeId = parameterTypeId, Name = "Tüketici Anketleri", IsActive = true });
            parameterType.Entity.Parameters.Add(new Parameter { ParameterId = "4d4b3802-2128-44c8-ad98-47fe3000c102", ParameterTypeId = parameterTypeId, Name = "Pazar Araştırma Anketleri", IsActive = true });
            parameterType.Entity.Parameters.Add(new Parameter { ParameterId = "4d4b3802-2128-44c8-ad98-47fe3000c103", ParameterTypeId = parameterTypeId, Name = "Eğitim Anketleri", IsActive = true });
            parameterType.Entity.Parameters.Add(new Parameter { ParameterId = "4d4b3802-2128-44c8-ad98-47fe3000c104", ParameterTypeId = parameterTypeId, Name = "Sosyal Bilinç Anketleri", IsActive = true });

            await context.SaveChangesAsync();
        }
    }
}