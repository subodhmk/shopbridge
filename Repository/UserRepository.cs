using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shopbridge_base.Data;
using Shopbridge_base.Models;
using Shopbridge_base.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shopbridge_base.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Shopbridge_Context _db;
        private readonly AppSettings _appSettings;

        public UserRepository(Shopbridge_Context db, IOptions<AppSettings> appsettings)
        {
            _db = db;
            _appSettings = appsettings.Value;
        }

        public User Authenticate(string username, string password)
        {
            try
            {
                var user = _db.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

                if (user == null)
                {
                    return null;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescrptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {

                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var Token = tokenHandler.CreateToken(tokenDescrptor);
                user.Token = tokenHandler.WriteToken(Token);
                user.Password = "";
                return user;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public bool IsUniqueUser(string username)
        {
            try
            {
                var user = _db.Users.SingleOrDefault(x => x.Username == username);

                if (user == null)
                    return true;

                return false;
            }
            catch (Exception Ex)
            {
                return false;               
            }
        }

        public User Register(string username, string password)
        {
            try
            {
                User objuser = new User()
                {
                    Username = username,
                    Password = password,
                    Role = "Admin"
                };

                _db.Users.Add(objuser);
                _db.SaveChanges();
                objuser.Password = "";
                return objuser;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }
    }
}
