using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business
{
    /// <summary>
    /// Accepted sentence.
    /// </summary>
    public class SentenceAction
    {
        /// <summary>
        /// Gets or sets unique key.
        /// </summary>
        public string UniqueKey { get; set; }

        /// <summary>
        /// Gets or sets accepted sentences.
        /// </summary>
        public IList<string> Sentences { get; set; }

        /// <summary>
        /// Gets or sets action to execute.
        /// </summary>
        public Action<ISentence> Action { get; set; }
    }
}
