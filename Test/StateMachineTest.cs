using Hake.Extension.StateMachine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test
{
    [TestClass]
    public class StateMachineTest
    {
        private bool Is147(int state, char ch) => "147".IndexOf(ch) >= 0;
        private bool Is0369(int state, char ch) => "0369".IndexOf(ch) >= 0;
        private bool Is258(int state, char ch) => "258".IndexOf(ch) >= 0;

        [TestMethod]
        public void TestTransform()
        {
            IStateMachine<int, char> machine = new StateMachine<int, char>();
            machine.Configure(0)
                .OnValue(0, '0')
                .OnValue(1, '1')
                .OnValue(2, '2')
                .OnValue(0, '3')
                .OnValue(1, '4')
                .OnValue(2, '5')
                .OnValue(0, '6')
                .OnValue(1, '7')
                .OnValue(2, '8')
                .OnValue(0, '9');

            machine.Configure(1)
                .OnCondition(1, Is0369)
                .OnCondition(2, Is147)
                .OnCondition(0, Is258);

            machine.Configure(2)
                .OnCondition(2, Is0369)
                .OnCondition(0, Is147)
                .OnCondition(1, Is258);

            Random random = new Random(Guid.NewGuid().GetHashCode());
            int times = 1000;
            int rem;
            int number;
            while (times-- > 0)
            {
                number = random.Next();
                rem = machine.Invoke(0, number.ToString());
                Assert.AreEqual(number % 3, rem);
            }
        }
    }
}
