namespace Domain.Entities;

public class SurveyQuestion
{
    [Key]
    public string SurveyQuestionId { get; set; }
    public string SurveyItemId {  get; set; }
    public string Question { get; set; }
    public int  RowNumber { get; set; }

    public string QuestionA {  get; set; }
    public string QuestionB { get; set; }
    public string QuestionC { get; set; }
    public string QuestionD { get; set; }
    public string QuestionE { get; set; }


    public virtual SurveyItem SurveyItem { get; set; }
}
