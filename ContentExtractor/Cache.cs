using System;
using System.Collections.Generic;
using System.Text;
using MetaTech.Library;

namespace ContentExtractor.Core
{
  //public class CachedConverter<TInput, TOutput>
  //{
  //  public Converter<TInput, TOutput> Operation;
  //  public TOutput DefaultValue = default(TOutput);

  //  public CachedConverter() { }
  //  public CachedConverter(Converter<TInput, TOutput> operation)
  //  {
  //    this.Operation = operation;
  //  }

  //  public TOutput GetValue(TInput arg)
  //  {
  //    if (Operation != null)
  //    {
  //      if (!cacheInited || !Equality(arg, cachedArg))
  //      {
  //        TOutput result = Operation(arg);
  //        cacheInited = true;
  //        cachedArg = ArgCaching(arg);
  //        cachedVal = result;
  //      }
  //      return cachedVal;

  //    }
  //    return DefaultValue;
  //  }

  //  /// <summary>
  //  /// Правило сравнения двух элементов типа T на эквивалентность
  //  /// </summary>
  //  public IsEqual<TInput> Equality = CachedSynchronization<TInput>.DefaultComparsion;
  //  /// <summary>
  //  /// Правило кэширования для типа T (иногда надо делать Copy)
  //  /// </summary>
  //  public Converter<TInput, TInput> ArgCaching = CachedSynchronization<TInput>.DefaultCaching;
  //  ///// <summary>
  //  ///// Правило кэширования для типа T (иногда надо делать Copy)
  //  ///// </summary>
  //  //public Converter<TOutput, TOutput> ValCaching = CachedSynchronization<TOutput>.DefaultCaching;


  //  private bool cacheInited = false;
  //  private TInput cachedArg;
  //  private TOutput cachedVal;
  //}

  ///// <summary>
  ///// Оболочка, позволяющая осуществлять кэшированую синхронизацию.
  ///// Синхронизация будет происходить только если значение аргумента изменилось
  ///// </summary>
  ///// <typeparam name="T"></typeparam>
  //public class CachedSynchronization<T>
  //{
  //  public CachedSynchronization()
  //  {
  //  }
  //  public CachedSynchronization(Action<T> action)
  //    : this()
  //  {
  //    this.Action = action;
  //  }

  //  /// <summary>
  //  /// Операция синхронизации
  //  /// </summary>
  //  public Action<T> Action;

  //  /// <summary>
  //  /// Правило сравнения двух элементов типа T на эквивалентность
  //  /// </summary>
  //  public IsEqual<T> Equality = CachedSynchronization<T>.DefaultComparsion;
  //  /// <summary>
  //  /// Правило кэширования для типа T (иногда надо делать Copy)
  //  /// </summary>
  //  public Converter<T, T> Caching = CachedSynchronization<T>.DefaultCaching;

  //  public static bool DefaultComparsion(T val1, T val2)
  //  {
  //    return object.Equals(val1, val2);
  //    //return object.ReferenceEquals(val1, val2) || (val1 != null && val1.Equals(val2));
  //  }

  //  public static T DefaultCaching(T arg)
  //  {
  //    if (arg is ICloneable)
  //      return (T)((ICloneable)arg).Clone();
  //    else
  //      return arg;
  //  }

  //  /// <summary>
  //  /// Выполняет синхронизацию, только если значение аргумента изменилось с прошлого раза
  //  /// </summary>
  //  /// <param name="arg"></param>
  //  public void Synchronize(T arg)
  //  {
  //    if (Action != null)
  //    {
  //      if (!cacheInited || !Equality(arg, cache))
  //      {
  //        Action(arg);
  //        cache = Caching(arg);
  //        //cache = arg;
  //        cacheInited = true;
  //      }
  //    }
  //  }
  //  private bool cacheInited = false;
  //  private T cache;
  //}
}
