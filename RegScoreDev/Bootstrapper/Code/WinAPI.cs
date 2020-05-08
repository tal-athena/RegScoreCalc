using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Bootstrapper.Code
{
	public static class WinAPI
	{
		#region Operations

		public static bool IsDll64Bit(string dllPath)
		{
			using (var fs = new FileStream(dllPath, FileMode.Open, FileAccess.Read))
			{
				using (var br = new BinaryReader(fs))
				{
					fs.Seek(0x3c, SeekOrigin.Begin);

					var peOffset = br.ReadInt32();
					fs.Seek(peOffset, SeekOrigin.Begin);

					var peHead = br.ReadUInt32();

					if (peHead != 0x00004550) // "PE\0\0", little-endian
						throw new Exception("Cannot find PE header");

					var machineType = (MachineType)br.ReadUInt16();

					br.Close();
					fs.Close();

					if (machineType == MachineType.IMAGE_FILE_MACHINE_AMD64)
						return true;
				}
			}

			return false;
		}

		#endregion

		#region Interop

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsWow64Process(IntPtr hProcess, out bool wow64Process);

		#endregion
	}

	public enum MachineType : ushort
	{
		IMAGE_FILE_MACHINE_UNKNOWN = 0x0,
		IMAGE_FILE_MACHINE_AM33 = 0x1d3,
		IMAGE_FILE_MACHINE_AMD64 = 0x8664,
		IMAGE_FILE_MACHINE_ARM = 0x1c0,
		IMAGE_FILE_MACHINE_EBC = 0xebc,
		IMAGE_FILE_MACHINE_I386 = 0x14c,
		IMAGE_FILE_MACHINE_IA64 = 0x200,
		IMAGE_FILE_MACHINE_M32R = 0x9041,
		IMAGE_FILE_MACHINE_MIPS16 = 0x266,
		IMAGE_FILE_MACHINE_MIPSFPU = 0x366,
		IMAGE_FILE_MACHINE_MIPSFPU16 = 0x466,
		IMAGE_FILE_MACHINE_POWERPC = 0x1f0,
		IMAGE_FILE_MACHINE_POWERPCFP = 0x1f1,
		IMAGE_FILE_MACHINE_R4000 = 0x166,
		IMAGE_FILE_MACHINE_SH3 = 0x1a2,
		IMAGE_FILE_MACHINE_SH3DSP = 0x1a3,
		IMAGE_FILE_MACHINE_SH4 = 0x1a6,
		IMAGE_FILE_MACHINE_SH5 = 0x1a8,
		IMAGE_FILE_MACHINE_THUMB = 0x1c2,
		IMAGE_FILE_MACHINE_WCEMIPSV2 = 0x169,
	}
}
