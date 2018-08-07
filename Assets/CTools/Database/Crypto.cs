using System.Security.Cryptography;
using System.Text;
using System;

public class Crypto{
	//内容加密
	public static string Encrypt(string toEncrypt){
		byte[] keyArray=UTF8Encoding.UTF8.GetBytes("15345678301834567490123456589012");//32位
		byte[] toEncryptArray=UTF8Encoding.UTF8.GetBytes(toEncrypt);
		RijndaelManaged rDel=new RijndaelManaged();
		rDel.Key=keyArray;
		rDel.Mode=CipherMode.ECB;
		rDel.Padding=PaddingMode.PKCS7;
		ICryptoTransform cTransform=rDel.CreateEncryptor();
		byte[] resultArray=cTransform.TransformFinalBlock(toEncryptArray,0,toEncryptArray.Length);
		return Convert.ToBase64String(resultArray,0,resultArray.Length);
	}

	//内容解密
	public static string Decrypt(string toDecrypt){
		byte[] keyArray=UTF8Encoding.UTF8.GetBytes("15345678301834567490123456589012");//同上
		byte[] toEncryptArray=Convert.FromBase64String(toDecrypt);
		RijndaelManaged rDel=new RijndaelManaged();
		rDel.Key=keyArray;
		rDel.Mode=CipherMode.ECB;
		rDel.Padding=PaddingMode.PKCS7;
		ICryptoTransform cTransform=rDel.CreateDecryptor();
		byte[] resultArray=cTransform.TransformFinalBlock(toEncryptArray,0,toEncryptArray.Length);
		return UTF8Encoding.UTF8.GetString(resultArray);
	}
}
