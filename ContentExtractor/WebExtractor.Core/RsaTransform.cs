using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace ContentExtractor.Core
{
  public class RsaTransform : ICryptoTransform
  {
    public enum Direction
    {
      Encrypt,
      Decrypt
    }

    public RsaTransform(RSACryptoServiceProvider rsa, Direction direction)
    {
      this.rsa = rsa;
      this.direction = direction;
      parameters = rsa.ExportParameters(false);
    }
    private RSACryptoServiceProvider rsa;
    private Direction direction;
    private RSAParameters parameters;

    public bool CanReuseTransform
    {
      get { return true; }
    }

    public bool CanTransformMultipleBlocks
    {
      get { return false; }
    }

    private int SouceSize
    {
      get { return parameters.Modulus.Length - 11; }
    }
    private int CodedSize
    {
      get { return parameters.Modulus.Length; }
    }

    public int InputBlockSize
    {
      get
      {
        if (direction == Direction.Encrypt)
          return SouceSize;
        else// if (direction == Direction.Decrypt)
          return CodedSize;
      }
    }

    public int OutputBlockSize
    {
      get
      {
        if (direction == Direction.Decrypt)
          return SouceSize;
        else// if (direction == Direction.Encrypt)
          return CodedSize;
      }
    }

    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      byte[] buffer = new byte[inputCount];
      if (inputCount > 0)
      {
        Buffer.BlockCopy(inputBuffer, inputOffset, buffer, 0, inputCount);
        byte[] coded = Transform(buffer);
        Buffer.BlockCopy(coded, 0, outputBuffer, outputOffset, coded.Length);
        return coded.Length;
      }
      else
        return 0;
    }

    private byte[] Transform(byte[] buffer)
    {
      if (direction == Direction.Encrypt)
        return rsa.Encrypt(buffer, false);
      else if (direction == Direction.Decrypt)
        return rsa.Decrypt(buffer, false);
      else
        return new byte[0];
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      byte[] buffer = new byte[inputCount];
      if (inputCount > 0)
      {
        Buffer.BlockCopy(inputBuffer, inputOffset, buffer, 0, inputCount);
        return Transform(buffer);
      }
      else
        return buffer;
    }

    public void Dispose()
    {
    }
  }

}
