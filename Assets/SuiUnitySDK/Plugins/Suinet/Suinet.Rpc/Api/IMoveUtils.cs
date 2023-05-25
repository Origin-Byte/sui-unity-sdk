using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Suinet.Rpc.Api
{
    public interface IMoveUtils
    {
        /// <summary>
        /// Return the argument types of a Move function, based on normalized Type.
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        Task<RpcResult<MoveFunctionArgType[]>> GetMoveFunctionArgTypesAsync(string packageId, string module, string function);

        /// <summary>
        /// Return a structured representation of Move function
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="module"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        Task<RpcResult<SuiMoveNormalizedFunction>> GetNormalizedMoveFunctionAsync(string packageId, string module, string function);

        /// <summary>
        /// Return a structured representation of Move module
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        Task<RpcResult<SuiMoveNormalizedModule>> GetNormalizedMoveModuleAsync(string packageId, string module);

        /// <summary>
        /// Return structured representations of all modules in the given package
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        Task<RpcResult<Dictionary<string, SuiMoveNormalizedModule>>> GetNormalizedMoveModulesByPackageAsync(string packageId);

        /// <summary>
        /// Return a structured representation of Move struct
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="module"></param>
        /// <param name="structName"></param>
        /// <returns></returns>
        Task<RpcResult<SuiMoveNormalizedStruct>> GetNormalizedMoveStructAsync(string packageId, string module, string structName);
    }
}
