using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DncyTemplate.Domain.Infra
{
    /// <summary>
    /// 错误结果，配合Return使用
    /// </summary>
    /// <param name="Message"></param>
    public record ErrorResult(string Message);
}
