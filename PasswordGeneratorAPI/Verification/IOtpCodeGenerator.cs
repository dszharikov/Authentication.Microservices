using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGeneratorAPI.Verification
{
    public interface IOtpCodeGenerator
    {
        string GenerateCode();
    }
}
