namespace HospitalSaoJose.Domain.Repositories.Category;

public interface ICategoryUpdateOnlyRepository
{
    Task<Entities.Category?> GetById(Guid id);

    void Update(Entities.Category category);
}
