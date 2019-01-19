using System.Collections.Generic;
using System.Linq;
using DotNet.Globbing;
using TSQLLint.Core.Interfaces;
using TSQLLint.Infrastructure.Interfaces;

namespace TSQLLint.Infrastructure.Parser
{
    public class ExcludeGlobPathFilter : IFilePathFilter
    {
        private readonly List<Glob> excludeGlobs;

        public ExcludeGlobPathFilter(IConfigReader configReader)
        {
            this.excludeGlobs = configReader.GetExcludeFilePaths()
                .Select(x => Glob.Parse(x, new GlobOptions { Evaluation = new EvaluationOptions { CaseInsensitive = true }}))
                .ToList();
        }

        public bool IsFilePathAllowed(string filePath)
        {
            return !excludeGlobs.Any(x => x.IsMatch(filePath));
        }
    }
}
