using System;
using ExpressEncription;

namespace SecurityProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            //Generate Public and Pivate Key
            string publicKeyPath = @"D:\Project\SecurityProgram\SecurityProgram\Keypath\public.key";
            string privateKeyPath = @"D:\Project\SecurityProgram\SecurityProgram\Keypath\private.key";
            ExpressEncription.RSAEncription.MakeKey(publicKeyPath, privateKeyPath);

            //Encrypt
            var text = "Hello this is a security program with encryption";
            var encryptText = ExpressEncription.RSAEncription.EncryptString(text, publicKeyPath);

            //Decrypt
            var decryptText = ExpressEncription.RSAEncription.DecryptString(encryptText, privateKeyPath);
        }
    }
}
