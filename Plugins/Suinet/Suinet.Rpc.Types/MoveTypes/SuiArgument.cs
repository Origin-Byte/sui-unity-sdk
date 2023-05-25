using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;

namespace Suinet.Rpc.Types.MoveTypes
{
    public enum SuiArgumentType
    {
        GasCoin,
        Input,
        Result,
        NestedResult
    }
    
    [JsonConverter(typeof(SuiArgumentConverter))]
    public abstract class SuiArgument
    {
        public SuiArgumentType ArgumentType { get; set; }
    }

    public class GasCoinArgument : SuiArgument
    {
        public GasCoinArgument()
        {
            ArgumentType = SuiArgumentType.GasCoin;
        }
    }

    public class InputArgument : SuiArgument
    {
        public ushort Input { get; set; }

        public InputArgument()
        {
            ArgumentType = SuiArgumentType.Input;
        }
    }

    public class ResultArgument : SuiArgument
    {
        public ushort Result { get; set; }

        public ResultArgument()
        {
            ArgumentType = SuiArgumentType.Result;
        }
    }

    public class NestedResultArgument : SuiArgument
    {
        public ushort[] NestedResult { get; set; }

        public NestedResultArgument()
        {
            ArgumentType = SuiArgumentType.NestedResult;
        }
    }
}
