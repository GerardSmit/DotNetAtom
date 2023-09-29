using System.Data;
using DotNetAtom.Application;
using DotNetNuke.Abstractions.Application;
using DotNetNuke.Common;
using DotNetNuke.Data.PetaPoco;
using DotNetNuke.Entities.Users;

namespace DotNetAtom.Applications;

public class ApplicationRepository : IApplicationRepository
{
    private readonly IDnnContext _dnnContext;

    public ApplicationRepository(IDnnContext dnnContext)
    {
        _dnnContext = dnnContext;
    }

    public Task<Guid?> GetApplicationId()
    {
        // TODO: Validate if this is correct.
        // Source: https://github.com/dnnsoftware/Dnn.Platform/blob/421210f9a20d1c272db77e7208f7a5e960011534/DNN%20Platform/Modules/DnnExportImport/Components/Services/UsersExportService.cs#L497-L508

        var connectionStringName = DotNetNuke.Data.DataProvider.Instance().Settings["connectionStringName"];
        using var db = new PetaPocoDataContext(connectionStringName, "aspnet_");

        return Task.FromResult<Guid?>(
            db.ExecuteScalar<Guid>(
                CommandType.Text,
                "SELECT TOP 1 ApplicationId FROM aspnet_Applications"));
    }
}
