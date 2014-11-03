using System;
using LuaBijoux.Core.Data;

namespace LuaBijoux.Core.Services
{
    public interface IServiceUoW : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
