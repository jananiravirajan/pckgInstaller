using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pckgInstaller
{
    public class InstallationDependencyResolver
    {
        private const string DependencySeparator = ", ";
        private readonly HashSet<string> _unresolvedPackages;
        private readonly HashSet<string> _resolvedPackages;
        private readonly Dictionary<string, Package> _packages;
        private readonly InstallationDependencySanityChecker _installationDependencySanityChecker;

        public InstallationDependencyResolver()
        {
            _unresolvedPackages = new HashSet<string>();
            _resolvedPackages = new HashSet<string>();
            _packages = new Dictionary<string, Package>();
            _installationDependencySanityChecker = new InstallationDependencySanityChecker(_packages, _unresolvedPackages);
        }

        public string Resolver(IEnumerable<string> packageInputLines)
        {
            ClearCollections();
            GetPackagesFromPackageInputLines(packageInputLines);
            ResolvePackages();
            PerformSanityCheck();
            return FormatResolvedDependecyText();
        }


        private void ResolvePackages()
        {
            var currentDependencyDepth = 0;
            var maxDependencyDepth = GetMaxDependencyDepth();

            while (currentDependencyDepth <= maxDependencyDepth)
            {
                var packagesToResolve = GetPackagesMatchingDepth(currentDependencyDepth);

                foreach (var package in packagesToResolve)
                {
                    TryResolvePackage(package);
                }
                currentDependencyDepth++;
            }
        }

        private void TryResolvePackage(Package package)
        {
            if (package.CanResolveDependencies(_resolvedPackages))
            {
                _resolvedPackages.Add(package.Name);
                TryResolveCurrentUnresolvedPackages();
                return;
            }
            _unresolvedPackages.Add(package.Name);
        }

        private void GetPackagesFromPackageInputLines(IEnumerable<string> packageInputLines)
        {
            foreach (var packageInputLine in packageInputLines)
            {
                var package = Package.CreateFromPackageInputLine(packageInputLine);
                _packages.Add(package.Name, package);
            }
        }

        private void TryResolveCurrentUnresolvedPackages()
        {
            var copyOfUnresolvedPackages = CreateCopyOfUnresolvedPackages();

            foreach (var unresolvedPackage in copyOfUnresolvedPackages)
            {
                var package = _packages[unresolvedPackage];
                if (package.CanResolveDependencies(_resolvedPackages))
                {
                    _unresolvedPackages.Remove(unresolvedPackage);
                    _resolvedPackages.Add(unresolvedPackage);
                }
            }
        }

        private void ClearCollections()
        {
            _unresolvedPackages.Clear();
            _resolvedPackages.Clear();
            _packages.Clear();
        }

        private void PerformSanityCheck()
        {
            _installationDependencySanityChecker.PerformSanityCheck();
        }

        private IEnumerable<string> CreateCopyOfUnresolvedPackages()
        {
            return _unresolvedPackages.ToList();
        }

        private string FormatResolvedDependecyText()
        {
            return string.Join(DependencySeparator, _resolvedPackages);
        }

        private IEnumerable<Package> GetPackagesMatchingDepth(int depthToMatch)
        {
            return _packages.Values.Where(x => x.DependencyDepth == depthToMatch);
        }

        private int GetMaxDependencyDepth()
        {
            return 1; //For now the max depth is one, it could be deeper later
        }
    }
}
