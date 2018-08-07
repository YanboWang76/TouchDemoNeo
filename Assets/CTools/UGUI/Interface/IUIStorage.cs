public interface IUIStorage  
{
	UIWindow CurWindow{ get;}
	//隐藏当前UI，isShowlast表示是否显示上一层UI
	void UIPop (bool isShowlast = true);
	//显示新的UI，隐藏上一层UI
	void UIPush (UIWindow element);
}
