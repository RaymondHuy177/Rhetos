﻿/*
    Copyright (C) 2014 Omega software d.o.o.

    This file is part of Rhetos.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Rhetos.Processing.DefaultCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rhetos.Dom.DefaultConcepts
{
    internal class ReflectionHelper<TEntityInterface>
    {
        private readonly string _entityName;
        private readonly IDomainObjectModel _domainObjectModel;
        private readonly Lazy<IRepository> _repository;

        public ReflectionHelper(string entityName, IDomainObjectModel domainObjectModel, Lazy<IRepository> repository)
        {
            _entityName = entityName;
            _domainObjectModel = domainObjectModel;
            _repository = repository;
        }

        //===================================================
        #region Types

        private Type _entityType = null;
        public Type EntityType
        {
            get
            {
                if (_entityType == null)
                {
                    _entityType = _domainObjectModel.GetType(_entityName);
                    if (!typeof(TEntityInterface).IsAssignableFrom(_entityType))
                        throw new FrameworkException(string.Format(
                            "The given data structure's type {0} does not implement {1} interface.",
                            _entityType.FullName, typeof(TEntityInterface).FullName));
                }
                return _entityType;
            }
        }

        private Type _enumerableType = null;
        public Type EnumerableType
        {
            get
            {
                if (_enumerableType == null)
                    _enumerableType = typeof(IEnumerable<>).MakeGenericType(new[] { EntityType });
                return _enumerableType;
            }
        }

        private Type _listType = null;
        public Type ListType
        {
            get
            {
                if (_listType == null)
                    _listType = typeof(List<>).MakeGenericType(new[] { EntityType });
                return _listType;
            }
        }

        private Type _queryableType = null;
        public Type QueryableType
        {
            get
            {
                if (_queryableType == null)
                    _queryableType = typeof(IQueryable<>).MakeGenericType(new[] { EntityType });
                return _queryableType;
            }
        }

        private Type _repositoryType = null;
        public Type RepositoryType
        {
            get
            {
                if (_repositoryType == null)
                    _repositoryType = _repository.Value.GetType();
                return _repositoryType;
            }
        }

        #endregion
        //===================================================
        #region Methods

        private MethodInfo _asQueryableMethod = null;
        public MethodInfo AsQueryableMethod
        {
            get
            {
                if (_asQueryableMethod == null)
                    _asQueryableMethod = typeof(Queryable).GetMethod("AsQueryable", new[] { EnumerableType });
                return _asQueryableMethod;
            }
        }
        public IQueryable<TEntityInterface> AsQueryable(IEnumerable<TEntityInterface> items)
        {
            return (IQueryable<TEntityInterface>)AsQueryableMethod.Invoke(null, new object[] { items });
        }

        private MethodInfo _addRangeMethod = null;
        public MethodInfo AddRangeMethod
        {
            get
            {
                if (_addRangeMethod == null)
                    _addRangeMethod = ListType.GetMethod("AddRange");
                return _addRangeMethod;
            }
        }
        public void AddRange(IEnumerable<TEntityInterface> list, IEnumerable<TEntityInterface> items)
        {
            AddRangeMethod.Invoke(list, new object[] { items });
        }

        private MethodInfo _castEntityMethod = null;
        public MethodInfo CastEntityMethod
        {
            get
            {
                if (_castEntityMethod == null)
                    _castEntityMethod = typeof(Enumerable).GetMethod("Cast", BindingFlags.Public | BindingFlags.Static)
                        .MakeGenericMethod(new[] { EntityType });
                return _castEntityMethod;
            }
        }
        public IEnumerable<TEntityInterface> CastEntity(IEnumerable<object> items)
        {
            return (IEnumerable<TEntityInterface>)CastEntityMethod.Invoke(null, new object[] { items });
        }

        private MethodInfo _toListEntityMethod = null;
        public MethodInfo ToListEntityMethod
        {
            get
            {
                if (_toListEntityMethod == null)
                    _toListEntityMethod = typeof(Enumerable).GetMethod("ToList", BindingFlags.Public | BindingFlags.Static)
                        .MakeGenericMethod(new[] { EntityType });
                return _toListEntityMethod;
            }
        }
        public IEnumerable<TEntityInterface> ToListEntity(IEnumerable<TEntityInterface> items)
        {
            return (IEnumerable<TEntityInterface>)ToListEntityMethod.Invoke(null, new object[] { items });
        }

        #endregion
        //===================================================
        #region Repository methods

        private MethodInfo _repositoryLoadMethod = null;
        public MethodInfo RepositoryLoadMethod
        {
            get
            {
                if (_repositoryLoadMethod == null)
                    _repositoryLoadMethod = RepositoryType.GetMethod("All", new Type[] { }); // TODO: Rename All to Load
                return _repositoryLoadMethod;
            }
        }

        private MethodInfo _repositoryQueryMethod = null;
        public MethodInfo RepositoryQueryMethod
        {
            get
            {
                if (_repositoryQueryMethod == null)
                    _repositoryQueryMethod = RepositoryType.GetMethod("Query", new Type[] { });
                return _repositoryQueryMethod;
            }
        }

        private Dictionary<Type, MethodInfo> _repositoryLoadWithParameterMethod = null;
        public MethodInfo RepositoryLoadWithParameterMethod(Type parameterType)
        {
            MethodInfo method = null;
            bool exists = false;

            if (_repositoryLoadWithParameterMethod == null)
                _repositoryLoadWithParameterMethod = new Dictionary<Type, MethodInfo>();
            else
                exists = _repositoryLoadWithParameterMethod.TryGetValue(parameterType, out method);

            if (!exists)
            {
                method = RepositoryType.GetMethod("Filter", new Type[] { parameterType }); // TODO: Rename Filter to Load
                _repositoryLoadWithParameterMethod.Add(parameterType, method);
            }

            return method;
        }

        private Dictionary<Type, MethodInfo> _repositoryQueryableFilterMethod = null;
        public MethodInfo RepositoryQueryableFilterMethod(Type parameterType)
        {
            MethodInfo method = null;
            bool exists = false;

            if (_repositoryQueryableFilterMethod == null)
                _repositoryQueryableFilterMethod = new Dictionary<Type, MethodInfo>();
            else
                exists = _repositoryQueryableFilterMethod.TryGetValue(parameterType, out method);

            if (!exists)
            {
                method = RepositoryType.GetMethod("Filter", new Type[] { QueryableType, parameterType });
                _repositoryQueryableFilterMethod.Add(parameterType, method);
            }

            return method;
        }

        private MethodInfo _repositorySaveMethod = null;
        public MethodInfo RepositorySaveMethod
        {
            get
            {
                if (_repositorySaveMethod == null)
                    _repositorySaveMethod = RepositoryType.GetMethod("Save", new Type[] { EnumerableType, EnumerableType, EnumerableType, typeof(bool) });
                return _repositorySaveMethod;
            }
        }

        private MethodInfo _repositoryQueryDataSourceCommandMethod = null;
        public MethodInfo RepositoryQueryDataSourceCommandMethod
        {
            get
            {
                if (_repositoryQueryDataSourceCommandMethod == null)
                    _repositoryQueryDataSourceCommandMethod = RepositoryType.GetMethod("QueryData", new Type[] { typeof(QueryDataSourceCommandInfo) });
                return _repositoryQueryDataSourceCommandMethod;
            }
        }

        #endregion
    }
}
