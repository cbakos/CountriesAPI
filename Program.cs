﻿using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;

namespace CountriesAPI
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private const string baseURI = "https://restcountries-v1.p.rapidapi.com/";
        private const string headerKeyString = "x-rapidapi-key";
        private const string headerKey = "87df6da5f0mshdc4ff4d39ed1d44p1696f0jsn20e0c00600ed";
        
        static async Task Main(string[] args)
        {
            int dialNumber;
            Console.Write("Enter calling code to get information about countries that use it: ");
            dialNumber = Convert.ToInt32(Console.ReadLine());
            var countries = await GetCountriesByCallingCode(dialNumber);
            if(countries is null)
            {
                Console.WriteLine("No countries found with this calling code.");
            } 
            else 
            {
                foreach(Country c in countries)
                {
                    Console.WriteLine(c.ToString());
                } 
            }
            
            
        }

        private static async Task<List<Country>> GetCountriesByCallingCode(int callingCode)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(baseURI+"/callingcode/"+callingCode.ToString())
            };
            request.Headers.Add(headerKeyString, headerKey);
            var response = await client.SendAsync(request);
            try 
            {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException)
            {
                return null;
            }
            
            var body = await response.Content.ReadAsStreamAsync();
            var countries = await JsonSerializer.DeserializeAsync<List<Country>>(body);

            return countries;
                
        }
    }
}
