using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace HanzoProxyAPI.Core
{
    class HttpRequestAuth
    {
        private string _apikey = string.Empty;
        private string _subid = string.Empty;
        private string _authtoken = string.Empty;
        private bool _isHeaderVerified = false;
        private bool _isTokenAuthenticated = false;

        public HttpRequestAuth(HttpRequestMessage req)
        {
            var headers = req.Headers;
            if (headers.Contains("Api-Key"))
            {
                this._apikey = headers.GetValues("Api-Key").First();
                // do some api-key matching 
                // fixme
            }
            if (headers.Contains("Api-Sig"))
            {
                string apisig = headers.GetValues("Api-Sig").First();

                string decryptedsig = DecryptString(apisig, this._apikey);
                if (!string.IsNullOrEmpty(decryptedsig))
                {
                    char[] d1 = { '&' };
                    char[] d2 = { '=' };
                    string[] ws = decryptedsig.Split(d1);
                    if (ws.Length == 2)
                    {
                        foreach (string w in ws)
                        {
                            string[] ss = w.Split(d2);
                            if (ss.Length == 2 && ss[0].Equals("Sub-ID"))
                            {
                                this._subid = ss[1];
                            }
                            if (ss.Length == 2 && ss[0].Equals("Auth-Token"))
                            {
                                this._authtoken = ss[1];
                                this._isTokenAuthenticated = true;
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(this._apikey)   
                && !string.IsNullOrEmpty(this._subid) 
                && !string.IsNullOrEmpty(this._authtoken) )
            {
                this._isHeaderVerified = true;
            }
        }

        public string GetApiKey()
        {
            return this._apikey;
        }

        public string GetSubID()
        {
            return this._subid;
        }

        public string GetTokenCache()
        {
            return this._authtoken;
        }

        public bool IsHeaderVerfied()
        {
            return this._isHeaderVerified;
        }

        public bool IsTokenAuthenticated()
        {
            return this._isTokenAuthenticated;
        }

        private static string DecryptString(string sourceString, string password)
        {
            System.Security.Cryptography.RijndaelManaged rijndael =
                new System.Security.Cryptography.RijndaelManaged();

            byte[] key, iv;
            GenerateKeyFromPassword(
                password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
            rijndael.Key = key;
            rijndael.IV = iv;

            byte[] strBytes = System.Convert.FromBase64String(sourceString);

            System.Security.Cryptography.ICryptoTransform decryptor =
                rijndael.CreateDecryptor();
            byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            decryptor.Dispose();
            return System.Text.Encoding.UTF8.GetString(decBytes);
        }

        private static void GenerateKeyFromPassword(string password,
            int keySize, out byte[] key, int blockSize, out byte[] iv)
        {
            byte[] salt = System.Text.Encoding.UTF8.GetBytes("salthastobemorethanandequalto8bytes");
            System.Security.Cryptography.Rfc2898DeriveBytes deriveBytes =
                new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt);
            deriveBytes.IterationCount = 1000;
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }

    }
}
