using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using Serilog;

namespace Triton.Core
{
    public static class RestApiHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly CancellationToken _cancellationToken = default;

        /// <summary>
        /// GetAsycn Helper method to get from a WebApi
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns>T</returns>
        public static async Task<T> GetAsync<T>(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri, _cancellationToken).ConfigureAwait(false);
            return await GetResult<T>(response);
        }

        /// <summary>
        /// Method to Post Async 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="postObject"></param>
        /// <returns></returns>
        public static async Task<T> PostAsync<T>(Uri uri, T postObject)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(postObject), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, httpContent, _cancellationToken).ConfigureAwait(false);

            return await GetResult<T>(response);
        }

        /// <summary>
        /// Method to post and return a long
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="postObject"></param>
        /// <returns></returns>
        public static async Task<long> PostAsyncLong<T>(Uri uri, T postObject)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(postObject), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, httpContent, _cancellationToken).ConfigureAwait(false);

            return await GetResult<long>(response);
        }

        /// <summary>
        /// Method to post and return a string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="postObject"></param>
        /// <returns></returns>
        public static async Task<string> PostAsyncString<T>(Uri uri, T postObject)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(postObject), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, httpContent, _cancellationToken).ConfigureAwait(false);

            return await GetResult<string>(response);
        }

        public static async Task<bool> PostAsyncBool<T>(Uri uri, T postObject)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(postObject), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, httpContent, _cancellationToken).ConfigureAwait(false);

            return await GetResult<bool>(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="putObject"></param>
        /// <returns></returns>
        public static async Task<bool> PutAsync<T>(Uri uri, T putObject)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(putObject), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(uri, httpContent, _cancellationToken).ConfigureAwait(false);

            return await GetResult<bool>(response);
        }

        /// <summary>
        /// Delete a record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteAsync(Uri uri)
        {
            var response = await _httpClient.DeleteAsync(uri, _cancellationToken).ConfigureAwait(false);
            return await GetResult<bool>(response);
        }

        /// <summary>
        /// Helper for common Async calls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns>T</returns>
        private static async Task<T> GetResult<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    throw new HttpRequestException($"{response.StatusCode}:{response.Content}");

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(responseStream, cancellationToken: _cancellationToken);
            }

            //var message = ErrorHelper(response, uri, "");
            throw new HttpRequestException($"{response.StatusCode}");
        }

        /// <summary>
        /// Method to log error to a file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="uri"></param>
        /// <param name="objectValue"></param>
        /// <returns></returns>
        private static async Task<HttpResponseMessage> ErrorHelper<T>(HttpResponseMessage response, Uri uri, T objectValue)
        {
            var startupPath = $"{System.IO.Directory.GetCurrentDirectory()}\\logs\\log.txt";

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(startupPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var content = await response.Content.ReadAsStringAsync();
            response.Content?.Dispose();
            Log.Error("Content: {@content}, apiUrl:  {@uri}, Object Value:  {@objectValue}", content, uri, objectValue);

            return response;
        }
    }
}
