using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pckgInstaller
{
    public class InstallationDependencySanityChecker
    {
        private readonly Dictionary<string, Package> _packages;
        private readonly HashSet<string> _unresolvedPackages;

        public InstallationDependencySanityChecker(
            Dictionary<string, Package> packages,
            HashSet<string> unresolvedPackages)
        {
            _packages = packages;
            _unresolvedPackages = unresolvedPackages;
        }

        public void PerformSanityCheck()
        {
            EnsureNoUnrecognizedDependencies();
            EnsureNoCyclicDependencies();       
        }

        private void EnsureNoCyclicDependencies()
        {
            if (_unresolvedPackages.Any()) throw new Exception(_unresolvedPackages.First());
        }

        private void EnsureNoUnrecognizedDependencies()
        {
            foreach (var unresolvedPackage in _unresolvedPackages)
            {
                var package = _packages[unresolvedPackage];
                foreach (var dependency in package.Dependencies)
                {
                    EnsurePackageHasRecognizedDependencies(dependency);
                }
            }
        }

        private void EnsurePackageHasRecognizedDependencies(string dependency)
        {
            if (!_packages.ContainsKey(dependency))
            {
                throw new Exception(dependency);
            }
        }

    }
}
