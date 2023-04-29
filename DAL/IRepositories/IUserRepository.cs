﻿using MODEL.Entities;

namespace DAL.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<Role?> GetRoleByCode(string code, bool? isDeleted = false);
        public Task<User?> GetUserById(Guid id);
        public Task<User?> GetUserByEmail(string email);
    }
}