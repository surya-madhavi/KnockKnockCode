using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RedPill
{
    [DataContract(Namespace="http://KnockKnock.readify.net")]
  public class ContactDetails
  {
    [DataMember]
    public string EmailAddress;
    [DataMember]
    public string FamilyName;
    [DataMember]
    public string GivenName;
    [DataMember]
    public string PhoneNumber;
  }

  [DataContract(Namespace="http://KnockKnock.readify.net")]
  public enum TriangleType 
  { 
    [EnumMember] 
    Error,
    [EnumMember]
    Equilateral,
    [EnumMember]
    Isosceles,
    [EnumMember]
    Scalene
  }

  [ServiceContract(Namespace="http://KnockKnock.readify.net")]
  public interface IRedPill
  {
    [OperationContract, FaultContract(typeof(ArgumentOutOfRangeException))]
    long FibonacciNumber(long n);
    [OperationContract, FaultContract(typeof(ArgumentNullException))]
    string ReverseWords(string s);
    [OperationContract]
    TriangleType WhatShapeIsThis(int a, int b, int c);
    [OperationContract]
    Guid WhatIsYourToken();
  }

  [ServiceBehavior(Namespace = "http://KnockKnock.readify.net")]
  public class RedPill : IRedPill
  {
    private static double x = ((1d + Math.Sqrt(5d)) / 2d);
    private static double y = 1d / Math.Sqrt(5d);


    /// <summary>
    /// Fibonnacci Number
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public long FibonacciNumber(long n)
    {
      if (!(-92 <= n && n <= 92)) Fault(new ArgumentOutOfRangeException("n", "Require 0 <= n <= 92"));

      try
      {
          long inp = n;

          // in case of negative number multiply with -1  as per the formula in 
          if (n < 0)
          {
              inp = n * -1;
          }

          if (inp == 0) return 0;
          if (inp == 1) return 1;


          long prevPrev = 0;
          long prev = 1;
          long result = 0;


          for (long i = 2; i <= inp; i++)
          {
              result = prev + prevPrev;
              prevPrev = prev;
              prev = result;
          }


          if (n < 0)
          {
              result = result * -1;
          }
          return result;
      }    
      catch (Exception e)
      {
          throw new Exception(e.Message);
      }
    }

    public string ReverseWords(string s)
    {
      if (s == null) Fault(new ArgumentNullException("s", "Require s != null"));

      string[] str = s.Split(' ');
      StringBuilder sb = new StringBuilder();
      foreach (string s1 in str)
      {
          sb.Append(ReverseString(s1));
          sb.Append(' ');
      }
      return sb.ToString();
      
      
     
    }
    public System.Threading.Tasks.Task<long> FibonacciNumberAsync(long n)
    {
        return Task.Factory.StartNew<long>(() => FibonacciNumber(n));
    }
   
    /// <summary>
    /// To reverse a word
    /// </summary>
    /// <param name="s">Input</param>
    /// <returns>Reverse string</returns>
    public string ReverseString(string s)
    {
        char[] charArray = new char[s.Length];
        int len = s.Length - 1;
        for (int i = 0; i <= len; i++)
            charArray[i] = s[len - i];
        return new string(charArray);


    }
    /// <summary>
    /// Method to determine type of Triangle
    /// </summary>
    /// <param name="a">side1</param>
    /// <param name="b">side2</param>
    /// <param name="c">side3</param>
    /// <returns></returns>
    public TriangleType WhatShapeIsThis(int a, int b, int c)
    {
      if (a <= 0 || b <= 0 || c <= 0) return TriangleType.Error;
      else if (!(a < b + c && b < a + c && c < a + b)) return TriangleType.Error;
      else if (a == b && b == c) return TriangleType.Equilateral;
      else if (a == b || a == c || b == c) return TriangleType.Isosceles;
      else return TriangleType.Scalene;
    }

    public System.Threading.Tasks.Task<TriangleType> WhatShapeIsThisAsync(int a, int b, int c)
    {
        return Task.Factory.StartNew<TriangleType>(() => WhatShapeIsThis(a, b, c));
    }

    public System.Threading.Tasks.Task<string> ReverseWordsAsync(string s)
    {
        return Task.Factory.StartNew<string>(() => ReverseWords(s));
    }

    public Guid WhatIsYourToken()
    {
        Guid g = new Guid("3ebe3892-4c90-4650-a001-bd9d0cd9b46f");
        return g;
    }

    public System.Threading.Tasks.Task<Guid> WhatIsYourTokenAsync()
    {
        return Task.Factory.StartNew<Guid>(() => WhatIsYourToken());
    }

    void Fault<T>(T detail) where T : Exception
    {
      throw new FaultException<T>(detail, detail.Message);
    }
  }

}

