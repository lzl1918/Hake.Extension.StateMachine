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
                .OnValue('0', 0)
                .OnValue('1', 1)
                .OnValue('2', 2)
                .OnValue('3', 0)
                .OnValue('4', 1)
                .OnValue('5', 2)
                .OnValue('6', 0)
                .OnValue('7', 1)
                .OnValue('8', 2)
                .OnValue('9', 0);

            machine.Configure(1)
                .OnCondition(Is0369, 1)
                .OnCondition(Is147, 2)
                .OnCondition(Is258, 0);

            machine.Configure(2)
                .OnCondition(Is0369, 2)
                .OnCondition(Is147, 0)
                .OnCondition(Is258, 1);

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

    [TestClass]
    public class ProcessTest
    {
        [TestMethod]
        public void TestProcess()
        {
            IStateMachine<int, char> proc = new StateMachine<int, char>();
            proc.Configure(0)
                .OnValue('3', 1);

            proc.Configure(1)
                .OnValue('4', 2);

            proc.Configure(2)
                .OnValue('0', 3, arg =>
                {
                    arg.FollowingAction = FollowingAction.Stop;
                });


            IStateMachine<int, char> main = new StateMachine<int, char>();
            main.Configure(0)
                .OnValue('1', 1)
                .OnValueKeep('0');

            main.Configure(1)
                .OnValue('2', 2, arg =>
                {
                    arg.SetShift(proc, 0, i => i == 3 ? 0 : i);
                })
                .OnValue('0', 0)
                .OnValueKeep('1');

            string input = "01234012";
            int state = main.Invoke(0, input);
            Assert.AreEqual(0, state);
        }
    }
}
