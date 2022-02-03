using Acr.UserDialogs;
using App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

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

        public async Task<ApiResponse<Dictionary<string, string>>> RegisterAsUser(User user)
        {
            try
            {
                var apiResponse = new ApiResponse<Dictionary<string, string>>();
                var location = GetGeoLocation();

                var response = await httpClient.PostAsync(
                    $"{url}/mm/validatemobilenumber?phoneNumber={user.phoneNumber}",
                    new StringContent(JsonConvert.SerializeObject(user)));

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.IsSuccessStatusCode = true;
                    apiResponse.Message = "Success";
                    //apiResponse.Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());
                    return apiResponse;
                }
            }
            catch
            {

            }

            return new ApiResponse<Dictionary<string, string>>
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

        public async Task<ApiResponse<Dictionary<string, string>>> SaveFirstTimeMaster(User user)
        {
            try
            {
                var apiResponse = new ApiResponse<Dictionary<string, string>>();

                var response = await httpClient.PostAsync(
                    $"{url}/mm/savefirsttimemaster?phoneNumber={user.phoneNumber}" +
                    $"&mailId={user.mailId}&gender={user.gender}" +
                    $"&pinNumber={user.pinNumber}&merchantType={user.merchantType}&deviceId={user.deviceId}",
                    new StringContent(string.Empty));

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.IsSuccessStatusCode = true;
                    apiResponse.Message = "Success";
                    //apiResponse.Data = JsonConvert.DeserializeObject<Dictionary<string, string>>((await response.Content.ReadAsStringAsync())); ;
                    return apiResponse;
                }
            }
            catch { }

            return new ApiResponse<Dictionary<string, string>>
            {
                IsSuccessStatusCode = false,
                Message = CommonErrorMessage
            };
        }

        public async Task<ApiResponse<Dictionary<string, string>>> Login(User user)
        {
            try
            {
                var apiResponse = new ApiResponse<Dictionary<string, string>>();

                var response = await httpClient.PostAsync(
                    $"{url}mm/fetchdetails?phoneNumber={user.phoneNumber}" +
                    $"&pinNumber={user.pinNumber}" +
                    $"&deviceId={user.deviceId}",
                    new StringContent(string.Empty));

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.IsSuccessStatusCode = true;
                    apiResponse.Message = "Success";
                    apiResponse.Data = JsonConvert.DeserializeObject<Dictionary<string, string>>((await response.Content.ReadAsStringAsync())); ;
                    return apiResponse;
                }
            }
            catch { }

            return new ApiResponse<Dictionary<string, string>>
            {
                IsSuccessStatusCode = false,
                Message = CommonErrorMessage
            };
        }

        //Xaminals

        async Task<(double,double)> GetGeoLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    return (location.Latitude, location.Longitude);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                UserDialogs.Instance.Toast(new ToastConfig("Please allow us to get location access.")
                {
                    MessageTextColor = System.Drawing.Color.Red,
                    Position = ToastPosition.Bottom
                });
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return (0.0, 0.0);
        }
    }
}
