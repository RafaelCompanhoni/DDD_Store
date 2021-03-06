﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LuaBijoux.Core.Data;
using LuaBijoux.Core.DomainModels;
using LuaBijoux.Core.DomainModels.Identity;
using LuaBijoux.Core.Extensions;
using LuaBijoux.Data.Extensions;
using LuaBijoux.Data.Identity.Models;
using Microsoft.AspNet.Identity;
using LuaBijoux.Core.Identity;
using Microsoft.Owin.Security;

namespace LuaBijoux.Data.Identity
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly UserManager<ApplicationIdentityUser, int> _userManager;
        private readonly IAuthenticationManager _authenticationManager;
        private bool _disposed;

        public ApplicationUserManager(UserManager<ApplicationIdentityUser, int> userManager, IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync().ConfigureAwait(false);
            return users.ToAppUserList();
        }

        public IEnumerable<AppUser> GetUsers()
        {
            var users = _userManager.Users.ToList();
            return users.ToAppUserList();
        }

        public PaginatedList<AppUser> GetUsers(Expression<Func<AppUser, string>> keySelector, OrderBy orderBy = OrderBy.Ascending)
        {
            // TODO - ao obter users com ToAppUserList ocorre uma chamada ao banco (retorna IEnumerable). O ideal é retornar um IQueryable(AppUser) e apenas
            // depois de feita a ordenação, retornar a lista (onde ocorrerá então a execução da query)

            var users = _userManager.Users.ToAppUserList().AsQueryable();
            users = (orderBy == OrderBy.Ascending) ? users.OrderBy(keySelector) : users.OrderByDescending(keySelector);
            return new PaginatedList<AppUser>(users, 0, 50, users.Count());
        }

        public PaginatedList<AppUser> GetUsers(int pageIndex, int pageSize, Expression<Func<AppUser, string>> keySelector, Expression<Func<AppUser, bool>> predicate, OrderBy orderBy = OrderBy.Ascending)
        {
            var users = GetUsers().AsQueryable();
            users = (predicate != null) ? users.Where(predicate) : users;
            users = (orderBy == OrderBy.Ascending) ? users.OrderBy(keySelector) : users.OrderByDescending(keySelector);
            var total = users.Count();
            users.Paginate(pageIndex, pageSize);
            return new PaginatedList<AppUser>(users, pageIndex, pageSize, total);
        }

        public async Task<AppUser> FindByIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            return user.ToAppUser();
        }

        public async Task<AppUser> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
            return user.ToAppUser();
        }

        public AppUser FindByEmail(string email)
        {
            var user = _userManager.FindByEmail(email);
            return user.ToAppUser();
        }

        public async Task<AppUser> FindByCpfAsync(string cpf)
        {
            var users = await _userManager.Users.ToListAsync().ConfigureAwait(false);
            return users.FirstOrDefault(u => u.Cpf == cpf).ToAppUser();
        }

        public AppUser FindByCpf(string cpf)
        {
            var users = _userManager.Users.ToList();
            return users.FirstOrDefault(u => u.Cpf == cpf).ToAppUser();
        }

        public async Task<ApplicationIdentityResult> CreateAsync(AppUser user, string password)
        {
            var applicationUser = user.ToApplicationUser();
            var identityResult = await _userManager.CreateAsync(applicationUser, password).ConfigureAwait(false);
            user.CopyApplicationIdentityUserProperties(applicationUser);
            return identityResult.ToApplicationIdentityResult();
        }

        public async Task<ApplicationIdentityResult> UpdateAsync(AppUser user)
        {
            ApplicationIdentityUser applicationUser = _userManager.FindById(user.Id);
            applicationUser.CopyAppUserProperties(user);
            var identityResult = await _userManager.UpdateAsync(applicationUser);
            return identityResult.ToApplicationIdentityResult();
        }

        public async Task<ApplicationIdentityResult> DeleteAsync(int userId)
        {
            var applicationUser = await _userManager.FindByIdAsync(userId);
            if (applicationUser == null)
            {
                return new ApplicationIdentityResult(new[] { "Nenhum usuário encontrado com o Id informado." }, false);
            }
            var identityResult = await _userManager.DeleteAsync(applicationUser).ConfigureAwait(false);
            return identityResult.ToApplicationIdentityResult();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
