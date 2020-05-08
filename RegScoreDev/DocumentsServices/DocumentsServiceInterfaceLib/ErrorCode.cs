using System.ComponentModel;

namespace DocumentsServiceInterfaceLib
{
	public enum ErrorCode
	{
		[Description("requested by client")]
		ClientRequest = 0,

		[Description("main tool closed")]
		ParentClosed = 0,

		[Description("invalid pipe name")]
		InvalidPipeName,

		[Description("error limit reached")]
		ErrorLimitReached,

		[Description("see log for details")]
		SeeLogs
	}
}
