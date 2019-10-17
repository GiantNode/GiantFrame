﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace Giant.Core
{
    public class RSAHelper
    {
        private static readonly int KeySize = 1024;
        private static readonly RSACryptoServiceProvider RSA;

        private static byte[] privateKey;
        private static byte[] publicKey;

        public static string PublicKey { get; private set; }
        public static string PrivateKey { get; private set; }

        static RSAHelper()
        {
            LoadKey();
            RSA = new RSACryptoServiceProvider();
            RSA.ImportCspBlob(privateKey);//导入私钥，私钥已经包含公钥信息
        }

        public static byte[] Encrypt(byte[] data)
        {
            return RSA.Encrypt(data, RSAEncryptionPadding.Pkcs1);
        }

        public static byte[] Decrypt(byte[] data)
        {
            return RSA.Decrypt(data, RSAEncryptionPadding.Pkcs1);
        }

        private static void LoadKey()
        {
            string path = Directory.GetCurrentDirectory();
            string publicKeyPath = Path.Combine(path, "PublicKey.txt");
            string privateKeyPath = Path.Combine(path, "PrivateKey.txt");

            if (!File.Exists(publicKeyPath) || !File.Exists(privateKeyPath))
            {
                RSACryptoServiceProvider SACryptoServiceProvider = new RSACryptoServiceProvider(KeySize);
                privateKey = SACryptoServiceProvider.ExportCspBlob(true);
                publicKey = SACryptoServiceProvider.ExportCspBlob(false);
                SACryptoServiceProvider.Dispose();

                PublicKey = Convert.ToBase64String(publicKey);
                PrivateKey = Convert.ToBase64String(privateKey);

                File.WriteAllText(publicKeyPath, PublicKey);
                File.WriteAllText(privateKeyPath, PrivateKey);
                return;
            }
            else
            {
                PublicKey = File.ReadAllText(publicKeyPath);
                PrivateKey = File.ReadAllText(privateKeyPath);

                publicKey = Convert.FromBase64String(PublicKey);
                privateKey = Convert.FromBase64String(PrivateKey);
            }
        }
    }
}
