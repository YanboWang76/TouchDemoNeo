using System.Text;
using System;
using System.Security.Cryptography;

public class MD5Tool{

	public static string GetMD5_32(string inputStr){
		MD5 md5=new MD5CryptoServiceProvider();
		byte[] inputBytes=Encoding.UTF8.GetBytes(inputStr);
		byte[] hash=md5.ComputeHash(inputBytes);
		StringBuilder sb=new StringBuilder();
		for(int i = 0;i < hash.Length;i++){
			sb.Append(hash[i].ToString("X2"));
		}
		return sb.ToString();
	}

}
