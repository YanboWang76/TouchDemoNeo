using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureExtension  {
	public static Texture2D Tex2D(this RenderTexture srcTex)
	{
		Texture2D dstTex2D = new Texture2D (srcTex.width, srcTex.height, TextureFormat.ARGB32, false);
		RenderTexture.active = srcTex;
		dstTex2D.ReadPixels (new Rect (0, 0, srcTex.width, srcTex.height), 0, 0);
		dstTex2D.Apply ();
		return dstTex2D;
	}
	public static Texture2D Tex2D(this byte[] srcData,int width,int height)
	{
		Texture2D dstTex2D = new Texture2D (width,height);
		dstTex2D.LoadImage (srcData);
		return dstTex2D;
	}
}
