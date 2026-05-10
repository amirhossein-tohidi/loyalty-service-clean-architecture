using Grpc.Core;
using Grpc.Core.Interceptors;
using Loyalty.Domain.Exceptions;

namespace Loyalty.Api.Interceptors;

public class GlobalGrpcExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex) when (
            ex is ArgumentException
                or DomainValidationException
                or FluentValidation.ValidationException)
        {
            throw new RpcException(
                new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new RpcException(
                new Status(StatusCode.OutOfRange, ex.Message));
        }
        catch (Exception)
        {
            throw new RpcException(
                new Status(StatusCode.Internal, "Internal server error"));
        }
    }
}