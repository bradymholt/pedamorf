using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Security.Permissions;
using System.IO;
using System.Diagnostics;
using Ionic.Zip;

namespace PedamorfServiceInstaller
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        private const string FIREWALL_RULE_NAME = "Pedamorf Service";
        private const string PACKED_CONTENT_LOCATION = "bin_external";
        public Installer()
        {
            InitializeComponent();
        }

        [SecurityPermission(SecurityAction.Demand)]
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            AddFirewallException(Context.Parameters["TARGETDIR"]);
            UnpackContent(Context.Parameters["TARGETDIR"]);
        }

        [SecurityPermission(SecurityAction.Demand)]
        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
            DeleteFirewallException(Context.Parameters["TARGETDIR"]);
            RemoveUnpackedContent(Context.Parameters["TARGETDIR"]);
        }

        [SecurityPermission(SecurityAction.Demand)]
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
            DeleteFirewallException(Context.Parameters["TARGETDIR"]);
            RemoveUnpackedContent(Context.Parameters["TARGETDIR"]);
        }

        [SecurityPermission(SecurityAction.Demand)]
        private static void AddFirewallException(string targetDirectory)
        {
            try
            {
                string targetDirectoryScrubbed = targetDirectory.Replace("\\\\", "\\");
                string[] executableFiles = Directory.GetFiles(targetDirectoryScrubbed, "*.exe");
                string primaryExecutable = new FileInfo(executableFiles[0]).Name;
                string executablePath = Path.Combine(targetDirectoryScrubbed, primaryExecutable);

                Process process = Process.Start("netsh",
                    string.Format("advfirewall firewall add rule name=\"{0}\" dir=in program=\"{1}\" action=allow",
                   FIREWALL_RULE_NAME,
                   executablePath));

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        [SecurityPermission(SecurityAction.Demand)]
        private static void DeleteFirewallException(string targetDirectory)
        {
            try
            {
                string targetDirectoryScrubbed = targetDirectory.Replace("\\\\", "\\");
                string[] executableFiles = Directory.GetFiles(targetDirectoryScrubbed, "*.exe");
                string primaryExecutable = new FileInfo(executableFiles[0]).Name;
                string executablePath = Path.Combine(targetDirectoryScrubbed, primaryExecutable);

                Process process = Process.Start("netsh",
                    string.Format("advfirewall firewall delete rule name=\"{0}\"",
                  FIREWALL_RULE_NAME));

                process.WaitForExit();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        private static void RemoveUnpackedContent(string targetDirectory)
        {
            try
            {
                string contentPackagesLocation = Path.Combine(targetDirectory, PACKED_CONTENT_LOCATION);
                if (Directory.Exists(contentPackagesLocation))
                {
                    Directory.Delete(contentPackagesLocation);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        private static void UnpackContent(string targetDirectory)
        {
            string contentPackagesLocation = Path.Combine(targetDirectory, PACKED_CONTENT_LOCATION);
            string[] contentPackages = Directory.GetFiles(contentPackagesLocation, "*.zip");

            try
            {
                foreach (string package in contentPackages)
                {
                    string contentPackageTargetPath = package.Replace(".zip", string.Empty);

                    if (!Directory.Exists(contentPackageTargetPath))
                    {
                        using (ZipFile zip = ZipFile.Read(package))
                        {
                            zip.ExtractAll(contentPackageTargetPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }
    }
}
