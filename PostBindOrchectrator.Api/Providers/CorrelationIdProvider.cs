﻿using PostBindOrchestrator.DomainLayer;

namespace PostBindOrchestrator.Api;

public sealed class CorrelationIdProvider
{
    private readonly HttpContext httpContext;

    public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor) => httpContext = httpContextAccessor.HttpContext!;

    public string CorrelationId
    {
        get
        {
            if (httpContext.Items.TryGetValue("CorrelationId", out object? value))
            {
                return (string)value!;
            }

            throw new CorrelationIdNotProvidedException("Correlation Id was Not Provided in the HTTP Header `X-Correlation-Id`. An attempt was made to access this value, but no Header was found");
        }
    }
}