﻿using Newtonsoft.Json;
using Surveys.Entities;
using Surveys.Models;
using Surveys.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Services
{
    public class WebApiService : IWebApiService
    {
        private readonly HttpClient client;

        public WebApiService()
        {
            client = new HttpClient { BaseAddress = new Uri(Literals.WebApiServiceBaseAddress) };
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            IEnumerable<Team> result = null;
            var teams = await client.GetStringAsync("api/teams");

            if (!string.IsNullOrWhiteSpace(teams))
            {
                result = JsonConvert.DeserializeObject<IEnumerable<Team>>(teams);

                return result;
            }

            return result;
        }

        public async Task<bool> SaveSurveysAsync(IEnumerable<Survey> surveys)
        {
            var content = new StringContent(JsonConvert.SerializeObject(surveys),
                System.Text.Encoding.UTF8, "application/json");

            var respone = await client.PostAsync("api/surveys", content);

            return respone.IsSuccessStatusCode;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var encodedUsername = WebUtility.UrlEncode(username);
            var encodedPassword = WebUtility.UrlEncode(password);
            var content = new StringContent(
                $"grant_type=password&username={encodedUsername}&password={encodedPassword}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
                );
            var uri = new Uri($"{Literals.WebApiServiceBaseAddress}token");

            using (var response = client.PostAsync(uri.ToString(), content).Result)
            {
                var value = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var token = JsonConvert.DeserializeObject<TokenResponseModel>(value);
                    var tokenString = token.AccessToken;

                    if (!client.DefaultRequestHeaders.Contains("Authorization"))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenString);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
