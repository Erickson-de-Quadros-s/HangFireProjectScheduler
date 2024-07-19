namespace TaskScheduler.Model
{
    public class ResponseModel<T>
    {
        public int StatusCode { get; set; }
        public string? StatusMessage { get; set; }
        public T? Data { get; set; }

        public ResponseModel() { }

        public ResponseModel(int statusCode, string statusMessage, T? data)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            Data = data;
        }
    }
}