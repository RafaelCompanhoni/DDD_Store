using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using LuaBijoux.Core.Data;
using LuaBijoux.Core.DomainModels;
using LuaBijoux.Core.DomainModels.Identity;

namespace LuaBijoux.Core.Identity
{
    public interface IApplicationUserManager : IDisposable
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        IEnumerable<AppUser> GetUsers();
        PaginatedList<AppUser> GetUsers(Expression<Func<AppUser, string>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        PaginatedList<AppUser> GetUsers(int pageIndex, int pageSize, Expression<Func<AppUser, string>> keySelector, Expression<Func<AppUser, bool>> predicate, OrderBy orderBy = OrderBy.Ascending);
        Task<ApplicationIdentityResult> CreateAsync(AppUser user, string password);
        Task<AppUser> FindByIdAsync(int userId);
        Task<AppUser> FindByEmailAsync(string email);
        AppUser FindByEmail(string email);
        Task<AppUser> FindByCpfAsync(string cpf);
        AppUser FindByCpf(string cpf);
        Task<ApplicationIdentityResult> UpdateAsync(AppUser user);
        Task<ApplicationIdentityResult> DeleteAsync(int userId);
    }
}