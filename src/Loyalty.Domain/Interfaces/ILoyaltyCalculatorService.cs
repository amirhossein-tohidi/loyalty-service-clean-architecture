using Loyalty.Domain.Models;
using Loyalty.Domain.Results;

namespace Loyalty.Domain.Interfaces;

public interface ILoyaltyCalculatorService
{
    LoyaltyResult CalculateFinalScore(LoyaltyCalculationInput input);
}