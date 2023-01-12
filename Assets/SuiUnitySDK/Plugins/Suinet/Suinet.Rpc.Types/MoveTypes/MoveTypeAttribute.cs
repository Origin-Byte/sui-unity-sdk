using System;

namespace Suinet.Rpc.Types.MoveTypes
{
    public class MoveTypeAttribute : Attribute
    {
        /// <summary>
        /// Can be a regex
        /// </summary>
        public string Type { get; set; }

        public MoveTypeAttribute(string type)
        {
            Type = type;
        }
    }
}