using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Bedrock.Data.Components
{
    public abstract class Component(string id)
    {
        public string Id { get; protected set; } = id;
        public int HashId = id.GetHashCode();
        public abstract T CopyTo<T>(T component) where T : Component;
    }
}
