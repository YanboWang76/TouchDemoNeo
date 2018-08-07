using CTool.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomObjectFactory<T> :IObjectFactory<T> {

	protected Func<T> mFactoryMethod;

	public CustomObjectFactory(Func<T> factoryMethod)
	{
		mFactoryMethod = factoryMethod;
	}

	#region IObjectFactory implementation
	public T Create ()
	{
		return mFactoryMethod ();
	}
	#endregion
}
