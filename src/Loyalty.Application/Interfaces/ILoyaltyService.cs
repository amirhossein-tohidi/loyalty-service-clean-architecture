using Loyalty.Application.DTOs;

namespace Loyalty.Application.Interfaces;

public interface ILoyaltyService
{
    LoyaltyResponseDto Calculate(LoyaltyRequestDto request);
}