using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Infrastructure.Entities;
using Newtonsoft.Json;
using RestSharp;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface IGithubUpdateRepository
    {
        Version GetLatestReleaseVersion();
        GithubReleaseEntity GetLatestRelease();
    }

    public class GithubUpdateRepository : IGithubUpdateRepository
    {
        private const string _githubOwner = "christophergoltz";
        private const string _githubRepository = "imago-app";
        private readonly RestClient _client;
        public GithubUpdateRepository()
        {
            _client = new RestClient("https://api.github.com");
        }
        
        public Version GetLatestReleaseVersion()
        {
            var request = new RestRequest($"repos/{_githubOwner}/{_githubRepository}/releases/latest");

            var response = _client.Get(request);

            var t = JsonConvert.DeserializeObject<GithubReleaseEntity>(response.Content);
            return new Version(t.tag_name);
        }

        public GithubReleaseEntity GetLatestRelease()
        {
            var request = new RestRequest($"repos/{_githubOwner}/{_githubRepository}/releases/latest");

            var response = _client.Get(request);

            var t = JsonConvert.DeserializeObject<GithubReleaseEntity>(response.Content);
            return t;
        }
    }
}
