using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitExample
{
    [TestFixture]
    public class AccountTest
    {
        Account source;
        Account destination;

        [SetUp]
        public void Init()
        {
            source = new Account();
            source.Deposit(200m);

            destination = new Account();
            destination.Deposit(150m);
        }

        [Test]
        public void TransferFunds()
        {
            Init();
            source.TransferFunds(destination, 100m);

            Assert.AreEqual(250m, destination.Balance);
            Assert.AreEqual(100m, source.Balance);
        }

        [Test]
        public void DepoisitNegativeFunds()
        {
            Init();
            //This should fail: don't deposit negative amounts
            Assert.That(() => source.TransferFunds(destination, -100m), Throws.TypeOf<InvalidAmountException>());

            Assert.AreEqual(150m, destination.Balance);
            Assert.AreEqual(200m, source.Balance);
        }

        [Test]
        public void DepositZeroDollarFunds()
        {
            Init();
            //This should work, but have no impact
            source.TransferFunds(destination, 0m);

            Assert.AreEqual(150m, destination.Balance);
            Assert.AreEqual(200m, source.Balance);
        }

        [Test]
        public void DepositAdditionalFunds()
        {
            Init();
            source.Deposit(100m);
            destination.Deposit(100m);

            Assert.AreEqual(250m, destination.Balance);
            Assert.AreEqual(300m, source.Balance);
        }

        [Test]
        public void TransferWithInsufficientFunds()
        {
            Assert.That(() => source.TransferFunds(destination, 350m), Throws.TypeOf<InsufficientFundsException>());
        }

        [Test]
        [Ignore("Validate that this test meets requirements")]
        public void TransferWithInsufficientFundsAtomicity()
        {
            try
            {
                source.TransferFunds(destination, 300m);
            }
            catch (InsufficientFundsException expected)
            {
                System.Console.WriteLine(expected.Message);
            }

            Assert.AreEqual(200m, source.Balance);
            Assert.AreEqual(350m, destination.Balance);
        }
    }
}
