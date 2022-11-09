using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGeneratorAPI.Verification
{
    public class HotpCodeGenerator : IOtpCodeGenerator
    {
        public HotpCodeGenerator()
        {
            Chilkat.Crypt2 crypt = new Chilkat.Crypt2();
        }
        public string GenerateCode()
        {
            string secret = "12345678901234567890";
            int count;
            for (count = 0; count <= 9; count++)
            {
                string counterHex = crypt.EncodeInt(count, 8, false, "hex");
                string hotp = crypt.Hotp(secret, "ascii", counterHex, 6, -1, "sha1");
                Debug.WriteLine(Convert.ToString(count) + "  HOTP = " + hotp);
            }
        }
    }
}
