using Newtonsoft.Json;
using TaskScheduler.Model;

namespace TaskScheduler.Service
{
    public class MovieService
    {
        private readonly HttpClient _httpClient;
        private string _apiKey = Environment.GetEnvironmentVariable("TMDb_API_KEY");

        public MovieService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        public async Task<ResponseModel<MovieApiResponse>> GetPopularMoviesAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.themoviedb.org/3/movie/popular?api_key={_apiKey}"),
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            try
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var movieApiResponse = JsonConvert.DeserializeObject<MovieApiResponse>(responseData);

                return new ResponseModel<MovieApiResponse>
                {
                    StatusCode = 200,
                    StatusMessage = "Popular movies found in the TMDB API..",
                    Data = movieApiResponse
                };

            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return new ResponseModel<MovieApiResponse>
                {
                    StatusCode = 500,
                    StatusMessage = "An error occurred while fetching popular movies.",
                    Data = null
                };
            }
        }
    }
}