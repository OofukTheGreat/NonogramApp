using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NonogramApp.Models;

namespace NonogramApp.Services
{
    public class NonogramService
    {
        #region without tunnel
        /*
        //Define the serevr IP address! (should be realIP address if you are using a device that is not running on the same machine as the server)
        private static string serverIP = "localhost";
        private HttpClient client;
        private string baseUrl;
        public static string BaseAddress = (DeviceInfo.Platform == DevicePlatform.Android &&
            DeviceInfo.DeviceType == DeviceType.Virtual) ? "http://10.0.2.2:5110/api/" : $"http://{serverIP}:5110/api/";
        private static string ImageBaseAddress = (DeviceInfo.Platform == DevicePlatform.Android &&
            DeviceInfo.DeviceType == DeviceType.Virtual) ? "http://10.0.2.2:5110" : $"http://{serverIP}:5110";
        */
        #endregion

        #region with tunnel
        //Define the serevr IP address! (should be realIP address if you are using a device that is not running on the same machine as the server)
        private static string serverIP = "rbswm66k-5068.euw.devtunnels.ms";
        private HttpClient client;
        private string baseUrl;
        public static string BaseAddress = "https://rbswm66k-5068.euw.devtunnels.ms/api/";
        public static string ImageBaseAddress = "https://rbswm66k-5068.euw.devtunnels.ms";
        #endregion

        public NonogramService()
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            this.client = new HttpClient(handler);
            this.baseUrl = BaseAddress;
        }

        public string GetImagesBaseAddress()
        {
            return NonogramService.ImageBaseAddress;
        }

        public string GetDefaultProfilePhotoUrl()
        {
            return $"{NonogramService.ImageBaseAddress}/profileImages/default.png";
        }
        public async Task<PlayerDTO?> LoginAsync(LoginInfo userInfo)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}login";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(userInfo);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    PlayerDTO? result = JsonSerializer.Deserialize<PlayerDTO>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<PlayerDTO?> Signup(PlayerDTO user)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}signUp";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    PlayerDTO? result = JsonSerializer.Deserialize<PlayerDTO>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string?> UploadProfileImage(string imagePath, int userId)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}uploadprofileimage?userId={userId}";
            try
            {
                //Create the form data
                MultipartFormDataContent form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                form.Add(fileContent, "file", imagePath);
                //Call the server API
                HttpResponseMessage response = await client.PostAsync(url, form);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    string? result = resContent;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Tile[,] TileListToArray(int size, ObservableCollection<Tile> tiles)
        {
            Tile[,] array = new Tile[size, size];
            foreach (Tile T in tiles)
            {
                array[T.Y, T.X] = new Tile()
                {
                    CurrentColor = T.CurrentColor,
                    X = T.X,
                    Y = T.Y,
                    IsMarked = T.IsMarked,
                };
            }
            return array;
        }
        public string TileArrayToLayout(Tile[,] board)
        {
            int size = board.GetLength(0);
            string layout = "";
            for (int i = 0; i < size; i++)
            {
                string color = "White";
                int count = 0;
                if (board[i, 0].CurrentColor == "Black")
                {
                    count++;
                    layout += "0,";
                    color = "Black";

                }
                else count++;
                for (int j = 1; j < size; j++)
                {
                    if (j == size - 1 && board[i, j].CurrentColor == color)
                    {
                        layout += count+1;
                        layout += ',';
                    }
                    else if (board[i, j].CurrentColor == color) count++;
                    else
                    {
                        if (board[i,j].CurrentColor == "White") color = "White";
                        else color = "Black";
                        layout += count;
                        layout += ',';
                        count = 1;
                    }
                }
                layout += '.';
            }
            return layout;
        }
        public async Task<ScoreDTO> AddScore(ScoreDTO score)
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}addScore";
            try
            {
                //Call the server API
                string json = JsonSerializer.Serialize(score);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    ScoreDTO? result = JsonSerializer.Deserialize<ScoreDTO>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<ScoreDTO>> GetScoresByList(int levelid)
        {
            //Set URI to the specific function API
            string parameterKey = "levelid";
            string parameterValue = levelid.ToString();
            string url = $"{this.baseUrl}getScoresByList?{parameterKey}={parameterValue}";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    List<ScoreDTO?> result = JsonSerializer.Deserialize<List<ScoreDTO?>>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<PlayerDTO>> GetPlayers()
        {
            //Set URI to the specific function API
            string url = $"{this.baseUrl}getPlayers";
            try
            {
                //Call the server API
                HttpResponseMessage response = await client.GetAsync(url);
                //Check status
                if (response.IsSuccessStatusCode)
                {
                    //Extract the content as string
                    string resContent = await response.Content.ReadAsStringAsync();
                    //Desrialize result
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    List<PlayerDTO?> result = JsonSerializer.Deserialize<List<PlayerDTO?>>(resContent, options);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
