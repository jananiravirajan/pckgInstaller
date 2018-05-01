using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using pckgInstaller;

namespace pckgInstallerTests
{
    public class PackageExtractorTest
    {
        [TestCase("C: ", "C")]
        [TestCase("K: ", "K")]
        public void GetPackageName(string packageInputLine, string expected)
        {
            var result = new PackageInputLineExtractor().GetPackageName(packageInputLine);
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("C: K, X", "K, X")]
        [TestCase("K: ", "")]
        public void GetPackageDependencyText(string packageInputLine, string expected)
        {
            var result = new PackageInputLineExtractor().GetPackageName(packageInputLine);
            Assert.That(result, Is.EqualTo(expected));
        }

    }
}
