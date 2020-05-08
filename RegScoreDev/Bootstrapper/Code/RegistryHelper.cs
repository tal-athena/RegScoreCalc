using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace Bootstrapper
{
	public static class RegistryHelper
	{
		#region Operations

		/// <summary>
		///     Open a registry key using the Wow64 node instead of the default 32-bit node.
		/// </summary>
		/// <param name="parentKey">Parent key to the key to be opened.</param>
		/// <param name="subKeyName">Name of the key to be opened</param>
		/// <param name="writable">Whether or not this key is writable</param>
		/// <param name="options">32-bit node or 64-bit node</param>
		/// <returns></returns>
		public static RegistryKey OpenSubKey(RegistryKey parentKey, string subKeyName, bool writable, RegWow64Options options)
		{
			// Sanity check
			if (parentKey == null || _getRegistryKeyHandle(parentKey) == IntPtr.Zero)
				return null;

			// Set rights
			var rights = (int) RegistryRights.ReadKey;
			if (writable)
				rights = (int) RegistryRights.WriteKey;

			//Call the native function >.<
			int subKeyHandle, result = RegOpenKeyEx(_getRegistryKeyHandle(parentKey), subKeyName, 0, rights | (int) options, out subKeyHandle);

			// If we errored, return null
			if (result != 0)
				return null;

			// Get the key represented by the pointer returned by RegOpenKeyEx
			var subKey = _pointerToRegistryKey((IntPtr) subKeyHandle, writable, false);

			return subKey;
		}

		#endregion

		#region Implementation

		/// <summary>
		///     Get a pointer to a registry key.
		/// </summary>
		/// <param name="registryKey">Registry key to obtain the pointer of.</param>
		/// <returns>Pointer to the given registry key.</returns>
		private static IntPtr _getRegistryKeyHandle(RegistryKey registryKey)
		{
			// Get the type of the RegistryKey
			var registryKeyType = typeof (RegistryKey);
			
			// Get the FieldInfo of the 'hkey' member of RegistryKey
			var fieldInfo = registryKeyType.GetField("hkey", BindingFlags.NonPublic | BindingFlags.Instance);

			// Get the handle held by hkey
			var handle = (SafeHandle) fieldInfo.GetValue(registryKey);
			
			// Get the unsafe handle
			var dangerousHandle = handle.DangerousGetHandle();
			
			return dangerousHandle;
		}

		/// <summary>
		///     Get a registry key from a pointer.
		/// </summary>
		/// <param name="hKey">Pointer to the registry key</param>
		/// <param name="writable">Whether or not the key is writable.</param>
		/// <param name="ownsHandle">Whether or not we own the handle.</param>
		/// <returns>Registry key pointed to by the given pointer.</returns>
		private static RegistryKey _pointerToRegistryKey(IntPtr hKey, bool writable, bool ownsHandle)
		{
			// Get the BindingFlags for private contructors
			var privateConstructors = BindingFlags.Instance | BindingFlags.NonPublic;
			
			// Get the Type for the SafeRegistryHandle
			var safeRegistryHandleType = typeof (SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle");
			
			// Get the array of types matching the args of the ctor we want
			Type[] safeRegistryHandleCtorTypes = { typeof (IntPtr), typeof (bool) };
			
			// Get the constructorinfo for our object
			var safeRegistryHandleCtorInfo = safeRegistryHandleType.GetConstructor(
				privateConstructors, null, safeRegistryHandleCtorTypes, null);
			
			// Invoke the constructor, getting us a SafeRegistryHandle
			var safeHandle = safeRegistryHandleCtorInfo.Invoke(new Object[] { hKey, ownsHandle });

			// Get the type of a RegistryKey
			var registryKeyType = typeof (RegistryKey);
			
			// Get the array of types matching the args of the ctor we want
			Type[] registryKeyConstructorTypes = { safeRegistryHandleType, typeof (bool) };
			
			// Get the constructorinfo for our object
			var registryKeyCtorInfo = registryKeyType.GetConstructor(privateConstructors, null, registryKeyConstructorTypes, null);
			
			// Invoke the constructor, getting us a RegistryKey
			var resultKey = (RegistryKey) registryKeyCtorInfo.Invoke(new[] { safeHandle, writable });
			
			// return the resulting key
			return resultKey;
		}

		#endregion

		#region Interop

		[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
		private static extern int RegOpenKeyEx(IntPtr hKey, string subKey, int ulOptions, int samDesired, out int phkResult);

		#endregion

		#region Types

		public enum RegWow64Options
		{
			#region Constants

			None = 0,
			KEY_WOW64_64KEY = 0x0100,
			KEY_WOW64_32KEY = 0x0200

			#endregion
		}

		private enum RegistryRights
		{
			#region Constants

			ReadKey = 131097,
			WriteKey = 131078

			#endregion
		}

		#endregion
	}
}