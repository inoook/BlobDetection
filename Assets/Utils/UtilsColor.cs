using UnityEngine;
using System.Collections;

using System;
using System.Runtime.InteropServices;

public class Utils
{
	#region utils
	public static uint ColorToUint (Color32 c)
	{
		return (uint)(c.a << 24 | c.r << 16 | c.g << 8 | c.b);
	}
	
	public static Color32 UintToColor (uint uintColor)
	{
		uint a = (uintColor >> 24 & 0xFF);
		uint r = (uintColor >> 16 & 0xFF);
		uint g = (uintColor >> 8 & 0xFF);
		uint b = (uintColor & 0xFF);
		return new Color32 ((byte)r, (byte)g, (byte)b, (byte)a);
	}
	
	public static Color32[] ConvColorsInt (uint[] intColors)
	{
		Color32[] colors = new Color32[intColors.Length];
		for (int i = 0; i < intColors.Length; i++) {
			uint intColor = intColors [i];
			colors [i] = UintToColor (intColor);
		}
		return colors;
	}
	
	public static byte[] ConvColorsToBytes (uint[] intColors)
	{
		byte[] bytes = new byte[intColors.Length * 4];
		for (int i = 0; i < intColors.Length; i++) {
			uint uintColor = intColors [i];
			byte a = (byte)(uintColor >> 24 & 0xFF);
			byte r = (byte)(uintColor >> 16 & 0xFF);
			byte g = (byte)(uintColor >> 8 & 0xFF);
			byte b = (byte)(uintColor & 0xFF);
			
			bytes [i * 4 + 0] = r;
			bytes [i * 4 + 1] = g;
			bytes [i * 4 + 2] = b;
			bytes [i * 4 + 3] = a;
		}
		return bytes;
	}
	
	// http://stackoverflow.com/questions/21512259/fast-copy-of-color32-array-to-byte-array
	public static byte[] Color32ArrayToByteArray (Color32[] colors)
	{
		if (colors == null || colors.Length == 0)
			return null;
		
		int lengthOfColor32 = Marshal.SizeOf (typeof(Color32));
		int length = lengthOfColor32 * colors.Length;
		byte[] bytes = new byte[length];
		
		GCHandle handle = default(GCHandle);
		try {
			handle = GCHandle.Alloc (colors, GCHandleType.Pinned);
			IntPtr ptr = handle.AddrOfPinnedObject ();
			Marshal.Copy (ptr, bytes, 0, length);
		} finally {
			if (handle != default(GCHandle))
				handle.Free ();
		}
		
		return bytes;
	}
	#endregion
}