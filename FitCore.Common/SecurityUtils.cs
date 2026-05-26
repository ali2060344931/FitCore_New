using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using HashidsNet;

namespace FitCore.Common
{
    /// <summary>
    /// رمزنگاری روی کدها هر جدول
    /// </summary>

    public static class SecurityUtils
    {
        private const string Salt = "MySecretSalt_FitCore_2024"; // این کلید را مخفی نگه دارید
        private static readonly Hashids _hashids = new Hashids(Salt, minHashLength: 8);

        public static string EncryptId(long id) => _hashids.EncodeLong(id);

        public static long DecryptId(string encryptedId)
        {
            var decoded = _hashids.DecodeLong(encryptedId);
            return decoded.Length > 0 ? decoded[0] : 0;
        }
    }
}
