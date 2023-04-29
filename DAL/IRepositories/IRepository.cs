﻿using MODEL.Entities;
using System.Linq.Expressions;

namespace DAL.IRepositories
{
    public interface IRepository<S> where S : BaseEntity
    {
        /// <summary> Method of geting entity objects. </summary>
        /// <typeparam name="T"> Class that implements BaseEntity. </typeparam>
        /// <param name="predicate"> Expression for filtering. </param>
        /// <returns> Query based on parameters. </returns>
        public IQueryable<T> SearchFor<T>(Expression<Func<T, bool>> predicate, bool validOnly = true) where T : BaseEntity;

        /// <summary> Get entity with given Id. </summary>
        /// <typeparam name="T"> Entity of BaseEntity. </typeparam>
        /// <param name="id"> Id of entity which we want to return</param>
        /// <param name="validOnly"> Bool parametrar for returning relevant data </param>
        /// <returns> Return list of entites. </returns>
        public Task<T?> GetById<T>(Guid id, bool validOnly = true) where T : BaseEntity;

        /// <summary> Method which save changes on only one entity. </summary>
        /// <typeparam name="T"> Entit</typeparam>
        /// <param name="entity"> Entites based on BaseEntity. </param>
        /// <returns> Return true if the save is successful. </returns>
        public void Save<T>(T entity) where T : BaseEntity;
    }
}