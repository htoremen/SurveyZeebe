using Infrastructure.Persistence;
using Domain.Entities;

namespace Survey.Migration.App.Seed;

public static class SurveyItemSeed
{
    public static async Task Add(ApplicationDbContext context)
    {
        if (context == null)
            return;

        if (!context.SurveyItems.Any())
        {
            for (int i = 10; i < 15; i++)
            {
                var surveyItemId = "cd4c7486-354f-4104-8c75-dff6713601" + i;
                var surveyItem = context.SurveyItems.Add(new SurveyItem
                {
                    SurveyItemId = surveyItemId,
                    CategoryId = "4d4b3802-2128-44c8-ad98-47fe3000c101",
                    Title = "Anket " + i,
                    IsActive = true,
                    Created = DateTime.Now,
                }).Entity;

                var surveyResult = context.SaveChanges();
                if(surveyResult > 0)
                {
                    var random = new Random();
                    var k = random.Next(5,9);

                    int p = 0;
                    for (int j = 0; j < k; j++)
                    {
                        p += 1;
                        var surveyQuestionId = "df38e846-0623-43cf-926c-4adba5d09ec" + i + p;
                        context.SurveyQuestion.Add(new SurveyQuestion
                        {
                            SurveyQuestionId = surveyQuestionId,
                            SurveyItemId = surveyItemId,
                            Question = "Question " + i,
                            RowNumber = i,
                            QuestionA = "Choice A",
                            QuestionB = "Choice B",
                            QuestionC = "Choice C",
                            QuestionD = "Choice D",
                            QuestionE = "Choice E"
                        });
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}