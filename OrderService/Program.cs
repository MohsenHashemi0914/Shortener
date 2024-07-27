using Polly;
using Polly.Retry;

var retryOptions = new RetryStrategyOptions
{
    MaxRetryAttempts = 5,
    Delay = TimeSpan.FromSeconds(5)
};

var pipline = new ResiliencePipelineBuilder().AddRetry(retryOptions).Build();

var action = () =>
{
    using var client = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5106")
    };

    var longUrl = "https://github.com/MohsenHashemi0914";
    Console.WriteLine($"Try get long \"{longUrl}\" url from shortener service ...");
    var data = client.GetStringAsync($"Shorten?long_url={longUrl}").GetAwaiter().GetResult();
    Console.WriteLine($"Short url is {data}");
};

pipline.Execute(action);
Console.ReadLine();