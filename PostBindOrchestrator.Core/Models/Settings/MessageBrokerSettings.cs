﻿namespace PostBindOrchestrator.Core;

public sealed record MessageBrokerSettings(string MessageBrokerConnectionString, MessageBrokerType MessageBrokerType);