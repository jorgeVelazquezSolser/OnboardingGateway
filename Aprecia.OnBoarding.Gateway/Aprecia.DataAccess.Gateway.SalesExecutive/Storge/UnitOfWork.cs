
using System.Data;
using Aprecia.OnBoarding.Gateway.SalesExecutive.Storage;
using Microsoft.Data.SqlClient;

namespace Aprecia.DataAccess.Gateway.SalesExecutive.Storge;

public class UnitOfWork : IUnitOfWorks
{
    private readonly IDbConnection _dbConnection;
    private readonly IDbTransaction _dbTransaction;

    public ISalesExecutiveStorage SalesExecutiveStorage { get; }

    public UnitOfWork(string connectionString)
    {
        _dbConnection = new SqlConnection(connectionString);
        _dbConnection.Open();
        _dbTransaction = _dbConnection.BeginTransaction();
        SalesExecutiveStorage = new SalesExecutiveStorage(_dbTransaction);
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
