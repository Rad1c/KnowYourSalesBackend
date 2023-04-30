using MODEL.Entities;

namespace DAL.IRepositories;

public interface IRepository<S> where S : BaseEntity
{
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

    /// <summary>
    /// Update entity.
    /// </summary>
    /// <typeparam name="T"> Entity</typeparam>
    /// <param name="entity"> Entites based on BaseEntity.</param>
    public void UpdateEntity<T>(T entity) where T : BaseEntity;

    public Task<Role?> GetRoleByCode(string code, bool? isDeleted = false);
}
