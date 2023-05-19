using DataEncryption.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DataEncryption.Core.ApiClient
{
    public class DataServicesEncryption : WebApiClient
    {
        public DataServicesEncryption() : base(Helpers.ApiHelper.Instance.UrlApisPublicas,"api/Values/")
        { }
        public async Task<(HttpStatusCode statusCode, string Mensaje)> GetEncriptar(string data) =>
        await CallPostAsync<string, string>("Encriptar", data);
        public async Task<(HttpStatusCode statusCode, string Mensaje)> GetDesencriptar(string data) =>
       await CallPostAsync<string, string>("Desencriptar", data);
    }
}
