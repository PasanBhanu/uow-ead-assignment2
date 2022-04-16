namespace Finance_App_Service.REST
{
    public class BaseResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
