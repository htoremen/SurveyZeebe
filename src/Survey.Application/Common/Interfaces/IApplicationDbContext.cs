using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }
        DbSet<RefreshToken> RefreshTokens { get; }
        DbSet<LoginHistory> LoginHistories { get; }
        DbSet<Parameter> Parameters { get; }
        DbSet<ParameterType> ParameterTypes { get; }

        DbSet<SurveyItem> SurveyItems { get; }
        DbSet<SurveyQuestion> SurveyQuestion { get; }
        DbSet<UserSurvey> UserSurveys { get; }
        DbSet<UserSurveyAnswer> UserSurveyAnswers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}