namespace Survey.Shared.Models;

public class SurveyModel
{
    public string InstanceId { get; set; }
    public string UserId { get; set; }

    public bool IsSurveyCompleted { get; set; } 

    public int MaxRetryCount { get; set; }
    public string RetryFrequence { get; set; }
    public string ExpireInMinutes { get; set; }

    public ProcessSurveyRequest ProcessSurveyRequest { get; set; }
    public SurveyQuestionRequest SurveyQuestionRequest {  get; set; }
}

public class ProcessSurveyRequest
{
    public string SurveyItemId { get; set; }
}

public class SurveyQuestionRequest
{
    public string InstanceId { get; set; }
    public string UserSurveyId { get; set; }
    public string SurveyQuestionId { get; set; }
    public string Answer { get; set; }
}