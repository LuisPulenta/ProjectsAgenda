﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ProjectsAgenda.Common.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;

namespace ProjectsAgenda.Common.Services
{
    public class ApiService : IApiService
    {
        public async Task<Response<TokenResponse>> GetTokenAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            TokenRequest request)
        {
            try
            {
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<TokenResponse>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var token = JsonConvert.DeserializeObject<TokenResponse>(result);
                return new Response<TokenResponse>
                {
                    IsSuccess = true,
                    Result = token
                };
            }
            catch (Exception ex)
            {
                return new Response<TokenResponse>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response<PartnerResponse>> GetPartnerByEmailAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken,
            string email)
        {
            try
            {
                var request = new EmailRequest { Email = email };
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response<PartnerResponse>
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var owner = JsonConvert.DeserializeObject<PartnerResponse>(result);
                return new Response<PartnerResponse>
                {
                    IsSuccess = true,
                    Result = owner
                };
            }
            catch (Exception ex)
            {
                return new Response<PartnerResponse>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<bool> CheckConnectionAsync(string url)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return false;
            }

            return await CrossConnectivity.Current.IsRemoteReachable(url);
        }

    }
}
