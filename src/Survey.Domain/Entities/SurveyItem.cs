namespace Domain.Entities;

public class SurveyItem
{
    [Key]
    public string SurveyItemId {  get; set; }
    public string CategoryId {  get; set; }
    public string Title {  get; set; }
    public DateTime Created {  get; set; }
    public bool IsActive {  get; set; }


    public virtual Parameter Category {  get; set; }
}
