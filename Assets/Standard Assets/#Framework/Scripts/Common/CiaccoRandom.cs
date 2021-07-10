using GameEngine.Extensions;
using GameEngine.Kernel;

//using IonKiwi.lz4;
using LZ4;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Puerts.Attributes;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Unity.Linq;
using UnityEditor;
using UnityEngine;

namespace Common {

/// <summary>
/// https://stackoverflow.com/questions/33462319/how-do-i-replicate-phps-prng-algorithm-implementation-in-c
/// </summary>
public class CiaccoRandom {

    int tree = 0;
    static long serverStartTime = -1;

    //static string base56 = "kC9UGXhEtN72aQcPRKdAjyW5VqBvsiueTxpHrgbYLDzSZ4w683mFMJfn";

    // System.Text.Encoding.UTF8.GetString(bytes).ToCharArray();
    static byte[] base56 = {
        0x6b, 0x43, 0x39, 0x55, 0x47, 0x58, 0x68, 0x45, 0x74, 0x4e, 0x37, 0x32, 0x61, 0x51, 0x63,
        0x50,
        0x52, 0x4b, 0x64, 0x41,
        0x6a, 0x79, 0x57, 0x35, 0x56, 0x71, 0x42, 0x76, 0x73, 0x69, 0x75, 0x65, 0x54, 0x78, 0x70,
        0x48,
        0x72, 0x67, 0x62, 0x59, 0x4c, 0x44, 0x7a, 0x53, 0x5a, 0x34, 0x77, 0x36, 0x38, 0x33, 0x6d,
        0x46,
        0x4d, 0x4a, 0x66,
        0x6e,
    };

    static byte[] pubkey = {
        0x30, 0x81, 0x9f, 0x30, 0x0d, 0x06, 0x09, 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01,
        0x01,
        0x05, 0x00, 0x03, 0x81, 0x8d, 0x00, 0x30, 0x81, 0x89, 0x02, 0x81, 0x81, 0x00, 0xc0, 0x12,
        0x6a,
        0x54, 0x3b, 0x23, 0x8a, 0xc9, 0x29, 0x97, 0x57, 0x3c, 0xd3, 0x1f, 0x65, 0x57, 0x1c, 0xfa,
        0xc3,
        0x05, 0xba, 0xa5, 0x2c, 0xc5, 0x8a, 0xf5, 0x16, 0x9e, 0xbe, 0x8f,
        0x96, 0x88, 0xfb, 0x12, 0xb8, 0x80, 0x9e, 0xd3, 0xd9, 0x82, 0x80, 0xa3, 0x59, 0xe8, 0xcf,
        0xfe,
        0xa5, 0x5d, 0x89, 0x2e, 0xbc, 0x96, 0xc0, 0xf0, 0x6b, 0xcb, 0x65, 0x43, 0x09, 0xdb, 0x86,
        0xdc,
        0xd2, 0xf1, 0x9b, 0xe2, 0x18, 0x09, 0xa2, 0xd5, 0x6f, 0x2b, 0x04, 0xc6, 0x50, 0x28, 0x5a,
        0x7b,
        0xcb, 0xb6, 0xe7, 0x57, 0xe0, 0x62, 0xba, 0x99, 0x53, 0x7f, 0xd2,
        0x74, 0x5e, 0xe8, 0xf2, 0xab, 0x08, 0x37, 0x68, 0x70, 0x3b, 0xa5, 0x4d, 0x89, 0x06, 0x7f,
        0x6c,
        0xba, 0xa9, 0x9c, 0x10, 0xd4, 0xed, 0x70, 0x41, 0x45, 0xe9, 0x44, 0xff, 0x20, 0xfe, 0x01,
        0x69,
        0xbf, 0xc3, 0x2f, 0x74, 0x00, 0x86, 0x0f, 0x02, 0x03, 0x01, 0x00, 0x01,
    };

    public CiaccoRandom(int seed = -1)
    {
        plantSeed(seed > 0 ? (int)(seed % int.MaxValue) : (int)(UnixTimestamp() % int.MaxValue));
    }

    public static long SetServerStartTime(long serverTimestamp) =>
        serverStartTime = serverTimestamp - (int)Time.realtimeSinceStartup;

    public static long UnixTimestamp(bool useServerTime = true) => !useServerTime || serverStartTime < 0
        ? DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        : serverStartTime + (int)Time.realtimeSinceStartup;

    public CiaccoRandom() : this(-1) { }

    public CiaccoRandom(long seed = -1)
    {
        plantSeed(seed > 0 ? (int)(seed % int.MaxValue) : (int)(UnixTimestamp() % int.MaxValue));
    }

    public void plantSeed(int seed, int next = 0)
    {
        tree = Math.Abs(seed) % 9999999 + 1;
        getRand(next);
    }

    public void plantSeed(long seed = -1) => plantSeed((int)((seed > 0 ? seed : UnixTimestamp()) % int.MaxValue));

    public int getRand(int min = 0, int max = 9999999)
    {
        tree = tree * 125 % 2796203;

        return tree % (max - min + 1) + min;
    }

#region RC4加密

    /// <summary>
    /// RC4加密
    /// </summary>
    /// <param name="source">需要加密的字符串</param>
    /// <param name="keyStr">用于加密的密钥</param>
    /// <returns>返回加密后的字符串</returns>
    public static string RC4ENC(string keyStr, string source)
    {
        try {
            var contents = Encoding.UTF8.GetBytes(source);

            return Convert.ToBase64String(RC4(keyStr, contents)); //转为base64编码
        } catch {
            return "";
        }
    }

#endregion

#region RC4解密

    /// <summary>
    /// RC4解密
    /// </summary>
    /// <param name="source">需要加密的字符串</param>
    /// <param name="keyStr">用于加密的密钥</param>
    /// <returns>返回解密后的字符串</returns>
    public static string RC4DEC(string keyStr, string source)
    {
        try {
            var data = Convert.FromBase64String(source); //base64编码还原
            var str = RC4(keyStr, data);

            return Encoding.UTF8.GetString(str);
        } catch {
            return "";
        }
    }

#endregion

#region RC4加密解密

    /// <summary>
    /// RC4加密解密
    /// </summary>
    /// <param name="source">需要加密的字符串</param>
    /// <param name="keyStr">用于加密的密钥</param>
    /// <param name="contents"></param>
    /// <returns>加密后的字节集</returns>
    public static byte[] RC4(string keyStr, byte[] contents)
    {
        var results = new byte[contents.Length];
        var pwds = Encoding.UTF8.GetBytes(keyStr);
        var key = new byte[256];
        var box = new byte[256];

        for (var i = 0; i < 256; i++) {
            key[i] = pwds[i % pwds.Length];
            box[i] = (byte)i;
        }

        for (int j = 0, i = 0; i < 256; i++) {
            j = (j + box[i] + key[i]) % 256;
            var tmp = box[i];
            box[i] = box[j];
            box[j] = tmp;
        }

        for (int a = 0, j = 0, i = 0; i < contents.Length; i++) {
            a = (a + 1) % 256;
            j = (j + box[a]) % 256;
            var tmp = box[a];
            box[a] = box[j];
            box[j] = tmp;
            var k = box[(box[a] + box[j]) % 256];
            results[i] = (byte)(contents[i] ^ k);
        }

        return results;
    }

#endregion

    /// <summary>
    /// Import OpenSSH PEM public key string into MS RSACryptoServiceProvider
    /// </summary>
    /// <param name="pem"></param>
    /// <returns></returns>
    public static RSACryptoServiceProvider ImportPublicKey()
    {
        //var pem = System.Text.Encoding.Default.GetString(pubkey);
        //PemReader pr = new PemReader(new StringReader(pem));
        var publicKey = PublicKeyFactory.CreateKey(pubkey);

        //(AsymmetricKeyParameter)pr.ReadObject();
        var rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKey);
        var csp = new RSACryptoServiceProvider(); // cspParams);
        csp.ImportParameters(rsaParams);

        return csp;
    }

    public static byte[] NewGUIDKey(string currentKey, string appId) => RsaEncrypt(appId + currentKey).GetBytes();

    // RSACryptoServiceProvider rsp = ImportPublicKey();
    // byte[] encryptedData = rsp.Encrypt((appId + currentKey).GetBytes(), false);
    // return encryptedData;
    //return Convert.ToBase64String(encryptedData);
    /// <summary>
    /// 加密
    /// </summary>
    public static string RsaEncrypt(string rawInput /*, string publicKey*/)
    {
        if (string.IsNullOrEmpty(rawInput)) {
            return string.Empty;
        }

        // if (string.IsNullOrWhiteSpace(publicKey)) {
        //   throw new ArgumentException("Invalid Public Key");
        // }
        using (var rsaProvider = ImportPublicKey() /*new RSACryptoServiceProvider()*/) {
            var inputBytes = Encoding.UTF8.GetBytes(rawInput); //有含义的字符串转化为字节流

            //rsaProvider.FromXmlString(publicKey);              //载入公钥
            var bufferSize = 117; // (rsaProvider.KeySize / 8) - 11; //单块最大长度 1024/8-11=117
            var buffer = new byte[bufferSize];

            using (MemoryStream inputStream = new MemoryStream(inputBytes), outputStream = new MemoryStream()) {
                while (true) {
                    //分段加密
                    var readSize = inputStream.Read(buffer, 0, bufferSize);

                    if (readSize <= 0) {
                        break;
                    }
                    var temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    var encryptedBytes = rsaProvider.Encrypt(temp, false);
                    outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                }

                return Convert.ToBase64String(outputStream.ToArray()); //转化为字节流方便传输
            }
        }
    }

    /// <summary>
    /// 解密
    /// </summary>
    public static string RsaDecrypt(string encryptedInput /*, string privateKey*/)
    {
        if (string.IsNullOrEmpty(encryptedInput)) {
            return string.Empty;
        }

        // if (string.IsNullOrWhiteSpace(privateKey)) {
        //   throw new ArgumentException("Invalid Private Key");
        // }
        using (var rsaProvider = ImportPublicKey() /*new RSACryptoServiceProvider()*/) {
            var inputBytes = Convert.FromBase64String(encryptedInput);

            // rsaProvider.FromXmlString(privateKey);
            var bufferSize = 128; // rsaProvider.KeySize / 8; // 1024/8=128
            var buffer = new byte[bufferSize];

            using (MemoryStream inputStream = new MemoryStream(inputBytes), outputStream = new MemoryStream()) {
                while (true) {
                    var readSize = inputStream.Read(buffer, 0, bufferSize);

                    if (readSize <= 0) {
                        break;
                    }
                    var temp = new byte[readSize];
                    Array.Copy(buffer, 0, temp, 0, readSize);
                    var rawBytes = rsaProvider.Decrypt(temp, false);
                    outputStream.Write(rawBytes, 0, rawBytes.Length);
                }

                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }
    }

    public static string RepeatString(string str, int n)
    {
        var arr = str.ToCharArray();
        var arrDest = new char[arr.Length * n];

        for (var i = 0; i < n; i++) {
            Buffer.BlockCopy(arr, 0, arrDest, i * arr.Length * 2, arr.Length * 2);
        }

        return new string(arrDest);
    }

#if UNITY_EDITOR

    [PuertsIgnore]
    public static void test()
    {
        var seed = 123;
        var min = 0;
        var max = 255;
        var randomer = new CiaccoRandom();
        randomer.plantSeed(seed);
        randomer.getRand(min, max); // returns a pseudo-random int
    }

    [PuertsIgnore, MenuItem("Tests/[v1]Base56 Chars")]
    static void TestChars()
    {
        //var base56Test = "kC9UGXhEtN72aQcPRKdAjyW5VqBvsiueTxpHrgbYLDzSZ4w683mFMJfn";
        var str = Encoding.Default.GetString(base56);
        Debug.Log(str);
        Debug.Log(Convert.ToBase64String(pubkey));
        var testRC4 = RC4ENC("test", "hello");
        Debug.Log(testRC4);
        Debug.Log(RC4DEC("test", testRC4));
        var input = "test123testtttttttttttttttttest123testtttttttttttttttttest123testtttttttttttttttt";
        input = RepeatString(input, 20);
        var ret = Convert.ToBase64String(LZ4Codec.Wrap(Encoding.UTF8.GetBytes(input)));

        //var ret2 = Convert.ToBase64String(LZ4Utility.Compress(Encoding.UTF8.GetBytes(input)));
        Debug.Log(ret);

        //Debug.Log(ret2);

        //ret = "EwAAADcxMjMBAFAzMzMzMw==";
        Debug.Log(Encoding.UTF8.GetString(LZ4Codec.Unwrap(Convert.FromBase64String(ret))));

        //Debug.Log(Encoding.UTF8.GetString(LZ4Utility.Decompress(Convert.FromBase64String(ret2))));
        Debug.Log(new DeviceInfo(Core.NewGuid()).AppKey());
        Debug.Log(RsaEncrypt(input));
    }

#endif

}

}