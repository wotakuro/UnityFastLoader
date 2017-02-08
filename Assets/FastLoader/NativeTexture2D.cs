using UnityEngine;
using System;

namespace FastLoader
{
	public class NativeTexture2D : System.IDisposable
	{

		private Texture2D texture = null;
		private IntPtr nativePtr = IntPtr.Zero;

		public NativeTexture2D( Texture2D t , IntPtr p ){
			this.texture = t;
			this.nativePtr = p;
		}

		public static implicit operator Texture2D (NativeTexture2D t)
		{
			return t.texture;
		}

		public void Dispose ()
		{
			if (nativePtr != IntPtr.Zero) {
				TextureLoader.ReleaseTexture (nativePtr);
			}
			nativePtr = IntPtr.Zero;
		}
		~NativeTexture2D(){
			Dispose ();
		}

		protected virtual void Dispose(bool disposing)
		{
			Dispose ();
		}
	}
}