using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Models.Dapper;
using Server.DAOs;
using Server.Utils;

namespace Server.Services.Authentication
{
    public class UserService : IUserService
    {

        private readonly UserDAO _userDao;
        private readonly RefreshTokenDAO _refreshTokenDao;
        private readonly IPasswordEncoder _passwordEncoder;

        public UserService(UserDAO userDao, RefreshTokenDAO refreshTokenDao, IPasswordEncoder passwordEncoder)
        {
            _userDao = userDao;
            _refreshTokenDao = refreshTokenDao;
            _passwordEncoder = passwordEncoder;
        }

        public async Task<Optional<User>> Register(string username, string email, string password)
        {
            string hashedPassword = _passwordEncoder.HashPassword(password);
            byte[] bytes = Encoding.UTF8.GetBytes(hashedPassword);

            return await _userDao.Save(username, email, bytes);
        }

        public async Task<Optional<User>> Login(string email, string password)
        {
            var user = await _userDao.GetUserByEmail(email);

            if (user.IsEmpty)
            {
                return Optional<User>.Empty();
            }

            string hash = Encoding.UTF8.GetString(user.Value.Password);

            // check if password match with hash
            if (_passwordEncoder.Verify(password, hash))
            {
                return Optional<User>.of(new User
                {
                    Id = user.Value.Id,
                    Name = user.Value.Name,
                    Email = user.Value.Email
                });
            }

            return Optional<User>.Empty();
        }

        public async Task<Optional<User>> Login(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task Logout(int id)
        {
            await _refreshTokenDao.Logout(id);
        }
    }
}
