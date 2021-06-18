using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Imago.Util;
using Xunit;
using Xunit.Abstractions;

namespace Imago.Tests.Util
{
    public class WikiConstantsTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WikiConstantsTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CheckWikiPagesForContent()
        {
            var urlsToCheck = new List<string>()
            {
                WikiConstants.WikiMainPageUrl,
                WikiConstants.WikiUrlPrefix,
                WikiConstants.ArmorUrl,
                WikiConstants.MeleeWeaponUrl,
                WikiConstants.RangedWeaponUrl,
                WikiConstants.SpecialWeaponUrl,
                WikiConstants.ShieldsUrl,
                WikiConstants.ChangelogUrl
            };

            urlsToCheck.AddRange(WikiConstants.SkillGroupTypeLookUp.Values);
            urlsToCheck.AddRange(WikiConstants.SkillTypeLookUp.Values);
            urlsToCheck.AddRange(WikiConstants.ParsableSkillTypeLookUp.Values);

            foreach (var url in urlsToCheck)
            {
                try
                {
                    var request = WebRequest.Create(url) as HttpWebRequest;
                    var response = request?.GetResponse() as HttpWebResponse;

                    if (response == null)
                    {
                        _testOutputHelper.WriteLine("Respone null for " + url);
                        Assert.NotNull(response);
                    }
                    else
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            _testOutputHelper.WriteLine("Url response was not OK on: " + url);
                            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                        }
                    }
                }
                catch (Exception)
                {
                    Assert.True(false, "Request failed on : " + url);
                }
            }

        }
    }
}
