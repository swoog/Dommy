using Dommy.Business.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Dommy.Business.Test
{
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
                throw new KeyNotFoundException(string.Format("Uri {0} not found for mocking.", key));
            }

            return File.Open(this.dicoUriFiles[uri.AbsoluteUri], FileMode.Open);
        }
    }
}
