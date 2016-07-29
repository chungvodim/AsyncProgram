using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signaling
{
    public interface IBuffer
    {
        byte[] Buffer { get; }
    }

    public interface IBufferRegistration : IBuffer, IDisposable
    {

    }
}
