using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DuryTools
{
	/// <summary>
	/// Summary description for IO.
	/// </summary>
	public class IO
	{
		public IO()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		[DllImport("msvcrt.dll", SetLastError=true)]
		static extern int _mkdir(string path);
		//this function should provide safe substitude for Directory.CreateDirectory()
		public static DirectoryInfo CreateDirectory(string path)
		{
			int returnCode = _mkdir(path);
			if (returnCode!=0)
			{
				throw new ApplicationException("Error calling [msvcrt.dll]:_wmkdir(" + path + "), error code: " + returnCode.ToString());
			}
			return new DirectoryInfo(path);
		}
	}


}
