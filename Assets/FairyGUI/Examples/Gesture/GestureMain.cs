using UnityEngine;
using FairyGUI;

public class GestureMain : MonoBehaviour
{
	GComponent _mainView;
	Transform _ball;

	void Awake()
	{
		Application.targetFrameRate = 60;
		Stage.inst.onKeyDown.Add(OnKeyDown);

		UIPackage.AddPackage("UI/Gesture");
	}

	void Start()
	{
		_mainView = this.GetComponent<UIPanel>().ui;
		//空矩形图形组件(设置上下居中和左右居中关联于容器组件)
		GObject holder = _mainView.GetChild("holder");

		//获取Unity对象地球的transform
		_ball = GameObject.Find("Globe").transform;

		//滑动手势 TouchBegin->TouchMove->TouchEnd流程
		SwipeGesture gesture1 = new SwipeGesture(holder);
		gesture1.onMove.Add(OnSwipeMove);
		gesture1.onEnd.Add(OnSwipeEnd);

		//长按手势 once长按时间内是否只派发一次事件 ,interval间隔时间
		LongPressGesture gesture2 = new LongPressGesture(holder);
		gesture2.once = false;
		gesture2.onAction.Add(OnHold);

		//两指缩放手势 
		PinchGesture gesture3 = new PinchGesture(holder);
		gesture3.onAction.Add(OnPinch);

		//两指旋转的手势
		RotationGesture gesture4 = new RotationGesture(holder);
		gesture4.onAction.Add(OnRotate);
	}

	/// <summary>
	/// 移动中触发的事件
	/// </summary>
	/// <param name="context"></param>
	void OnSwipeMove(EventContext context)
	{
		//事件的分发者
		SwipeGesture gesture = (SwipeGesture)context.sender;
		Vector3 v = new Vector3();
		//X>Y表示的是向左或向右滑动
		if (Mathf.Abs(gesture.delta.x) > Mathf.Abs(gesture.delta.y))
		{
			//向左右滑动 就是地球绕Y轴旋转
			v.y = -Mathf.Round(gesture.delta.x);
			//消除手抖的影响 移动距离小于2则不响应
			if (Mathf.Abs(v.y) < 2) 
				return;
		}
		//X<Y表示的是向上或者向下滑动
		else
		{
			//向上下滑动 就是地球绕X轴旋转
			v.x = -Mathf.Round(gesture.delta.y);
			if (Mathf.Abs(v.x) < 2)
				return;
		}
		//Space.World设定旋转本地坐标还是世界坐标
		_ball.Rotate(v, Space.World);
	}

	/// <summary>
	/// 离开时做一个惯性动画
	/// </summary>
	/// <param name="context"></param>
	void OnSwipeEnd(EventContext context)
	{
		SwipeGesture gesture = (SwipeGesture)context.sender;
		Vector3 v = new Vector3();
		if (Mathf.Abs(gesture.velocity.x) > Mathf.Abs(gesture.velocity.y))
		{
			//手指离开时,左右旋转时的加速度
			v.y = -Mathf.Round(Mathf.Sign(gesture.velocity.x) * Mathf.Sqrt(Mathf.Abs(gesture.velocity.x)));
			if (Mathf.Abs(v.y) < 2)
				return;
		}
		else
		{
			v.x = -Mathf.Round(Mathf.Sign(gesture.velocity.y) * Mathf.Sqrt(Mathf.Abs(gesture.velocity.y)));
			if (Mathf.Abs(v.x) < 2)
				return;
		}
		//做一个TO数值过度从V->0经过0.3秒  ,设置目标对象是地球, 每帧调用 lambda表达式传入一个GTweener对象
		//差值旋转
		GTween.To(v, Vector3.zero, 0.3f).SetTarget(_ball).OnUpdate(
			(GTweener tweener) =>
			{
				_ball.Rotate(tweener.deltaValue.vec3, Space.World);
			});
	}

	/// <summary>
	/// 长按震动效果
	/// </summary>
	/// <param name="context"></param>
	void OnHold(EventContext context)
	{
		//震动的坐标为本地坐标,震动强度,震动时间
		GTween.Shake(_ball.transform.localPosition, 0.05f, 0.5f).SetTarget(_ball).OnUpdate(
			(GTweener tweener) =>
			{
				//震动XY轴
				_ball.transform.localPosition = new Vector3(tweener.value.x, tweener.value.y, _ball.transform.localPosition.z);
			});
	}

	/// <summary>
	/// 缩放事件
	/// </summary>
	/// <param name="context"></param>
	void OnPinch(EventContext context)
	{
		//停止当前变化
		GTween.Kill(_ball);

		PinchGesture gesture = (PinchGesture)context.sender;
		//球的X轴缩放系数+ 手指改变量 限制在0.3-2之间, 改变球的XYZ缩放系数
		float newValue = Mathf.Clamp(_ball.localScale.x + gesture.delta, 0.3f, 2);
		_ball.localScale = new Vector3(newValue, newValue, newValue);
	}

	void OnRotate(EventContext context)
	{
		GTween.Kill(_ball);

		RotationGesture gesture = (RotationGesture)context.sender;
		_ball.Rotate(Vector3.forward, -gesture.delta, Space.World);
	}

	void OnKeyDown(EventContext context)
	{
		if (context.inputEvent.keyCode == KeyCode.Escape)
		{
			Application.Quit();
		}
	}
}