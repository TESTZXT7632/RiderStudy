﻿using UnityEngine;
using FairyGUI;
using FairyGUI.Utils;

public class MailItem : GButton
{
	private GTextField _timeText;
	private Controller _readController;
	private Controller _fetchController;
	private Transition _trans;


	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);

		_timeText = this.GetChild("timeText").asTextField;
		_readController = this.GetController("IsRead");
		_fetchController = this.GetController("c1");
		_trans = this.GetTransition("t0");
	}

	public void setTime(string value)
	{
		_timeText.text = value;
	}

	public void setRead(bool value)
	{
		_readController.selectedIndex = value ? 1 : 0;
	}

	public void setFetched(bool value)
	{
		_fetchController.selectedIndex = value ? 1 : 0;
	}

	public void PlayEffect(float delay)
	{
		this.visible = false;
		_trans.Play(1, delay, null);
	}
}
