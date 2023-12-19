using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Survey.Application.Common.Interfaces;
using Survey.Application.Worker;
using Survey.Shared.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Zeebe.Client.Api.Worker;

namespace Survey.Infrastructure.Services;

public class SurveyWorkerService : ISurveyWorkerService
{
    private IZeebeService _zeebeService;
    private static readonly string WorkerName = Environment.MachineName;
    private IServiceProvider _provider = null!;
    private ISender _mediator = null!;

    public SurveyWorkerService(IZeebeService zeebeService, IServiceProvider provider)
    {
        _zeebeService = zeebeService;
        _provider = provider;
        _mediator = _provider.CreateScope().ServiceProvider.GetRequiredService<ISender>();
    }

    public void StartWorkers()
    {
        SurveyAssignment();
        SurveyConfirmation();
        CheckSurvey();
        SurveyCompleted();
        TimeOut();
        Error();
    }

    private void SurveyAssignment()
    {
        CreateWorker("SurveyAssignment", async (jobClient, job) =>
        {
            Dictionary<string, object> customHeaders = JsonSerializer.Deserialize<Dictionary<string, object>>(job.CustomHeaders);
            Dictionary<string, object> _variables = JsonSerializer.Deserialize<Dictionary<string, object>>(job.Variables);
            var variables = JsonSerializer.Deserialize<SurveyModel>(job.Variables);
     
            try
            {
                if (variables != null)
                {
                    var response = _mediator.Send(new SurveyAssignmentCommand
                    {
                        Model = variables,
                        ProcessInstanceKey = job.ProcessInstanceKey
                    }).Result;


                    var data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                    var success = jobClient.NewCompleteJobCommand(job.Key)
                        .Variables(data)
                        .Send();
                }
            }
            catch (Exception ex)
            {
            }
        });
    }
    private void SurveyConfirmation()
    {
        CreateWorker("SurveyConfirmation", async (jobClient, job) =>
        {
            Dictionary<string, object> _variables = JsonSerializer.Deserialize<Dictionary<string, object>>(job.Variables);
            var variables = JsonSerializer.Deserialize<SurveyModel>(job.Variables);

            try
            {
                if (variables != null)
                {
                    var response = _mediator.Send(new SurveyConfirmationCommand
                    {
                        Model = variables,
                    }).Result;


                    var data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                    var success = jobClient.NewCompleteJobCommand(job.Key)
                        .Variables(data)
                        .Send();
                }
            }
            catch (Exception ex)
            {
            }
        });
    }
    private void CheckSurvey()
    {
        CreateWorker("CheckSurvey", async (jobClient, job) =>
        {
            Dictionary<string, object> _variables = JsonSerializer.Deserialize<Dictionary<string, object>>(job.Variables);
            var variables = JsonSerializer.Deserialize<SurveyModel>(job.Variables);

            try
            {
                if (variables != null)
                {
                    var response = _mediator.Send(new CheckSurveyCommand
                    {
                        Model = variables,
                    }).Result;

                    variables.IsSurveyCompleted = response.Data.IsSurveyCompleted;
                    var data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                    var success = jobClient.NewCompleteJobCommand(job.Key)
                        .Variables(data)
                        .Send();
                }
            }
            catch (Exception ex)
            {
            }
        });
    }
    private void SurveyCompleted()
    {
        CreateWorker("SurveyCompleted", async (jobClient, job) =>
        {
            Dictionary<string, object> _variables = JsonSerializer.Deserialize<Dictionary<string, object>>(job.Variables);
            var variables = JsonSerializer.Deserialize<SurveyModel>(job.Variables);

            try
            {
                if (variables != null)
                {
                    var response = _mediator.Send(new SurveyCompletedCommand
                    {
                        Model = variables,
                    }).Result;

                    var data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                    var success = jobClient.NewCompleteJobCommand(job.Key)
                        .Variables(data)
                        .Send();
                }
            }
            catch (Exception ex)
            {
            }
        });
    }


    private void TimeOut()
    {
        CreateWorker("TimeOut", async (jobClient, job) =>
        {
            var variables = JsonSerializer.Deserialize<SurveyModel>(job.Variables);

            try
            {
                if (variables != null)
                {
                    var response = _mediator.Send(new TimeOutCommand
                    {
                        Model = variables,
                    }).Result;

                    var data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                    var success = jobClient.NewCompleteJobCommand(job.Key)
                        .Variables(data)
                        .Send();
                }
            }
            catch (Exception ex)
            {
            }

        });
    }

    private void Error()
    {
        CreateWorker("Error", async (jobClient, job) =>
        {
            var variables = JsonSerializer.Deserialize<SurveyModel>(job.Variables);
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            var success = jobClient.NewCompleteJobCommand(job.Key)
                      .Variables(data)
                      .Send();
        });
    }

    private void CreateWorker(String jobType, JobHandler handleJob)
    {
        _zeebeService.Client().NewWorker()
               .JobType(jobType)
               .Handler(handleJob)
               .MaxJobsActive(250)
               .Name(WorkerName)
               .AutoCompletion()
               .PollInterval(TimeSpan.FromSeconds(60))
               .PollingTimeout(TimeSpan.FromSeconds(60))
               .Timeout(TimeSpan.FromSeconds(60))
               .AutoCompletion()
               .Open();
    }
}
