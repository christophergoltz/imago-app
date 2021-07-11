using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Infrastructure.Repositories;

namespace ImagoApp.Application.Services
{
    public interface IGithubUpdateService
    {
        Version GetLatestRelease();
    }

    public class GithubUpdateService : IGithubUpdateService
    {
        private readonly IGithubUpdateRepository _githubUpdateRepository;

        public GithubUpdateService(IGithubUpdateRepository githubUpdateRepository)
        {
            _githubUpdateRepository = githubUpdateRepository;
        }

        public Version GetLatestRelease()
        {
            return _githubUpdateRepository.GetLatestReleaseVersion();
        }
    }
}
