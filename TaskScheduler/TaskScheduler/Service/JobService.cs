using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Newtonsoft.Json;
using System.Linq.Expressions;
using TaskScheduler.Cache;
using TaskScheduler.Model;

namespace TaskScheduler.Service
{
    public class JobService
    {
        private readonly MovieService _movieService;
        private readonly ICacheService _cacheService;

        public JobService(MovieService movieService, ICacheService cacheService)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        public void ScheduleJob(string message, TimeSpan delay)
        {
            BackgroundJob.Schedule(() => Print(message, null), delay);
        }

        public string EnqueueJob(string message)
        {
            var jobID = BackgroundJob.Enqueue(() => Print(message, null));
            return jobID;
        }

        public void ContinueJobWith(string parentJobId, string message)
        {
            BackgroundJob.ContinueJobWith(parentJobId, () => Print(message, null));
        }

        public void AddOrUpdateRecurringJob(string jobId, string message, string cronExpression)
        {
            RecurringJob.AddOrUpdate(jobId, () => PrintRecurringJob(message, null), cronExpression);
        }

        public string Print(string message, PerformContext? context)
        {
            context?.WriteLine(message);
            return $"Printed: {message}";
        }

        public string PrintRecurringJob(string message, PerformContext? context)
        {
            context?.WriteLine(message);
            return $"Printed: {message}";
        }

        public void ScheduleJobMovie(Expression<Func<Task>> methodCall, TimeSpan delay)
        {
            BackgroundJob.Schedule(methodCall, delay);
        }

        public async Task<ResponseModel<List<MovieApiResponse>>> GetAndCachePopularMoviesAsync()
        {
            List<MovieApiResponse> moviesResult = null;

            try
            {
                var cacheResponse = await GetPopularMoviesFromCacheAsync();
                moviesResult = cacheResponse.Data;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Erro ao obter filmes populares do cache.");
            }

            if (moviesResult != null && moviesResult.Count > 0)
            {
                return new ResponseModel<List<MovieApiResponse>>
                {
                    StatusCode = 200,
                    StatusMessage = "Popular movies found in the cache.",
                    Data = moviesResult
                };
            }

            BackgroundJob.Enqueue(() => FetchAndCachePopularMovies());

            return new ResponseModel<List<MovieApiResponse>>
            {
                StatusCode = 202,
                StatusMessage = "Search for popular movies will be scheduled. Please wait a few moments.",
                Data = null
            };
        }

        public void ScheduleFetchAndCachePopularMovies()
        {
            BackgroundJob.Schedule(() => FetchAndCachePopularMovies(), TimeSpan.FromMinutes(5));
        }

        public async Task FetchAndCachePopularMovies()
        {
            var popularMovies = await _movieService.GetPopularMoviesAsync();

            if (popularMovies != null)
            {
                var serializedMovieData = JsonConvert.SerializeObject(popularMovies.Data);
                await _cacheService.SetAsync("PopularMovies", serializedMovieData, TimeSpan.FromMinutes(5));
            }
        }
        public async Task<ResponseModel<List<MovieApiResponse>>> GetPopularMoviesFromCacheAsync()
        {
            var moviesResult = await _cacheService.GetAsync("movies");

            if (moviesResult != null)
            {
                var movieApiResponse = JsonConvert.DeserializeObject<List<MovieApiResponse>>(moviesResult);
                return new ResponseModel<List<MovieApiResponse>>
                {
                    StatusCode = 200,
                    StatusMessage = "Popular movies found in the cache.",
                    Data = movieApiResponse
                };
            }

            return new ResponseModel<List<MovieApiResponse>>
            {
                StatusCode = 404,
                StatusMessage = "Popular movies not found in cache.",
                Data = null
            };
        }
    }
}