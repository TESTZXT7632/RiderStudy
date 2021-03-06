﻿using System.Collections.Generic;
using FairyGUI.Utils;
using UnityEngine;

namespace FairyGUI
{
	/// <summary>
	/// GRichTextField class.
	/// </summary>
	public class GRichTextField : GTextField
	{
		/// <summary>
		/// 
		/// </summary>
		public RichTextField richTextField { get; private set; }

		public GRichTextField()
			: base()
		{
		}

		protected override void CreateDisplayObject()
		{
			richTextField = new RichTextField();
			richTextField.gOwner = this;
			displayObject = richTextField;

			_textField = richTextField.textField;
		}

		protected override void SetTextFieldText()
		{
			string str = _text;
			if (_templateVars != null)
				str = ParseTemplate(str);

			if (_ubbEnabled)
				richTextField.htmlText = UBBParser.inst.Parse(str);
			else
				richTextField.htmlText = str;
		}

		protected override void GetTextFieldText()
		{
			_text = richTextField.text;
		}

		/// <summary>
		/// 
		/// </summary>
		public Dictionary<uint, Emoji> emojies
		{
			get { return richTextField.emojies; }
			set { richTextField.emojies = value; }
		}
	}
}
