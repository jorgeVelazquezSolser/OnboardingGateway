using System.Data;
using Aprecia.Domain.Gateway.SalesExecutive.Dtos;
using Aprecia.OnBoarding.Gateway.SalesExecutive.Storage;
using Dapper;

namespace Aprecia.DataAccess.Gateway.SalesExecutive.Storge;

public class SalesExecutiveStorage : ISalesExecutiveStorage
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _dbConnection;

    public SalesExecutiveStorage(IDbTransaction dbTransaction)
    {
        _dbTransaction = dbTransaction;
        _dbConnection = _dbTransaction.Connection!;
    }
    public async Task<IEnumerable<GetSalesExecutiveResponseDto>> GetSalesExecutives(CancellationToken cancellationToken)
    {
        try
        {

            var result = await Task.Run(() => _dbConnection.QueryAsync<GetSalesExecutiveResponseDto>(
                                            "proc_Orgn_ejecutivos_extrae",
                                            transaction: _dbTransaction,
                                            commandType: CommandType.StoredProcedure
                                            ), cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            throw;
        }

    }
}