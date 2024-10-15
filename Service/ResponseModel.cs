namespace GymMembershipAPI.Service
{
    public class ResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public int? ResultCode { get; set; }

        //Method for success 
        public ResponseModel<T>SuccessResult(T result)
        {
            var c = new ResponseModel<T>
            {
                Message = "Operation Successfull",
                IsSuccess = true,
                Result = result,
                ResultCode = 200
            };
            return c;
        }
        //Method for failed
        public ResponseModel<T>FailedResult(T result)
        {
            var c = new ResponseModel<T>
            {
                Message = "Operation failed",
                IsSuccess = false,
                Result = result,
                ResultCode = 400
            };
            return c;
        }
    }
}
