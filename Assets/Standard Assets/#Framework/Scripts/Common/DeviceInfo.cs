using GameEngine.Extensions;
using GameEngine.Kernel;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common {

/// <summary>
/// the DeviceInfo class is used to collect all technical details to be included in any debug report.
/// this class can be easily encoded into JSON by JsonUtility.ToJson(new DeviceInfo())
/// </summary>
[Serializable]
public class DeviceInfo {

    public string AppId;
    public string platform;
    public string systemLanguage;

    // public string internetReachability;
    public string operatingSystem;
    public string operatingSystemFamily;
    public string deviceModel;
    public string deviceName;
    public string deviceType;
    public int systemMemorySize;
    public int screenWidth;
    public int screenHeight;
    public int screenDpi;
    public int graphicsDeviceID;
    public string graphicsDeviceName;
    public string graphicsDeviceType;
    public string graphicsDeviceVendor;
    public int graphicsDeviceVendorID;
    public string graphicsDeviceVersion;
    public int graphicsMemorySize;
    public bool graphicsMultiThreaded;
    public int graphicsShaderLevel;
    public bool supportsGyroscope;
    public bool supportsVibration;
    public bool supportsAccelerometer;
    public bool supportsLocationService;
    public bool supportsARGB32RenderTexture;
    public bool supportsAlpha8Texture;

    public DeviceInfo(string appId)
    {
        AppId = appId; //"E33F868C-B53E-4085-A6DC-2DA2533760C7";

        // Application.version; // AppConstants.AppVersion;
        platform = Application.platform.ToString();
        systemLanguage = Application.systemLanguage.ToString();

        //internetReachability = Application.internetReachability.ToString();
        operatingSystem = SystemInfo.operatingSystem;
        operatingSystemFamily = SystemInfo.operatingSystemFamily.ToString();
        deviceModel = SystemInfo.deviceModel;
        deviceName = SystemInfo.deviceName;
        deviceType = SystemInfo.deviceType.ToString();
        systemMemorySize = SystemInfo.systemMemorySize;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        screenDpi = Mathf.RoundToInt(Screen.dpi);
        graphicsDeviceID = SystemInfo.graphicsDeviceID;
        graphicsDeviceName = SystemInfo.graphicsDeviceName;
        graphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();
        graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;
        graphicsDeviceVendorID = SystemInfo.graphicsDeviceVendorID;
        graphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;
        graphicsMemorySize = SystemInfo.graphicsMemorySize;
        graphicsMultiThreaded = SystemInfo.graphicsMultiThreaded;
        graphicsShaderLevel = SystemInfo.graphicsShaderLevel;
        supportsGyroscope = SystemInfo.supportsGyroscope;
        supportsVibration = SystemInfo.supportsVibration;
        supportsAccelerometer = SystemInfo.supportsAccelerometer;
        supportsLocationService = SystemInfo.supportsLocationService;
        supportsARGB32RenderTexture = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32);
        supportsAlpha8Texture = SystemInfo.SupportsTextureFormat(TextureFormat.Alpha8);

        // a6d0a64e9647b956de084f7ff6ed8f8c
        var path = Path.Combine(Application.persistentDataPath, "data".md5());

        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        // 08abe002452a8e91e440edb368e2c951
        var file = Path.Combine(path, "install".md5());
        var fileInfo = new FileInfo(path);
        AppId = $"{fileInfo.CreationTime.GetTimeStampJS()}{AppKey()}".md5();

        if (!File.Exists(file)) {
        #if UNITY_EDITOR
            Debug.Log(AppId);
        #endif
            File.WriteAllText(file,
                Convert.FromBase64String(CiaccoRandom.RsaEncrypt(appId + AppId)).GetString() +
                CiaccoRandom.RC4(AppId, AppId.GetBytes()).GetString());
        }
        var len = CiaccoRandom.RC4(AppId, AppId.GetBytes()).Length;
        var content = File.ReadAllText(file);
        AppId = CiaccoRandom.RC4(AppId, content.Substring(content.Length - len).GetBytes()).GetString();
    }

    public string AppKey()
    {
        var info = JsonUtility.ToJson(this).md5();

        return info;
    }

    //
    // #if UNITY_EDITOR
    //   [MenuItem("Test/AppInfo")]
    //   static void Debug()
    //   {
    //     var info = JsonUtility.ToJson(new DeviceInfo());
    //     UnityEngine.Debug.Log($"{info} {info.md5()}");
    //   }
    //
    //   [MenuItem("Test/Ecc")]
    //   static void TestEcc()
    //   {
    //     // ECDiffieHellmanCng sender = new ECDiffieHellmanCng();
    //     // sender.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
    //     // sender.HashAlgorithm = CngAlgorithm.Sha256;
    //     // var senderPublicKey = sender.PublicKey.ToByteArray();
    //
    //     // var senderPublicKey = sender.PublicKey.ToByteArray();
    //     // //var senderPrivateKey = sender.
    //     // var senderPrivateKey =
    //     //   sender.DeriveKeyMaterial(CngKey.Import(receiverPublicKey, CngKeyBlobFormat.EccPublicBlob));
    //     var aliceKeyPair = CreateKeyPair();
    //     var bobKeyPair = CreateKeyPair();
    //     byte[] bobSharedSecret = CreateSharedSecret(bobKeyPair.privateKey, aliceKeyPair.publicKey);
    //     byte[] aliceSharedSecret = CreateSharedSecret(aliceKeyPair.privateKey, bobKeyPair.publicKey);
    //
    //     // derived shared secrets are the same - the whole point of this algoritm
    //     UnityEngine.Debug.Assert(aliceSharedSecret.SequenceEqual(bobSharedSecret));
    //     KeyValuePair<string, string> keyPair = Encrypter.CreateRSAKey();
    //     string privateKey = keyPair.Value;
    //     string publicKey = keyPair.Key;
    //     var input = "test hello";
    //     var result = Encrypter.EncryptByRSA(input, publicKey);
    //     UnityEngine.Debug.LogFormat("RSA公钥加密后的结果：{0}", result);
    //     result = Encrypter.DecryptByRSA(result, privateKey);
    //     Console.WriteLine("RSA私钥解密后的结果：{0}", result);
    //   }
    // #endif
    //   public static byte[] CreatePublicKey()
    //   {
    //     using (ECDiffieHellmanCng cng = new ECDiffieHellmanCng()) {
    //       cng.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
    //       cng.HashAlgorithm = CngAlgorithm.Sha512;
    //       return cng.PublicKey.ToByteArray();
    //     }
    //   }
    //
    //   public static byte[] CreatePrivateKey(byte[] publicKey1, byte[] publicKey2)
    //   {
    //     using (ECDiffieHellmanCng cng =
    //       new ECDiffieHellmanCng(CngKey.Import(publicKey1, CngKeyBlobFormat.EccPublicBlob))) {
    //       cng.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
    //       cng.HashAlgorithm = CngAlgorithm.Sha512;
    //       return cng.DeriveKeyMaterial(CngKey.Import(publicKey2, CngKeyBlobFormat.EccPublicBlob));
    //     }
    //   }
    //
    //   public static (byte[] publicKey, byte[] privateKey) CreateKeyPair()
    //   {
    //     using (ECDiffieHellmanCng cng = new ECDiffieHellmanCng(
    //
    //       // need to do this to be able to export private key
    //       CngKey.Create(CngAlgorithm.ECDiffieHellmanP256, null,
    //         new CngKeyCreationParameters { ExportPolicy = CngExportPolicies.AllowPlaintextExport }))) {
    //       cng.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
    //       cng.HashAlgorithm = CngAlgorithm.Sha512;
    //
    //       // export both private and public keys and return
    //       var pr = cng.Key.Export(CngKeyBlobFormat.EccPrivateBlob);
    //       var pub = cng.PublicKey.ToByteArray();
    //       return (pub, pr);
    //     }
    //   }
    //
    //   public static byte[] CreateSharedSecret(byte[] privateKey, byte[] publicKey)
    //   {
    //     // this returns shared secret, not private key
    //     // initialize algorithm with private key of one party
    //     using (ECDiffieHellmanCng cng =
    //       new ECDiffieHellmanCng(CngKey.Import(privateKey, CngKeyBlobFormat.EccPrivateBlob))) {
    //       cng.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
    //       cng.HashAlgorithm = CngAlgorithm.Sha512;
    //
    //       // use public key of another party
    //       return cng.DeriveKeyMaterial(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob));
    //     }
    //   }

    //
    // public string Decription(byte[] encryptedData, byte[] alicePubKeyBlob)
    // {
    //   byte[] rawData = null;
    //
    //   var aes = new AesCryptoServiceProvider();
    //
    //   int nBytes = aes.BlockSize >> 3;
    //   byte[] iv = new byte[nBytes];
    //   for (int i = 0; i < iv.Length; i++)
    //     iv[i] = encryptedData[i];
    //
    //   using (var bobAlgorithm = new ECDiffieHellmanCng(privKey))
    //   using (CngKey alicePubKey = CngKey.Import(alicePubKeyBlob,
    //     CngKeyBlobFormat.EccPublicBlob))
    //   {
    //     byte[] symmKey = bobAlgorithm.DeriveKeyMaterial(alicePubKey);
    //     aes.Key = symmKey;
    //     aes.IV = iv;
    //
    //     using (ICryptoTransform decryptor = aes.CreateDecryptor())
    //     using (MemoryStream ms = new MemoryStream())
    //     {
    //       var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);
    //       cs.Write(encryptedData, nBytes, encryptedData.Length - nBytes);
    //       cs.Close();
    //
    //       rawData = ms.ToArray();
    //     }
    //     aes.Clear();
    //   }
    //   return Encoding.UTF8.GetString(rawData);
    // }

}

}