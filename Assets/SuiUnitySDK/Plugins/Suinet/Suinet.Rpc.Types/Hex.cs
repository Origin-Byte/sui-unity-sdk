namespace Suinet.Rpc.Types
{
    public class Hex
    {
        private readonly string value;

        public Hex(string value)
        {
            this.value = value;
        }

        public static implicit operator Hex(string value)
        {
            return new Hex(value);
        }

        public static implicit operator string(Hex hex)
        {
            return hex.value;
        }
    }
}
