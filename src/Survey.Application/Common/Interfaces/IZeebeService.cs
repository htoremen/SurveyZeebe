﻿using Zeebe.Client;
using Zeebe.Client.Api.Responses;

namespace Survey.Application.Common.Interfaces;

public interface IZeebeService
{
    public Task<ITopology> Status();
    public Task<IDeployResourceResponse> Deploy(string modelFilename);
    public Task<string> SendMessage(string instanceId, string messageName, string payload);
    public IZeebeClient Client();
    public void StartWorkers(string url);
}