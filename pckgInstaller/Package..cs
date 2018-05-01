using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pckgInstaller
{
    public class Package
    {
        private const string DependecyItemSeparator = ",";

        public string Name { get; private set; }
        public int DependencyDepth { get { return Dependencies.Count(); } }
        public IEnumerable<string> Dependencies { get; private set; }

        private Package(string name, string dependencyText)
        {
            Name = name;
            Dependencies = GetDependencyItems(dependencyText);
        }

        private IEnumerable<string> GetDependencyItems(string dependencyText)
        {
            var uncleanedDependencies = dependencyText.Split(new[] { DependecyItemSeparator }, StringSplitOptions.RemoveEmptyEntries);
            return uncleanedDependencies.Select(x => x.Trim());
        }

        public static Package CreateFromPackageInputLine(string packageInputLine)
        {
            var packageInputLineExtractor = new PackageInputLineExtractor();
            var name = packageInputLineExtractor.GetPackageName(packageInputLine);
            var dependencyText = packageInputLineExtractor.GetPackageDependencyText(packageInputLine);
            return new Package(name, dependencyText);
        }

        public bool CanResolveDependencies(IEnumerable<string> resolvedPackages)
        {
            return Dependencies.All(resolvedPackages.Contains);
        }

    }
}
