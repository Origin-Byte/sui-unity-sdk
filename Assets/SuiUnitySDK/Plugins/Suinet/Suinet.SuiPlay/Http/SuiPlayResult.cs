namespace Suinet.SuiPlay.Http
{
    public class SuiPlayResult<T>
    {
        public T Value { get; set; }
        public string Error { get; set; }
        public bool IsSuccess => Error == null;
    }
}
