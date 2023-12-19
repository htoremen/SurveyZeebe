namespace Domain.Entities;

public class UserSurvey
{
    [Key]
    public string UserSurveyId {  get; set; }
    public string SurveyItemId { get; set; }
    public string UserId {  get; set; }
    public DateTime Created {  get; set; }
    public SurveyStatus Status {  get; set; }
    public int IsActived {  get; set; }  

    public string InstanceId { get; set; }
    public long ProcessInstanceKey {  get; set; }

    public virtual SurveyItem SurveyItem { get; set; }
    public virtual User User { get; set; }
}
