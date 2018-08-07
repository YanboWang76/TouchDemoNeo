using System.Collections.Generic;
public abstract class Pool<T> : IPool<T> {
	protected Stack<T> mCacheStack = new Stack<T> ();

	public int CurCount
	{
		get{ return mCacheStack.Count;}
	}

	protected IObjectFactory<T> mFactory ;

	protected int mMaxCount = 5;

	public virtual T Allocate()
	{
		return mCacheStack.Count == 0
			? mFactory.Create()
				: mCacheStack.Pop();
	}
	public abstract bool Recycle(T obj);

}
