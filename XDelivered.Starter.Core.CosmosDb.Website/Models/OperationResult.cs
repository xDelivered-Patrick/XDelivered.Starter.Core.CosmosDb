namespace XDelivered.StarterKits.NgCoreCosmosDb.Modals
{
    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }

        public void SetData(object o)
        {
            if (o is T payload)
                Data = payload;
        }
    }

    public class OperationResult
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        
        public void AddMessage(string message)
        {
            Message = message;
        }
    }
}