using App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public class RegisterUser
    {
        HttpClient httpClient;
        string url = "http://122.165.101.67:8084/booking-merchant";
        const string CommonErrorMessage = "Unable to process your request , please try again later.";
        public RegisterUser()
        {
            httpClient = new HttpClient();
        }

        public async Task<ApiResponse<User>> RegisterAsUser(User user)
        {
            try
            {
                var apiResponse = new ApiResponse<User>();

                var response = await httpClient.PostAsync(
                    $"{url}/mm/validatemobilenumber?phoneNumber={user.phoneNumber}",
                    new StringContent(JsonConvert.SerializeObject(user)));

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.IsSuccessStatusCode = true;
                    apiResponse.Message = "Success";
                    apiResponse.Data = JsonConvert.DeserializeObject<User>((await response.Content.ReadAsStringAsync()));
                    return apiResponse;
                }
            }
            catch
            {

            }

            return new ApiResponse<User>
            {
                IsSuccessStatusCode = false,
                Message = CommonErrorMessage
            };
        }

        public async Task<ApiResponse<bool>> ValidateMobileNumber(string mobileNumber)
        {
            try
            {
                var apiResponse = new ApiResponse<bool>();

                var response = await httpClient.PostAsync(
                    $"{url}/mm/validatemobilenumber?phoneNumber={mobileNumber}",
                    new StringContent(string.Empty));

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.IsSuccessStatusCode = true;
                    apiResponse.Message = "Success";
                    apiResponse.Data = true;
                    return apiResponse;
                }
            }
            catch { }

            return new ApiResponse<bool>
            {
                IsSuccessStatusCode = false,
                Message = CommonErrorMessage
            };
        }

        public async Task<ApiResponse<bool>> OtpService()
        {
            try
            {
                var apiResponse = new ApiResponse<bool>();

                var response = await httpClient.GetAsync(
                    $"{url}/cc/otpservice");

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.IsSuccessStatusCode = true;
                    apiResponse.Message = "Success";
                    apiResponse.Data = true;
                    return apiResponse;
                }
            }
            catch { }

            return new ApiResponse<bool>
            {
                IsSuccessStatusCode = false,
                Message = CommonErrorMessage
            };
        }

        public async Task<ApiResponse<bool>> ValidOtpService(string mobileNumber, string otp)
        {
            var apiResponse = new ApiResponse<bool>();

            try
            {
                var response = await httpClient.PostAsync(
                        $"{url}/mm/matchingotp?phoneNumber={mobileNumber}&otp={otp}",
                        new StringContent(string.Empty));

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.IsSuccessStatusCode = true;
                    apiResponse.Message = "Success";
                    apiResponse.Data = true;
                    return apiResponse;
                }
            }
            catch { }

            return new ApiResponse<bool>
            {
                IsSuccessStatusCode = false,
                Message = CommonErrorMessage
            };
        }

        public async Task<ApiResponse<User>> SaveFirstTimeMaster(User user)
        {
            try
            {
                var apiResponse = new ApiResponse<User>();

                var response = await httpClient.PostAsync(
                    $"{url}/mm/savefirsttimemaster?phoneNumber={user.phoneNumber}" +
                    $"&mailId={user.mailId}&gender={user.gender}" +
                    $"&pinNumber={user.pinNumber}&merchantType={user.merchantType}&deviceId={user.deviceId}",
                    new StringContent(string.Empty));

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.IsSuccessStatusCode = true;
                    apiResponse.Message = "Success";
                    //apiResponse.Data = JsonConvert.DeserializeObject<User>((await response.Content.ReadAsStringAsync())); ;
                    return apiResponse;
                }
            }
            catch { }

            return new ApiResponse<User>
            {
                IsSuccessStatusCode = false,
                Message = CommonErrorMessage
            };
        }

        public async Task<ApiResponse<User>> Login(User user)
        {
            try
            {
                var apiResponse = new ApiResponse<User>();

                var response = await httpClient.PostAsync(
                    $"{url}mm/fetchdetails?phoneNumber={user.phoneNumber}" +
                    $"&pinNumber={user.pinNumber}",
                    new StringContent(string.Empty));

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.IsSuccessStatusCode = true;
                    apiResponse.Message = "Success";
                    //apiResponse.Data = JsonConvert.DeserializeObject<User>((await response.Content.ReadAsStringAsync())); ;
                    return apiResponse;
                }
            }
            catch { }

            return new ApiResponse<User>
            {
                IsSuccessStatusCode = false,
                Message = CommonErrorMessage
            };
        }
    }
}
