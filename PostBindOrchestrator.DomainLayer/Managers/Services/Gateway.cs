using System.Diagnostics.CodeAnalysis;
using System.Net;
using PostBindOrchestrator.DomainLayer.Managers.SegregatedInterfaces;

namespace PostBindOrchestrator.DomainLayer.Managers.Services;
internal sealed class Gateway : IDisposable
{
    private bool _disposed;
    private HttpClient _httpClient;

    public Gateway(IHttpMessageHandlerProvider httpMessageHandlerProvider, string baseUrl) => _httpClient = CreateHttpClient(httpMessageHandlerProvider, baseUrl);

    private static HttpClient CreateHttpClient(IHttpMessageHandlerProvider httpMessageHandlerProvider, string baseUrl)
    {
        var httpMessageHandler = httpMessageHandlerProvider.CreateHttpMessageHandler();

        if (httpMessageHandler is SocketsHttpHandler socketsHttpHandler)
        {
            socketsHttpHandler.PooledConnectionLifetime = TimeSpan.FromMinutes(15);
            socketsHttpHandler.PreAuthenticate = true;
            socketsHttpHandler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
        }

        var httpClient = new HttpClient(httpMessageHandler)
        {
            BaseAddress = new Uri(baseUrl),
        };

        return httpClient;
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (disposing && !_disposed && _httpClient != null)
        {
            var localHttpClient = _httpClient;
            localHttpClient.Dispose();
            _httpClient = null!;
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
}
