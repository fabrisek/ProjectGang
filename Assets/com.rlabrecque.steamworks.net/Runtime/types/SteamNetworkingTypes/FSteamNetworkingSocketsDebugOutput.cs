// This file is provided under The MIT License as part of Steamworks.NET.
// Copyright (c) 2013-2022 Riley Labrecque
// Please see the included LICENSE.txt for additional information.

// This file is automatically generated.
// Changes to this file will be reverted when you update Steamworks.NET

#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
	#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS

using System.Runtime.InteropServices;
using IntPtr = System.IntPtr;

namespace Steamworks {
	/// Setup callback for debug output, and the desired verbosity you want.
	[System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
	public delegate void FSteamNetworkingSocketsDebugOutput(ESteamNetworkingSocketsDebugOutputType nType, System.Text.StringBuilder pszMsg);
}

#endif // !DISABLESTEAMWORKS
