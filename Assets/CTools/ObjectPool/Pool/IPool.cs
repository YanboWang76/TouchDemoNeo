using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPool<T> {
	T Allocate();
	bool Recycle(T obj);

}
