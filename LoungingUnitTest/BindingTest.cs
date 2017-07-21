using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using TouchUnit.Bindings;
using HypeFramework;

namespace LoungingUnitTest
{
	[TestFixture]
	public class BindingTest : ApiCtorInitTest
	{
		protected override Assembly Assembly
		{
			get { return typeof(HYP).Assembly; }
		}
	}
}

