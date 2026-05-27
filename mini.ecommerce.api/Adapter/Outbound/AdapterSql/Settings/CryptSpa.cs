using System.Security.Cryptography;
using System.Text;

namespace mini.ecommerce.api.Adapter.Outbound.AdapterSql.Settings;

public class CryptSpa
{
    public string Chave { get; set; } = "k9#mP2!x";

    public string DecryptDES(string sDados, string sChave)
    {
        DES des = DES.Create();

        des.Mode = CipherMode.ECB;
        des.Key = Encoding.UTF8.GetBytes(sChave);
        des.Padding = PaddingMode.PKCS7;

        ICryptoTransform desencrypt = des.CreateDecryptor();
        byte[] buffer = Convert.FromBase64String(sDados);

        return Encoding.UTF8.GetString(desencrypt.TransformFinalBlock(buffer, 0, buffer.Length));
    }
}