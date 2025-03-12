using System.Data;
using Aprecia.Bussines.Gateway.Authorization.Storage;
using Aprecia.Domain.Gateway.Authorization.Dtos;
using Microsoft.Data.SqlClient;

namespace Aprecia.DataAccess.Gateway.Authorization.Storage;

public class UnitOfWork : IUnitOfWorks
{
    private readonly IDbConnection _dbConnection;
    private readonly IDbTransaction _dbTransaction;

    public IAuthorizationStorage AuthorizationStorage { get; }

    public UnitOfWork(string connectionString)
    {
        _dbConnection = new SqlConnection(connectionString);
        _dbConnection.Open();
        _dbTransaction = _dbConnection.BeginTransaction();
        AuthorizationStorage = new AuthorizationStorage(_dbTransaction);
    }

    public void Commit()
    {
        try
        {
            _dbTransaction.Commit();
        }
        catch (Exception)
        {
            _dbTransaction.Rollback();
            throw;
        }
        finally { _dbTransaction.Dispose(); }
    }
    public void Rollback()
    {
        try
        {
            _dbTransaction.Rollback();
        }
        catch (Exception)
        {
            throw;
        }
        finally { _dbTransaction.Dispose(); }
    }
    public void Disponse()
    {
        Disponse(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Disponse(bool disponsing)
    {

        if (disponsing)
        {
            _dbTransaction?.Dispose();
            _dbConnection?.Dispose();
        }
    }
    ~UnitOfWork() 
    {
        Disponse(false);
    }

}
