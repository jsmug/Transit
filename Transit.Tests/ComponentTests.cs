using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Transit.Tests.Components;

namespace Transit.Tests
{

    [TestClass]
    public class ComponentTests
    {

        public ComponentTests()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }


        #region public

        [TestMethod]
        public void CanSetComponentName()
        {

            string name = "Component";
            InComponent component = new InComponent(name);

            Assert.IsTrue(string.Compare(name, component.Name, false) == 0);
             
        }

        [TestMethod]
        public void ComponentHasUniqueId()
        {

            InComponent component1 = new InComponent("Component1");
            InComponent component2 = new InComponent("Component2");

            Assert.IsTrue(component1.Id != component2.Id);

        }

        #endregion

    }

}
