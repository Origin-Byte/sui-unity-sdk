using System.Collections.Generic;

namespace Suinet.Rpc.Types.MoveTypes
{
    public abstract class SuiMoveNormalizedType { }

    public class SuiMoveNormalizedTypeString : SuiMoveNormalizedType
    {
        public string Value { get; set; }
    }

    public class SuiMoveNormalizedTypeStruct : SuiMoveNormalizedType
    {
        public StructType Struct { get; set; }

        public class StructType
        {
            public string Address { get; set; }
            public string Module { get; set; }
            public string Name { get; set; }
            public List<SuiMoveNormalizedType> TypeArguments { get; set; }
        }
    }

    public class SuiMoveNormalizedTypeVector : SuiMoveNormalizedType
    {
        public SuiMoveNormalizedType Vector { get; set; }
    }

    public class SuiMoveNormalizedTypeParameter : SuiMoveNormalizedType
    {
        public ushort TypeParameter { get; set; }
    }

    public class SuiMoveNormalizedTypeReference : SuiMoveNormalizedType
    {
        public SuiMoveNormalizedType Reference { get; set; }
    }

    public class SuiMoveNormalizedTypeMutableReference : SuiMoveNormalizedType
    {
        public SuiMoveNormalizedType MutableReference { get; set; }
    }
}
