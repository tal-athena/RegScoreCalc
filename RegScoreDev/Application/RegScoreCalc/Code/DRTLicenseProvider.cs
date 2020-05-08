using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Net.NetworkInformation;
using Helpers;

namespace RegScoreCalc
{
    public class DRTLicense : License
    {
        public override void Dispose()
        {
        }

        public override string LicenseKey
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
    }

    class DRTLicenseProvider : LicenseProvider
    {
        const string ekey = "+52sIR0fhnvPln2dd4aEKL+p8JrRghY04tPEqzaiif4=";
        const string akey = "bY6nSmdRlOmzU0ZElEo8KJgVx3sWTM5ftJWzn6GQnhs=";
        public override License GetLicense(
            LicenseContext context, Type type,
            object instance, bool allowExceptions)
        {
            if (DateTime.Today < new DateTime(2020, 5, 31))
            {
                return new DRTLicense();
            }
            // Do some logic to go figure out if this type or instance
            // is licensed. This can be implemented however you want.
            bool licenseIsValid = true;

            string lkeyenc = GetLicenseKey(type, instance);
            var key_bytes = LimitQueue.SimpleDecrypt(
                LimitQueue.GetBytes(lkeyenc),
                LimitQueue.GetBytes(ekey),
                LimitQueue.GetBytes(akey));
            string lkey = LimitQueue.GetString(key_bytes);
            licenseIsValid = IsLicenseValid(lkey);
            // If license check isn’t successful:
            if (!licenseIsValid)
            {
                throw new LicenseException(type, instance, "Invalid license.");
            }
            else
            {
                return new DRTLicense();
            }
        }

        bool IsLicenseValid(string lkey)
        {

            if (lkey.Length < 8)
                return false;

            var parts = lkey.Split('+');

            if (parts.Length < 2)
                return false;

            string dtPart = parts[1];

            // 2017 06 03
            string year = dtPart.Substring(0, 4);
            string month = dtPart.Substring(4, 2);
            string day = dtPart.Substring(6, 2);

            DateTime dt = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

            int nYear = int.Parse(year);
            if (dt.Date < DateTime.Now.Date || DateTime.Now.Date.Year > 2019)
            {
                return false;
            }

            if (parts.Length == 2)
                return true;

            // Check MAC address

            if (parts[2] != GetLocalMACAddress())
                return false;

            return true;
        }

        protected const string _strLicenseFileName = "license.dat";
        private string GetLicenseKey(Type type, object instance)
        {
            string strConfigPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _strLicenseFileName);
            if (!File.Exists(strConfigPath))
            {
                throw new LicenseException(type, instance, "Missing license file.");
            }
            try
            {
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(strConfigPath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                }
            }
            catch (Exception e)
            {
                throw new LicenseException(type, instance, e.Message);
            }
            return null;
        }

        private string GetLocalMACAddress()
        {
            var macAddr =
                (from nic in NetworkInterface.GetAllNetworkInterfaces()
                 where nic.OperationalStatus == OperationalStatus.Up
                 select nic.GetPhysicalAddress().ToString()
                    ).FirstOrDefault();

            return macAddr;
        }
    }
}