using System;
using System.Buffers.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace DotNetAtom.Crypto
{
    public class SymmetricCryptography
    {
        #region Fields

        private readonly SymmetricAlgorithm _provider;
        private readonly byte[] _key;
        private readonly byte[] _iv;

        #endregion Fields

        #region Constructors

        public SymmetricCryptography(SymmetricAlgorithm provider)
        {
            _provider = provider;
            _provider.GenerateKey();
            _key = _provider.Key;
            _provider.GenerateIV();
            _iv = _provider.IV;
        }

        public SymmetricCryptography(SymmetricAlgorithm provider, byte[] key, byte[]? iv = null)
        {
            _provider = provider;

            _key = key;

            if (iv == null)
            {
                _provider.Key = key;
                _provider.GenerateIV();
                iv = _provider.IV;
            }

            _iv = iv;
        }

        #endregion Constructors

        #region Byte Array Methods

        public byte[] Encrypt(byte[] input)
        {
            return Encrypt(input, _key, _iv);
        }

        public byte[] Encrypt(byte[] input, int offset, int length)
        {
            return Transform(input, offset, length, _provider.CreateEncryptor(_key, _iv));
        }

        public byte[] Decrypt(byte[] input)
        {
            return Decrypt(input, _key, _iv);
        }

        public byte[] Encrypt(byte[] input, byte[] key, byte[] iv)
        {
            return Transform(input, 0, input.Length, _provider.CreateEncryptor(key, iv));
        }

        public byte[] Decrypt(byte[] input, byte[] key, byte[] iv)
        {
            return Transform(input, 0, input.Length, _provider.CreateDecryptor(key, iv));
        }

        #endregion Byte Array Methods

        #region String Methods

        public string Encrypt(string text)
        {
            return Encrypt(text, _key, _iv);
        }

        public string Decrypt(string text)
        {
            return Decrypt(text, _key, _iv);
        }

        public string Encrypt(string text, byte[] key, byte[] iv)
        {
            using var encryptor = _provider.CreateEncryptor(key, iv);
            var bytes = Encoding.UTF8.GetBytes(text);
            var output = Transform(bytes, 0, bytes.Length, encryptor);
            return Convert.ToBase64String(output);
        }

        public string Decrypt(string text, byte[] key, byte[] iv)
        {
            using var decryptor = _provider.CreateDecryptor(key, iv);
            var bytes = Convert.FromBase64String(text);
            var output = Transform(bytes, 0, bytes.Length, decryptor);
            return Encoding.UTF8.GetString(output);
        }

        #endregion String Methods

        #region SecureString Methods

        public byte[] Encrypt(SecureString input)
        {
            return Encrypt(input, _key, _iv);
        }

        public void Decrypt(byte[] input, out SecureString output)
        {
            Decrypt(input, out output, _key, _iv);
        }

        public byte[] Encrypt(SecureString input, byte[] key, byte[] iv)
        {
            // defensive argument checking
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var inputPtr = IntPtr.Zero;

            try
            {
                // copy the SecureString to an unmanaged BSTR
                // and get back the pointer to the memory location
                inputPtr = Marshal.SecureStringToBSTR(input);
                if (inputPtr == IntPtr.Zero)
                    throw new InvalidOperationException("Unable to allocate" +
                                                        "necessary unmanaged resources.");

                var inputBuffer = new char[input.Length];

                try
                {
                    // pin the buffer array so the GC doesn't move it while we
                    // are doing an unmanaged memory copy, but make sure we
                    // release the pin when we are done so that the CLR can do
                    // its thing later
                    var handle = GCHandle.Alloc(inputBuffer,
                        GCHandleType.Pinned);
                    try
                    {
                        Marshal.Copy(inputPtr, inputBuffer, 0, input.Length);
                    }
                    finally
                    {
                        handle.Free();
                    }

                    // encode the input as UTF8 first so that we have a
                    // way to explicitly "flush" the byte array afterwards
                    var utf8Buffer = Encoding.UTF8.GetBytes(inputBuffer);
                    try
                    {
                        return Encrypt(utf8Buffer, key, iv);
                    }
                    finally
                    {
                        Array.Clear(utf8Buffer, 0, utf8Buffer.Length);
                    }
                }
                finally
                {
                    Array.Clear(inputBuffer, 0, inputBuffer.Length);
                }
            }
            finally
            {
                // because we are using unmanaged resources, we *must*
                // explicitly deallocate those resources ourselves
                if (inputPtr != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(inputPtr);
            }
        }

        public void Decrypt(byte[] input, out SecureString output, byte[] key,
            byte[] iv)
        {
            byte[]? decryptedBuffer = null;

            try
            {
                // do our normal decryption of a byte array
                decryptedBuffer = Decrypt(input, key, iv);

                char[]? outputBuffer = null;

                try
                {
                    // convert the decrypted array to an explicit
                    // character array that we can "flush" later
                    outputBuffer = Encoding.UTF8.GetChars(decryptedBuffer);

                    // Create the result and copy the characters
                    output = new SecureString();
                    try
                    {
                        for (var i = 0; i < outputBuffer.Length; i++)
                        {
                            output.AppendChar(outputBuffer[i]);
                        }
                    }
                    finally
                    {
                        output.MakeReadOnly();
                    }
                }
                finally
                {
                    if (outputBuffer != null)
                        Array.Clear(outputBuffer, 0, outputBuffer.Length);
                }
            }
            finally
            {
                if (decryptedBuffer != null)
                    Array.Clear(decryptedBuffer, 0, decryptedBuffer.Length);
            }
        }

        #endregion SecureString Methods

        #region Stream Methods

        public void Encrypt(Stream input, Stream output)
        {
            Encrypt(input, output, _key, _iv);
        }

        public void Decrypt(Stream input, Stream output)
        {
            Decrypt(input, output, _key, _iv);
        }

        public void Encrypt(Stream input, Stream output, byte[] key, byte[] iv)
        {
            TransformStream(true, ref input, ref output, key, iv);
        }

        public void Decrypt(Stream input, Stream output, byte[] key, byte[] iv)
        {
            TransformStream(false, ref input, ref output, key, iv);
        }

        #endregion Stream Methods

        #region Private Methods

        private static byte[] Transform(byte[] input, int offset, int length, ICryptoTransform cryptoTransform)
        {
            using var memStream = new MemoryStream();
            using var cryptStream = new CryptoStream(memStream, cryptoTransform, CryptoStreamMode.Write);

            cryptStream.Write(input, offset, length);
            cryptStream.FlushFinalBlock();

            return memStream.ToArray();
        }

        private void TransformStream(bool encrypt, ref Stream input, ref Stream output, byte[] key, byte[] iv)
        {
            // defensive argument checking
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (!input.CanRead)
            {
                throw new ArgumentException("Unable to read from the input Stream.", nameof(input));
            }

            if (!output.CanWrite)
            {
                throw new ArgumentException("Unable to write to the output Stream.", nameof(output));
            }

            // make the buffer just large enough for
            // the portion of the stream to be processed
            var inputBuffer = new byte[input.Length - input.Position];
            // read the stream into the buffer
            input.Read(inputBuffer, 0, inputBuffer.Length);
            // transform the buffer
            var outputBuffer = encrypt
                ? Encrypt(inputBuffer, key, iv)
                : Decrypt(inputBuffer, key, iv);
            // write the transformed buffer to our output stream
            output.Write(outputBuffer, 0, outputBuffer.Length);
        }

        #endregion Private Methods
    }
}