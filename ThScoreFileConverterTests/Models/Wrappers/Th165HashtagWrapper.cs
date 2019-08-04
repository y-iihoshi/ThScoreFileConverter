using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    public sealed class Th165HashtagWrapper
    {
        private static readonly Type ParentType = typeof(Th165Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+Hashtag";

        private readonly PrivateObject pobj = null;

        public Th165HashtagWrapper(bool outputs, string name)
            => this.pobj = new PrivateObject(
                AssemblyNameToTest, TypeNameToTest, new object[] { outputs, name });

        public object Target
            => this.pobj.Target;
        public bool? Outputs
            => this.pobj.GetProperty(nameof(this.Outputs)) as bool?;
        public string Name
            => this.pobj.GetProperty(nameof(this.Name)) as string;
    }
}
