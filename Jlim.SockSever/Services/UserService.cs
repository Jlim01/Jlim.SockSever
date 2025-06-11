using RestaurantHost.Main.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RestaurantHost.Main.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> Logger;
        public UserService(ILogger<UserService> logger)
        {
            Logger = logger;
        }
        public async Task SaveUserDataAsync()
        {
            Logger.LogInformation("Starting to save user data");
            await Task.Delay(1000);
            Logger.LogInformation("User data saved successfully");
        }
    }
}
