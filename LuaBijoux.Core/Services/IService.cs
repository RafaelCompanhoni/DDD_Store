using System;
using LuaBijoux.Core.Data;

namespace LuaBijoux.Core.Services
{
    public interface IService : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
