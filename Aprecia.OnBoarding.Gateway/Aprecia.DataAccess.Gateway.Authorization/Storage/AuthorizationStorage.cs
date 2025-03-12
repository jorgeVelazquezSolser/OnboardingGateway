using System.Data;
using Aprecia.Bussines.Gateway.Authorization.Storage;
using Aprecia.Domain.Gateway.Authorization.Dtos;
using Dapper;

namespace Aprecia.DataAccess.Gateway.Authorization.Storage;

public class AuthorizationStorage: IAuthorizationStorage
{
    private readonly IDbTransaction _dbTransaction;
    private readonly IDbConnection _dbConnection;

    public AuthorizationStorage(IDbTransaction dbTransaction) 
    {
        _dbTransaction = dbTransaction;
        _dbConnection = _dbTransaction.Connection!;
    }
    public async Task<ParamResponseDto> GetParamsByKey(string key, CancellationToken cancellationToken)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@key", key, DbType.String, ParameterDirection.Input);

            var result = await Task.Run(() => _dbConnection.QueryFirstOrDefaultAsync<ParamResponseDto>(
               "sp_get_params_by_Key",
               parameters,
               transaction: _dbTransaction,
               commandType: CommandType.StoredProcedure
           ), cancellationToken);

            return result;

        }
        catch (Exception) 
        {
            throw;
        }
        

        
    }

}
