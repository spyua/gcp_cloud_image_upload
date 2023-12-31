﻿using cbk.image.Domain.Entity;

namespace cbk.image.Infrastructure.Repository
{
    public interface IAccountRepository : IBaseRepository
    {
        Task<Account?> GetByName(string name);

        void Add(Account account);
    }
}
