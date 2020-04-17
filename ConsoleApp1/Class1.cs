using SHA3.Net;
using System;
using System.Security.Cryptography;

namespace ConsoleApp1
{
    public class HMACChiko2 : KeyedHashAlgorithm
    {
        private Sha3 hash1;
        private Sha3 hash2;
        private bool bHashing = false;
        private byte[] rgbInner = new byte[128];
        private byte[] rgbOuter = new byte[128];
        public HMACChiko2(byte[] rgbKey)
        {
            HashSizeValue = 512;
            // Create the hash algorithms.
            hash1 = Sha3.Sha3512();
            hash2 = Sha3.Sha3512();
            // Get the key.
            if (rgbKey.Length > 128)
            {
                KeyValue = hash1.ComputeHash(rgbKey);
                // No need to call Initialize; ComputeHash does it automatically.
            }
            else
            {
                KeyValue = (byte[])rgbKey.Clone();
            }
            // Compute rgbInner and rgbOuter.
            int i = 0;
            for (i = 0; i < 128; i++)
            {
                rgbInner[i] = 0x36;
                rgbOuter[i] = 0x5C;
            }
            for (i = 0; i < KeyValue.Length; i++)
            {
                rgbInner[i] ^= KeyValue[i];
                rgbOuter[i] ^= KeyValue[i];
            }
        }
        public override byte[] Key
        {
            get { return (byte[])KeyValue.Clone(); }
            set
            {
                if (bHashing)
                {
                    throw new Exception("Cannot change key during hash operation");
                }
                if (value.Length > 128)
                {
                    KeyValue = hash1.ComputeHash(value);
                    // No need to call Initialize; ComputeHash does it automatically.
                }
                else
                {
                    KeyValue = (byte[])value.Clone();
                }
                // Compute rgbInner and rgbOuter.
                int i = 0;
                for (i = 0; i < 128; i++)
                {
                    rgbInner[i] = 0x36;
                    rgbOuter[i] = 0x5C;
                }
                for (i = 0; i < KeyValue.Length; i++)
                {
                    rgbInner[i] ^= KeyValue[i];
                    rgbOuter[i] ^= KeyValue[i];
                }
            }
        }
        public override void Initialize()
        {
            hash1.Initialize();
            hash2.Initialize();
            bHashing = false;
        }
        protected override void HashCore(byte[] rgb, int ib, int cb)
        {
            if (bHashing == false)
            {
                hash1.TransformBlock(rgbInner, 0, 128, rgbInner, 0);
                bHashing = true;
            }
            hash1.TransformBlock(rgb, ib, cb, rgb, ib);
        }
        protected override byte[] HashFinal()
        {
            if (bHashing == false)
            {
                hash1.TransformBlock(rgbInner, 0, 128, rgbInner, 0);
                bHashing = true;
            }
            // Finalize the original hash.
            hash1.TransformFinalBlock(new byte[0], 0, 0);
            // Write the outer array.
            hash2.TransformBlock(rgbOuter, 0, 128, rgbOuter, 0);
            // Write the inner hash and finalize the hash.
            hash2.TransformFinalBlock(hash1.Hash, 0, hash1.Hash.Length);
            bHashing = false;
            return hash2.Hash;
        }
    }
}
