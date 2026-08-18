namespace HospitalSaoJose.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}
