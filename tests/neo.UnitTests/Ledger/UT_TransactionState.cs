using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.Network.P2P.Payloads;
using Neo.SmartContract;
using Neo.SmartContract.Native;
using Neo.VM;
using System;
using System.IO;

namespace Neo.UnitTests.Ledger
{
    [TestClass]
    public class UT_TransactionState
    {
        TransactionState origin;

        [TestInitialize]
        public void Initialize()
        {
            origin = new TransactionState
            {
                BlockIndex = 1,
                Transaction = new Transaction()
                {
                    Attributes = Array.Empty<TransactionAttribute>(),
                    Script = new byte[] { (byte)OpCode.PUSH1 },
                    Signers = new Signer[] { new Signer() { Account = UInt160.Zero } },
                    Witnesses = new Witness[] { new Witness() {
                        InvocationScript=Array.Empty<byte>(),
                        VerificationScript=Array.Empty<byte>()
                    } }
                }
            };
        }

        [TestMethod]
        public void TestDeserialize()
        {
            using MemoryStream ms = new MemoryStream(1024);
            using BinaryReader reader = new BinaryReader(ms);

            var data = BinarySerializer.Serialize(((IInteroperable)origin).ToStackItem(null), 1024);
            ms.Write(data);
            ms.Seek(0, SeekOrigin.Begin);

            TransactionState dest = new TransactionState();
            ((IInteroperable)dest).FromStackItem(BinarySerializer.Deserialize(reader, 1024, 1024, null));

            dest.BlockIndex.Should().Be(origin.BlockIndex);
            dest.Transaction.Hash.Should().Be(origin.Transaction.Hash);
        }
    }
}
