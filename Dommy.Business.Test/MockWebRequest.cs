
namespace Dommy.Business.Test
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Dommy.Business.Tools;

    public class MockWebRequest : IWebRequest
    {
        private Dictionary<string, string> dicoUriFiles;

        public MockWebRequest(Dictionary<string, string> dicoUriFiles)
        {
            this.dicoUriFiles = dicoUriFiles;
        }

        public System.IO.Stream Create(Uri uri)
        {
            var key = uri.AbsoluteUri;

            if (!this.dicoUriFiles.ContainsKey(key))
            {
                throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "Uri {0} not found for mocking.", key));
            }

            return File.Open(this.dicoUriFiles[uri.AbsoluteUri], FileMode.Open);
        }
    }
}
