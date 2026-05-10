using Loyalty.Application.DTOs;
using Loyalty.Application.Interfaces;
using Loyalty.Application.Mapping;
using Loyalty.Application.Validators;
using Loyalty.Domain.Interfaces;

namespace Loyalty.Application.Services;

public class LoyaltyService(ILoyaltyCalculatorService calculator, IFileStorageService fileStorage) : ILoyaltyService
{
    public LoyaltyResponseDto Calculate(LoyaltyRequestDto dto)
    {
        LoyaltyRequestValidator.Validate(dto);

        var domainInput = dto.ToDomain();

        var score = calculator.CalculateFinalScore(domainInput);

        var response = score.ToDto();

        fileStorage.Save(new
        {
            UtcTime = DateTime.UtcNow,
            Operation = "LoyaltyCalculation",
            Request = dto,
            Response = response
        });

        return response;
    }
}