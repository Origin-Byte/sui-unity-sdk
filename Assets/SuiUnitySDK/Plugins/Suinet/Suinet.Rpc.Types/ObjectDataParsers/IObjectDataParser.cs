using System.Text.RegularExpressions;

namespace Suinet.Rpc.Types.ObjectDataParsers
{
    public interface IObjectDataParser<T> where T : class
    {
        T Parse(ObjectData objectData);

        Regex TypeRegex { get; }
    }
}
