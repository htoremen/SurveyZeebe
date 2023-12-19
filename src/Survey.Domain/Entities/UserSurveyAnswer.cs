namespace Domain.Entities;

public class UserSurveyAnswer
{
    [Key]
    public string UserSurveyAnswerId {  get; set; }
    public string UserSurveyId {  get; set; }
    public string SurveyQuestionId { get; set; }
    public string Answer {  get; set; }


    public virtual UserSurvey UserSurvey { get; set; }
    public virtual SurveyQuestion SurveyQuestion { get; set; }

}
