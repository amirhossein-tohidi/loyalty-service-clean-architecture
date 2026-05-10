using FluentValidation;
using Grpc.Core;
using Grpc.Core.Testing;
using Loyalty.Api.Interceptors;
using Xunit;

namespace Loyalty.UnitTests.ApiTests;

public class GlobalGrpcExceptionInterceptorTests
{
    private readonly GlobalGrpcExceptionInterceptor _interceptor = new();

    private class DummyRequest
    {
    }

    private class DummyResponse
    {
    }

    private static ServerCallContext CreateContext()
    {
        return TestServerCallContext.Create(
            method: "dummyMethod",
            host: "localhost",
            deadline: DateTime.UtcNow.AddHours(1),
            requestHeaders: [],
            cancellationToken: CancellationToken.None,
            peer: "127.0.0.1",
            authContext: null,
            contextPropagationToken: null,
            writeHeadersFunc: _ => Task.CompletedTask,
            writeOptionsGetter: () => new WriteOptions(),
            writeOptionsSetter: _ => { }
        );
    }


    [Fact]
    public async Task UnaryServerHandler_ArgumentException_ShouldMapToInvalidArgument()
    {
        var context = CreateContext();

        var ex = await Assert.ThrowsAsync<RpcException>(async () =>
        {
            await _interceptor.UnaryServerHandler<DummyRequest, DummyResponse>(
                request: new DummyRequest(),
                context: context,
                continuation: (_, _) => throw new ArgumentException("Bad argument"));
        });

        Assert.Equal(StatusCode.InvalidArgument, ex.StatusCode);
        Assert.Equal("Bad argument", ex.Status.Detail);
    }

    [Fact]
    public async Task UnaryServerHandler_ValidationException_ShouldMapToInvalidArgument()
    {
        var context = CreateContext();

        var ex = await Assert.ThrowsAsync<RpcException>(async () =>
        {
            await _interceptor.UnaryServerHandler<DummyRequest, DummyResponse>(
                request: new DummyRequest(),
                context: context,
                continuation: (_, _) => throw new ValidationException("Validation failed"));
        });

        Assert.Equal(StatusCode.InvalidArgument, ex.StatusCode);
        Assert.Equal("Validation failed", ex.Status.Detail);
    }

    [Fact]
    public async Task UnaryServerHandler_UnknownException_ShouldMapToInternal()
    {
        var context = CreateContext();

        var ex = await Assert.ThrowsAsync<RpcException>(async () =>
        {
            await _interceptor.UnaryServerHandler<DummyRequest, DummyResponse>(
                request: new DummyRequest(),
                context: context,
                continuation: (_, _) => throw new Exception("Boom"));
        });

        Assert.Equal(StatusCode.Internal, ex.StatusCode);
        Assert.Equal("Internal server error", ex.Status.Detail);
    }
}