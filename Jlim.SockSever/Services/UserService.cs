using RestrauntHost.Main.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestrauntHost.Main.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }
        public async Task SaveUserDataAsync()
        {
            _logger.LogInformation("Starting to save user data");
            await Task.Delay(1000);
            _logger.LogInformation("User data saved successfully");
        }
    }
}
