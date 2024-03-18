using Connect2Gether_API.Models.Dtos;

namespace Connect2Gether_API.Serivce.IService
{
    public interface IAuth
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<bool> AssignPermision(string email, string permisionName);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
