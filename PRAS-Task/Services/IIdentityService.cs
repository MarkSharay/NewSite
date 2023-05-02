using Microsoft.AspNetCore.Identity;
using PRAS_Task.Models;

namespace PRAS_Task.Services
{
    public interface IIdentityService
    {
        LoginResponse Login(IdentityUser identityUser);

    }
}
