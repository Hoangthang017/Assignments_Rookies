using ECommerce.Models.Entities;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.IdentityServer
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
        private readonly UserManager<User> _userManager;

        public ProfileService(
            UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);

            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();

            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            //var user = await _userManager.GetUserAsync(context.Subject);

            claims.AddRange(new List<Claim>()
            {
                new Claim(JwtClaimTypes.GivenName, user.UserName),
                new Claim("displayName", user.LastName + " "+ user.FirstName ),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("dateOfBirth", user.DateOfBirth.ToString()),
                new Claim(IdentityServerConstants.StandardScopes.Email, user.Email),
                new Claim(IdentityServerConstants.StandardScopes.Phone, user.PhoneNumber)
            });

            var role = await _userManager.GetRolesAsync(user);
            var isAdmin = role.Contains("admin");

            if (isAdmin)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "admin"));
            }
            else
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "user"));
            }

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}