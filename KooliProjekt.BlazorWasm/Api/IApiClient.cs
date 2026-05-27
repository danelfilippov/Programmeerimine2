using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.BlazorWasm
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<User>>> List(int page, int pageSize);
        Task<OperationResult> Save(User list);
        Task<OperationResult> Delete(int id);
    }
}