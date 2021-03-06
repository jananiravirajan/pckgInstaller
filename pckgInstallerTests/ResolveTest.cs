﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using pckgInstaller;

namespace pckgInstallerTests
{
    public class ResolveTest
    {


        //case1
        [Test]
        public void NotInOrder()
        {
            var packageInputLines = new[]
            {
                "L: C",
                "C: I",
                "I: "
            };

            var result = new InstallationDependencyResolver().Resolver(packageInputLines);
            Assert.That(result, Is.EqualTo("I, C, L"));
        }

        [TestCase("C: ", "C")]
        [TestCase("K: ", "K")]
        public void NoDependencies(string packageInputLine, string expected)
        {
            var packageInputLines = new[] { packageInputLine };
            var result = new InstallationDependencyResolver().Resolver(packageInputLines);
            Assert.That(result, Is.EqualTo(expected));
        }


        [Test]
        public void InOrder()
        {
            var packageInputLines = new[] { "K: ", "C: " };


            var result = new InstallationDependencyResolver().Resolver(packageInputLines);
            Assert.That(result, Is.EqualTo("K, C"));
        }

        [Test]
        public void SingleDepdenency()
        {
            var packageInputLines = new[] { "K: C", "C: " };

            var result = new InstallationDependencyResolver().Resolver(packageInputLines);
            Assert.That(result, Is.EqualTo("C, K"));
        }



        [Test]
        public void CyclcicDep()
        {
            var packageInputLines = new[]
            {
                "KittenService: ",
                "Leetmeme: Cyberportal",
                "Cyberportal: Ice",
                "CamelCaser: KittenService",
                "Fraudstream: ",
                "Ice: Leetmeme"
            };

            //assert
            var installationDependencyResolver = new InstallationDependencyResolver();
            Assert.Throws<Exception>(() => installationDependencyResolver.Resolver(packageInputLines));
        }

        [Test]
        public void NotValidPackage()
        {
            var packageInputLines = new[]
            {
                "Leetmeme: Unknown"
            };


            var installationDependencyResolver = new InstallationDependencyResolver();
            Assert.Throws<Exception>(() => installationDependencyResolver.Resolver(packageInputLines));
        }

        [Test]
        public void Case1()
        {
            var packageInputLines = new[]
            {
                "KittenService: ",
                "Leetmeme: Cyberportal",
                "Cyberportal: Ice",
                "CamelCaser: KittenService",
                "Fraudstream: Leetmeme",
                "Ice: "
            };

            

            var installationDependencyResolver = new InstallationDependencyResolver();
            var result = installationDependencyResolver.Resolver(packageInputLines);
            Assert.That(result, Is.EqualTo("KittenService, Ice, Cyberportal, Leetmeme, CamelCaser, Fraudstream"));
        }

        [Test]
        public void Case2()
        {
            var packageInputLines = new[]
            {
                "A: F",
                "B: A",
                "C: B",
                "D: C",
                "E: D",
                "F: "
            };

            

            var installationDependencyResolver = new InstallationDependencyResolver();
            var result = installationDependencyResolver.Resolver(packageInputLines);
            Assert.That(result, Is.EqualTo("F, A, B, C, D, E"));
        }

        [Test]
        public void Case3()
        {
            var packageInputLines = new string[] { };
            var installationDependencyResolver = new InstallationDependencyResolver();
            var result = installationDependencyResolver.Resolver(packageInputLines);
            Assert.That(result, Is.EqualTo(""));

        }

        [Test]
        public void Case4()
        {
            var packageInputLines = new[]
            {
                "A: C",
                "B: D",
                "C: E",
                "D: ",
                "E: B",
                "F: D"
            };
            var installationDependencyResolver = new InstallationDependencyResolver();
            var result = installationDependencyResolver.Resolver(packageInputLines);
            Assert.That(result, Is.EqualTo("D, B, E, C, F, A"));
        }

    }
}
