using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cbk.cloudUploadImage.Infrastructure.Security.Jwt
{
    public class JwtSettings
    {
        public string Key => "JWT";

        /// <summary>
        /// Token發行者
        /// </summary>
        /// <value></value>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Token私鑰
        /// </summary>
        /// <value></value>
        public string TokenSecret { get; set; } = string.Empty;

        /// <summary>
        /// 有效期限的天數
        /// </summary>
        /// <value></value>
        public int ExpiredDay { get; set; }
    }
}
