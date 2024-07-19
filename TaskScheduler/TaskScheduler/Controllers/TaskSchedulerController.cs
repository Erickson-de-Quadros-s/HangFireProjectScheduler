using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TaskScheduler.Model;
using TaskScheduler.Service;

namespace TaskScheduler.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly JobService _jobService;
        private readonly MovieService _movieService;

        public JobsController(JobService jobService, MovieService movieService)
        {
            _jobService = jobService ?? throw new ArgumentNullException(nameof(jobService));
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
        }

        [HttpPost("schedule")]
        [SwaggerOperation(Summary = "Schedule a job")]
        public IActionResult ScheduleJob([FromBody] JobModel payload)
        {
            _jobService.ScheduleJob(payload.Message, TimeSpan.FromSeconds(payload.DelayInSeconds));
            return Ok("Job scheduled successfully");
        }

        [HttpPost("enqueue")]
        [SwaggerOperation(Summary = "Enqueue a job")]
        public IActionResult EnqueueJob([FromQuery][Required] string message)
        {
            var jobId = _jobService.EnqueueJob(message);
            return Ok($"Job enqueued successfully with ID: {jobId}");
        }

        [HttpPost("continue")]
        [SwaggerOperation(Summary = "Continue a job with a parent job ID")]
        public IActionResult ContinueJobWith([FromQuery][Required] string parentJobId, [Required][FromQuery] string message)
        {
            _jobService.ContinueJobWith(parentJobId, message);
            return Ok("Continuation job scheduled successfully");
        }

        [HttpPost("recurring")]
        [SwaggerOperation(Summary = "Add or update a recurring job")]
        public IActionResult AddOrUpdateRecurringJob([FromQuery][Required] string jobId, [FromQuery][Required] string message, [FromQuery][Required] string cronExpression)
        {
            _jobService.AddOrUpdateRecurringJob(jobId, message, cronExpression);
            return Ok("Recurring job added or updated successfully");
        }

        [HttpPost("schedule-job")]
        public IActionResult ScheduleJob()
        {
            _jobService.ScheduleJobMovie(() => _jobService.FetchAndCachePopularMovies(), TimeSpan.FromMinutes(5));
            return Ok("Task scheduled to run in 5 minutes.");
        }

        [HttpGet("popular-movies")]
        [SwaggerOperation(Summary = "Get popular movies from cache or schedule caching if not found")] // TO DO
        public async Task<ResponseModel<List<MovieApiResponse>>> GetPopularMovies()
        {
            return await _jobService.GetAndCachePopularMoviesAsync();
        }

        [HttpGet("schedule-cache")]
        [SwaggerOperation(Summary = "Fetches popular movies from TMDB API")]
        public async Task<ResponseModel<MovieApiResponse>> GenerateCach()
        {
            return await _movieService.GetPopularMoviesAsync();
        }

    }
}