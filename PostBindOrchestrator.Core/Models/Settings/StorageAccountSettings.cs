﻿namespace PostBindOrchestrator.Core;

public sealed record StorageAccountSettings(string StorageAccountConnectionString, string UpdatesContainerName, string ScreenshotsContainerName);