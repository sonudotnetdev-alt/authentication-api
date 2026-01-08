using Authentication.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser appuser);
    }
}
