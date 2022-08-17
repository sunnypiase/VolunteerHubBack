using Application.Services;
using Application.UnitOfWorks;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Commands.Users
{
    public record LoginUserCommand : IRequest<string>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HashingService _hashingService;
        public LoginUserCommandHandler(IUnitOfWork unitOfWork, HashingService hashingService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _hashingService = hashingService;
            _configuration = configuration;
        }
        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHash = _hashingService.GetHash(request.Password);
            var users = await _unitOfWork.Users.Get(user => user.Email == request.Login && user.Password == passwordHash);
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new Exception();// TODO: Make here custom exception
            }

            return GenerateJwtToken(user);
        }
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("Id", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
