﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities;
using Signum.Services;

namespace Signum.Windows
{
    public static class OperationServer
    {
        public static T Execute<T, B>(this T entity, ExecuteSymbol<B> symbol, params object[] args)
            where T : class, IIdentifiable
            where B : class, IIdentifiable, T
        {
            return (T)(IIdentifiable)Server.Return((IOperationServer s) => s.ExecuteOperation(entity, symbol.Operation, args)); 
        }

        public static T ExecuteLite<T, B>(this Lite<T> lite, ExecuteSymbol<B> symbol, params object[] args)
            where T : class, IIdentifiable
            where B : class, IIdentifiable, T
        {
            return (T)(IIdentifiable)Server.Return((IOperationServer s) => s.ExecuteOperationLite(lite, symbol.Operation, args)); 
        }

        public static void Delete<T, B>(this Lite<T> lite, DeleteSymbol<B> symbol, params object[] args)
            where T : class, IIdentifiable
            where B : class, IIdentifiable, T
        {
            Server.Execute((IOperationServer s) => s.Delete(lite, symbol.Operation, args)); 
        }

        public static T Construct<T>(ConstructSymbol<T>.Simple symbol, params object[] args)
            where T : class, IIdentifiable
        {
            return (T)(IIdentifiable)Server.Return((IOperationServer s) => s.Construct(typeof(T), symbol.Operation, args)); 
        }

        public static T ConstructFrom<F, FB, T>(this F entity, ConstructSymbol<T>.From<FB> symbol, params object[] args)
            where T : class, IIdentifiable
            where FB : class, IIdentifiable
            where F : class, IIdentifiable, FB
        {
            return (T)(IIdentifiable)Server.Return((IOperationServer s) => s.ConstructFrom(entity, symbol.Operation, args)); 
        }

        public static T ConstructFromLite<F, FB, T>(this Lite<F> lite, ConstructSymbol<T>.From<FB> symbol, params object[] args)
            where T : class, IIdentifiable
            where FB : class, IIdentifiable
            where F : class, IIdentifiable, FB
        {
            return (T)(IIdentifiable)Server.Return((IOperationServer s) => s.ConstructFromLite(lite, symbol.Operation, args)); 
        }

        public static T ConstructFromMany<F, FB, T>(List<Lite<F>> lites, ConstructSymbol<T>.FromMany<FB> symbol, params object[] args)
            where T : class, IIdentifiable
            where FB : class, IIdentifiable
            where F : class, IIdentifiable, FB
        {
            return (T)(IIdentifiable)Server.Return((IOperationServer s) => s.ConstructFromMany(lites, typeof(F), symbol.Operation, args)); 
        }
    }
}
