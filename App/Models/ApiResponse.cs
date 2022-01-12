using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    public class ApiResponse<T>
    {
        //True when status code is 200 Ok
        public bool IsSuccessStatusCode { get; set; }
        //Failed status code
        public int StatusCode { get; set; }
        //Response info "Successfully validated" ,"Successfully Added"
        //Response Error "Invalid Otp" , "Please provide mandate field"
        public string Message { get; set; }

        //Data object serialized in Json
        public T Data { get; set; }
        //Any exception occurs - "Internal Server Error etc".
        public string Exception { get; set; }
    }
}
