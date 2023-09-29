using System;
using System.Threading.Tasks;

namespace DotNetAtom.Application;

public interface IApplicationRepository
{
    Task<Guid?> GetApplicationId();
}
