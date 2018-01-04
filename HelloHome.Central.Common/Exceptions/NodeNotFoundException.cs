using System;

namespace HelloHome.Common.Exceptions
{
    [Serializable]
    public class NodeNotFoundException : HelloHomeException
    {
        public NodeNotFoundException(int rfAddress)
            : base("NODE_NOT_FOUND", $"Node with rfId {rfAddress} not found.")
        {

        }
        public NodeNotFoundException(long signature)
            : base("NODE_NOT_FOUND", $"Node with signature {signature} not found.")
        {

        }
    }
}