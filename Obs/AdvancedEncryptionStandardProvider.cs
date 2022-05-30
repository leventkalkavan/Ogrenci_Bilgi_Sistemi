using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Obs
{
    public class AdvancedEncryptionStandardProvider
    {

        // Private properties
        private readonly ICryptoTransform _encryptor, _decryptor;
        private UTF8Encoding _encoder;
        // Generated with : https://www.allkeysgenerator.com/Random/Security-Encryption-Key-Generator.aspx
        private const string key = "z$C&F)J@NcRfUjXn";
        private const string secret = "3s6v9y$B&E)H+MbQ";

        public AdvancedEncryptionStandardProvider()
        {

            this._encoder = new UTF8Encoding();

            var _key = _encoder.GetBytes(key);
            var _secret = _encoder.GetBytes(secret);

            var managedAlgorithm = new RijndaelManaged();
            managedAlgorithm.BlockSize = 128;
            managedAlgorithm.KeySize = 128;

            this._encryptor = managedAlgorithm.CreateEncryptor(_key, _secret);
            this._decryptor = managedAlgorithm.CreateDecryptor(_key, _secret);
        }


        public string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(this._encoder.GetBytes(unencrypted)));
        }


        public string Decrypt(string encrypted)
        {
            return this._encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, this._encryptor);
        }


        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, this._decryptor);
        }


        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {

            var stream = new MemoryStream();

            using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }

            return stream.ToArray();
        }
    }
}
