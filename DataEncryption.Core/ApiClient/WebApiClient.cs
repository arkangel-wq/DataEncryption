﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DataEncryption.Core.ApiClient
{
    public class WebApiClient : HttpClient
    {
        public WebApiClient(string urlBase = null, string urlController = null) : base(CertValidator.CertHandler)
        {
            CertValidator.HandleCertValidation();

            var isUrlBaseNull = string.IsNullOrWhiteSpace(urlBase);
            if (isUrlBaseNull && string.IsNullOrWhiteSpace(urlController))
            {
                if (!string.IsNullOrWhiteSpace(UrlBaseWebApi))
                    BaseAddress = new Uri(UrlBaseWebApi);
            }
            else
            {
                if (!isUrlBaseNull)
                {
                    UrlBaseWebApi = urlBase;
                    BaseAddress = new Uri(UrlBaseWebApi);
                }
                UrlController = urlController;
            }
            InitHeaders();
            //Timeout = TimeSpan.FromMinutes(5);
        }

        public string UrlBaseWebApi { get; set; } = string.Empty;
        string urlController;
        protected string UrlController
        {
            get => urlController;
            set
            {
                urlController = value;
                UrlBaseWebApi += urlController;
                BaseAddress = new Uri(UrlBaseWebApi);
            }
        }

        protected virtual void InitHeaders()
        {
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //if (!string.IsNullOrEmpty(Helpers.Settings.Current.Token))
            //{
            //    DefaultRequestHeaders.Remove("Authorization");
            //    DefaultRequestHeaders.Add("Authorization", "Bearer " + Helpers.Settings.Current.Token);
            //}
        }

        //private (HttpStatusCode StatusCode, TResponse Content) ProcessResponse<TResponse>(HttpResponseMessage res)
        //{
        //    try
        //    {
        //        if (res.IsSuccessStatusCode)
        //        {
        //            var s = res.Content.ReadAsStringAsync().Result;
        //            var response = JsonConvert.DeserializeObject<TResponse>(s);
        //            return (res.StatusCode, response);
        //        }
        //        else
        //        {
        //            return (res.StatusCode, default(TResponse));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return (res.StatusCode, default(TResponse));
        //    }


        //}

        private async Task<(HttpStatusCode StatusCode, TResponse Content)> ProcessResponse<TResponse>(HttpRequestMessage requestMessage)
        {
            try
            {
                using var res = await SendAsync(requestMessage);
                if (res.IsSuccessStatusCode)
                {
                    var response = await res.Content.ReadFromJsonAsync<TResponse>();
                    return (res.StatusCode, response);
                }
                else
                {
                    return (res.StatusCode, default(TResponse));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return default;
            }
        }

        public async Task<(HttpStatusCode StatusCode, TResponse Content)> CallAsync<TResponse>(HttpMethod method, string url, HttpContent content = null)
        {
            using var requestMessage = new HttpRequestMessage(method, url);
            if (!requestMessage.RequestUri.IsAbsoluteUri)
                requestMessage.RequestUri = new Uri(UrlBaseWebApi + url);
            if (content != null) requestMessage.Content = content;
            return await ProcessResponse<TResponse>(requestMessage);
        }

        public async Task<(HttpStatusCode StatusCode, TResponse Content)> CallGetAsync<TResponse>(string url) =>
            await CallAsync<TResponse>(HttpMethod.Get, url);

        public async Task<(HttpStatusCode StatusCode, TResponse Content)> CallGetAsync<TResponse>(string url, params (string, object)[] parameters)
        {
            try
            {
                var sb = new StringBuilder();
                if (parameters.Length > 0)
                {
                    sb.Append("?");
                    foreach (var param in parameters)
                    {
                        sb.Append($"{param.Item1}={param.Item2}&");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
                return await CallAsync<TResponse>(HttpMethod.Get, $"{url}{sb}");
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<(HttpStatusCode StatusCode, TResponse Content)> CallPostAsync<TRequest, TResponse>(string url, TRequest req) =>
            await CallAsync<TResponse>(HttpMethod.Post, url, JsonContent.Create(req));

        //public async Task<(HttpStatusCode StatusCode, TResponse Content)> CallPostAsyncLog<TRequest, TResponse>(string url, TRequest req)
        //{
        //    try
        //    {
        //        var res = await PostAsync(url, new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json")).ConfigureAwait(false);
        //        return ProcessResponse<TResponse>(res);
        //    }
        //    catch (HttpRequestException)
        //    {
        //        return default;
        //        throw new ApplicationException("No se pudo conectar con el servicio");
        //    }
        //}

        public async Task<(HttpStatusCode StatusCode, TResponse Content)> CallPatchAsync<TRequest, TResponse>(string url, TRequest req) =>
            await CallAsync<TResponse>(HttpMethod.Patch, url, JsonContent.Create(req));

        public async Task<(HttpStatusCode StatusCode, TResponse Content)> CallPostAsync<TResponse>(string url, params (string, object)[] parameters)
        {
            try
            {
                var sb = new StringBuilder();
                if (parameters.Length > 0)
                {
                    sb.Append("?");
                    foreach (var param in parameters)
                    {
                        sb.Append($"{param.Item1}={param.Item2}&");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
                return await CallAsync<TResponse>(HttpMethod.Post, $"{url}{sb}");
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<(HttpStatusCode StatusCode, TResponse Content)> CallPutAsync<TRequest, TResponse>(string url, TRequest req) =>
            await CallAsync<TResponse>(HttpMethod.Put, url, JsonContent.Create(req));

        public async Task<(HttpStatusCode StatusCode, TResponse Content)> CallDeleteAsync<TResponse>(string url) =>
            await CallAsync<TResponse>(HttpMethod.Delete, url);

        private async Task<HttpStatusCode> ProcessResponse(HttpRequestMessage requestMessage)
        {
            try
            {
                using var res = await SendAsync(requestMessage);
                return res.StatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return default;
            }
        }

        public async Task<HttpStatusCode> CallAsync(HttpMethod method, string url, HttpContent content = null)
        {
            using var requestMessage = new HttpRequestMessage(method, url);
            if (!requestMessage.RequestUri.IsAbsoluteUri)
                requestMessage.RequestUri = new Uri(UrlBaseWebApi + url);
            if (content != null) requestMessage.Content = content;
            return await ProcessResponse(requestMessage);
        }

        public async Task<HttpStatusCode> CallPostAsync<TRequest>(string url, TRequest req) =>
            await CallAsync(HttpMethod.Post, url, JsonContent.Create(req));

        public async Task<HttpStatusCode> CallPostAsync(string url, params (string, object)[] parameters)
        {
            try
            {
                var sb = new StringBuilder();
                if (parameters.Length > 0)
                {
                    sb.Append("?");
                    foreach (var param in parameters)
                    {
                        sb.Append($"{param.Item1}={param.Item2}&");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
                return await CallAsync(HttpMethod.Post, $"{url}{sb}");
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<HttpStatusCode> CallDeleteAsync(string url) =>
           await CallAsync(HttpMethod.Delete, url);
        public async Task<HttpStatusCode> CallDeleteAsync(string url, params (string, object)[] parameters)
        {
            try
            {
                var sb = new StringBuilder();
                if (parameters.Length > 0)
                {
                    sb.Append("?");
                    foreach (var param in parameters)
                    {
                        sb.Append($"{param.Item1}={param.Item2}&");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
                return await CallAsync(HttpMethod.Delete, $"{url}{sb}");
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<(HttpStatusCode StatusCode, TResponse Content)> CallDeleteAsync<TResponse>(string url, params (string, object)[] parameters)
        {
            try
            {
                var sb = new StringBuilder();
                if (parameters.Length > 0)
                {
                    sb.Append("?");
                    foreach (var param in parameters)
                    {
                        sb.Append($"{param.Item1}={param.Item2}&");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }
                return await CallAsync<TResponse>(HttpMethod.Delete, $"{url}{sb}");
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<HttpStatusCode> CallPutAsync<TRequest>(string url, TRequest req) =>
            await CallAsync(HttpMethod.Put, url, JsonContent.Create(req));

        public async Task<(HttpStatusCode StatusCode, TResponse Content)> CallPostFileAsync<TResponse>(string url, byte[] file, string contentName, string fileName, string mediaType, HttpContent extraContent = null, string extraName = "")
        {
            //http://stackoverflow.com/questions/16416601/c-sharp-httpclient-4-5-multipart-form-data-upload
            using var requestContent = new MultipartFormDataContent();
            using var imageContent = new ByteArrayContent(file);
            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mediaType);
            requestContent.Add(imageContent, contentName, fileName);
            if (extraContent != null)
                requestContent.Add(extraContent, extraName);

            return await CallAsync<TResponse>(HttpMethod.Post, url, requestContent);
        }
    }

    /// <summary>
    /// Validate SSL Certificate for HttpClient calls
    /// </summary>
    static class CertValidator
    {
        internal static HttpClientHandler CertHandler { get; } = new HttpClientHandler();
        static bool setServerCertificateCallback;

        [DebuggerStepThrough]
        internal static void HandleCertValidation()
        {
            if (setServerCertificateCallback) return;
            setServerCertificateCallback = true;
            try
            {
                if (CertHandler.ServerCertificateCustomValidationCallback == null)
                {
                    CertHandler.ServerCertificateCustomValidationCallback = (m, cer, chain, e) => true;
                }
            }
#pragma warning disable CA1031 // Only because for Blazor don't implments ServerCertificateCustomValidationCallback
            catch { return; }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
