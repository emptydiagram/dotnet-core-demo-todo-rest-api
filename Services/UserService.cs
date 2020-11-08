using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoMysqlApi.Models;
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;

namespace TodoMysqlApi.Services
{
  public interface IUserService
  {
    Task<User> Authenticate(string username, string password);
  }

  public class UserService : IUserService
  {

    private readonly TodoContext _context;

    public UserService(TodoContext context)
    {
      _context = context;
    }

    public async Task<User> Authenticate(string username, string password)
    {
      var user = await Task.Run(() =>
        _context.Users.SingleOrDefault(u => u.UserName == username));

      user = verifyUserPassword(user, password) ? user : null;

      // return null if user not found
      if (user == null) {
        return null;
      }
      return user;
    }

    private static bool verifyUserPassword(User user, string password)
    {
      if (user == null) return false;
      var pwHasher = new PasswordHasher<User>();
      var result = pwHasher.VerifyHashedPassword(user, user.PasswordHash, password);
      Console.WriteLine($"verify result = {result}");
      return result == PasswordVerificationResult.Success;
    }
  }

}