using System;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

/// <summary>
/// 继承Window类
/// </summary>
public class Window2 : Window
{
	/// <summary>
	/// 动态构建窗口需要调用AddUISource
	/// </summary>
	public Window2()
	{
	}

	/// <summary>
	/// 重写OnInit, 获取contentPane, 并设置位置居中
	/// </summary>
	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject("Basics", "WindowB").asCom;
		this.Center();
	}

	/// <summary>
	/// 窗口显示时的动画效果
	/// </summary>
	protected override void DoShowAnimation()
	{
		//窗口显示动画播放完成后,调用OnShown
		this.SetScale(0.1f, 0.1f);
		this.SetPivot(0.5f, 0.5f);
		this.TweenScale(new Vector2(1, 1), 0.3f).OnComplete(this.OnShown);
	}

	/// <summary>
	/// 窗口隐藏时的动画效果
	/// </summary>
	protected override void DoHideAnimation()
	{
		//不是调用OnHide , 动画播放完成时候应该调用HideImmediately
		this.TweenScale(new Vector2(0.1f, 0.1f), 0.3f).OnComplete(this.HideImmediately);
	}

	/// <summary>
	/// 窗口显示时候处理的业务逻辑
	/// </summary>
	protected override void OnShown()
	{
		contentPane.GetTransition("t1").Play();
	}

	/// <summary>
	/// 窗口隐藏时处理的业务逻辑
	/// </summary>
	protected override void OnHide()
	{
		contentPane.GetTransition("t1").Stop();
	}
}
