using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PostBindOrchestrator.Api;
using PostBindOrchestrator.DomainLayer;
using Xunit;
using Xunit.Sdk;

namespace PostBindOrchestrator.MiddlewareTests;
public class RouteHandlerTests
{
    private readonly WebApplicationFactory<Program> webApplicationFactory;

    public RouteHandlerTests() => webApplicationFactory = new CustomWebApplicationFactory();

    private HttpClient CreateHttpClient(string? correlationId)
    {
        var httpClient = webApplicationFactory.CreateClient();

        if (correlationId is not null)
        {
            httpClient.DefaultRequestHeaders.Add("X-Correlation-Id", correlationId);
        }

        return httpClient;
    }

    private TestMediator GetAssociatedTestMediator<TRouteHandler>() where TRouteHandler : notnull
    {
        var routeHandler = webApplicationFactory.Services.GetRequiredService<TRouteHandler>();
        return ((ITestMediatorCompositor)routeHandler).TestMediator;
    }

    private static StringContent GetJsonStringContent<T>(T dto) => new(JsonSerializer.Serialize(dto, typeof(T)), Encoding.UTF8, "application/json");

    [Fact]
    public async Task ProcessPostBind_WhenValidRequestIsMade_ReturnsOKAndValidValuesAreReceived()
    {
        // Arrange
        var expectedPolicyNumber = "Pol-12345";
        var expectedInterviewData = new InterviewData("Q13579");
        var expectedCorrelationId = "Corr-1a2b3c4d5e";
        var testMediator = GetAssociatedTestMediator<RouteHandlerPostBind>();

        using var httpClient = CreateHttpClient(expectedCorrelationId);

        // Act
        var httpResponseMessage = await httpClient.PostAsync($"/processpostbind/{expectedPolicyNumber}", GetJsonStringContent(expectedInterviewData));

        // Assert
        await AssertHttpResponseIsSuccess(httpResponseMessage);
        Assert.Equal(expectedCorrelationId, testMediator.CorrelationId);
        Assert.Equal(expectedPolicyNumber, testMediator.PolicyNumber);
        Assert.Equal(expectedInterviewData.QuoteId, testMediator.InterviewData!.QuoteId);
    }

    [Fact]
    public async Task ProcessPostBind_WhenNoCorrelationIdInHttpHeader_ReturnsBadRequestWithDetailedExceptionInformation()
    {
        // Arrange
        var expectedPolicyNumber = "Pol-12345";
        var expectedInterviewData = new InterviewData("Q13579");

        using var httpClient = CreateHttpClient(null);

        // Act
        var httpResponseMessage = await httpClient.PostAsync($"/processpostbind/{expectedPolicyNumber}", GetJsonStringContent(expectedInterviewData));

        // Assert
        AssertHttpResponseHasAllRequisitesForNon200Response(httpResponseMessage, HttpStatusCode.BadRequest, new CorrelationIdNotProvidedException().Reason, typeof(CorrelationIdNotProvidedException));
        await AssertHttpResponseContentContains(httpResponseMessage.Content, "Correlation Id was Not Provided in the HTTP Header");
    }

    [Fact]
    public async Task ProcessRevertToQuote_WhenValidRequestIsMade_ReturnsOKAndValidValuesAreReceived()
    {
        // Arrange
        var expectedPolicyNumber = "Pol-24680";
        var expectedCorrelationId = "Corr-2z4y6x8w0v";
        var testMediator = GetAssociatedTestMediator<RouteHandlerRevertToQuote>();

        using var httpClient = CreateHttpClient(expectedCorrelationId);

        // Act
        var httpResponseMessage = await httpClient.PostAsync($"/processreverttoquote/{expectedPolicyNumber}", null);

        // Assert
        await AssertHttpResponseIsSuccess(httpResponseMessage);
        Assert.Equal(expectedCorrelationId, testMediator.CorrelationId);
        Assert.Equal(expectedPolicyNumber, testMediator.PolicyNumber);
    }

    [Fact]
    public async Task ProcessRevertToQuote_WhenNoCorrelationIdInHttpHeader_ReturnsBadRequestWithDetailedExceptionInformation()
    {
        // Arrange
        var expectedPolicyNumber = "Pol-24680";

        using var httpClient = CreateHttpClient(null);

        // Act
        var httpResponseMessage = await httpClient.PostAsync($"/processreverttoquote/{expectedPolicyNumber}", null);

        // Assert
        AssertHttpResponseHasAllRequisitesForNon200Response(httpResponseMessage, HttpStatusCode.BadRequest, new CorrelationIdNotProvidedException().Reason, typeof(CorrelationIdNotProvidedException));
        await AssertHttpResponseContentContains(httpResponseMessage.Content, "Correlation Id was Not Provided in the HTTP Header");
    }

    private static async Task AssertHttpResponseContentContains(HttpContent content, string expectedContent)
    {
        var actualContent = await content.ReadAsStringAsync();

        if (!actualContent.Contains(expectedContent))
        {
            throw new XunitException($"The HTTP Content was Expected to contain: `{expectedContent}`, but the Actual Content was `{actualContent}`");
        }
    }

    private static void AssertHttpResponseHasAllRequisitesForNon200Response(HttpResponseMessage httpResponseMessage, HttpStatusCode expectedHttpStatusCode, string expectedReasonPhrase, Type expectedExceptionType)
    {
        var errorMessages = new StringBuilder();

        if (expectedHttpStatusCode != httpResponseMessage.StatusCode)
        {
            errorMessages.AppendLine($"The Expected HTTP Status Code is: {expectedHttpStatusCode}, but the Actual HTTP Status Code was: {httpResponseMessage.StatusCode}");
        }

        if (expectedReasonPhrase != httpResponseMessage.ReasonPhrase)
        {
            errorMessages.AppendLine($"The Expected Reason Phrase is: {expectedReasonPhrase}, but the Actual Reason Phrase was: {httpResponseMessage.ReasonPhrase}");
        }

        if (httpResponseMessage.Headers.TryGetValues("Exception-Type", out var values))
        {
            var actualExceptionTypeHeaderValue = values.Single();

            if (expectedExceptionType.Name != actualExceptionTypeHeaderValue)
            {
                errorMessages.AppendLine($"The Expected value of the HTTP Header: `Exception-Type` is: {expectedExceptionType.Name}, but the Actual value : {actualExceptionTypeHeaderValue}");
            }
        }
        else
        {
            errorMessages.AppendLine($"Expected to find an HTTP Header: `Exception-Type` with a value of `{expectedExceptionType.Name}`, but no such header was found");
        }

        if (errorMessages.Length is not 0)
        {
            throw new XunitException(errorMessages.ToString());
        }
    }

    private static async Task AssertHttpResponseIsSuccess(HttpResponseMessage httpResponseMessage)
    {
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new XunitException($"Expected HTTP Status Code 200, but Received HTTP Status Code: {httpResponseMessage.StatusCode}, with Reason Phrase: {httpResponseMessage.ReasonPhrase}, with Content: {await httpResponseMessage.Content.ReadAsStringAsync()}");
        }
    }
}