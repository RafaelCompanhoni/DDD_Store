using System;
using System.Data.Entity;
using LuaBijoux.Core.DomainModels;
using LuaBijoux.Core.Logging;
using LuaBijoux.Data.Identity;
using LuaBijoux.Data.Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LuaBijoux.Data
{
    // Impede que o banco seja deletado quando existirem novas migrations
    public class ApplicationDbInitializer : NullDatabaseInitializer<LuaBijouxContext> {}
}