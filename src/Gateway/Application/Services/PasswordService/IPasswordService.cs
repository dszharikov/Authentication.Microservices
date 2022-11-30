using Gateway.Dtos;

namespace Gateway.Application.Services.PasswordService;

public interface IPasswordService
{
    public Task<IResult> GeneratePassword(PasswordGeneratorDto passwordGeneratorDto);
}