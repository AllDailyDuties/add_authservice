﻿using AllDailyDuties_AuthService.Models.Users;
using RabbitMQ.Client;

namespace AllDailyDuties_AuthService.Services.Interfaces
{
    public interface IUserService
    {
        AuthResponse Authenticate(AuthRequest model);
        IEnumerable<User> GetAll();
        User GetById(Guid id);
        void Create(CreateRequest model);
        void SendUserModel(Guid uid, IBasicProperties props);
    }
}
