using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CTool.Extension
{

    public delegate void Action();

    public delegate void Action<T>(T t);

    public delegate void Action<T1, T2>(T1 t1, T2 t2);

    public delegate void Action<T1, T2, T3>(T1 t1, T2 t2, T3 t3);

    public delegate void Action<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);

    public delegate TResult Func<out TResult>();

    public delegate TResult Func<T1, TResult>(T1 t1);

    public delegate TResult Func<T1, T2, TResult>(T1 t1, T2 t2);

    public delegate TResult Func<T1, T2, T3, TResult>(T1 t1, T2 t2, T3 t3);

    public delegate TResult Func<T1, T2, T3, T4, TResult>(T1 t1, T2 t2, T3 t3, T4 t4);
}


