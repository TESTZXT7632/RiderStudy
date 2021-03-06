﻿using FairyGUI.Utils;

namespace FairyGUI
{
	public class ChangePageAction : ControllerAction
	{
		public string objectId;
		public string controllerName;
		public string targetPage;

		public ChangePageAction()
		{
		}

		protected override void Enter(Controller controller)
		{
			if (string.IsNullOrEmpty(controllerName))
				return;

			GComponent gcom;
			if (!string.IsNullOrEmpty(objectId))
				gcom = controller.parent.GetChildById(objectId) as GComponent;
			else
				gcom = controller.parent;
			if (gcom != null)
			{
				Controller cc = gcom.GetController(controllerName);
				if (cc != null && cc != controller && !cc.changing)
					cc.selectedPageId = targetPage;
			}
		}

		public override void Setup(ByteBuffer buffer)
		{
			base.Setup(buffer);

			objectId = buffer.ReadS();
			controllerName = buffer.ReadS();
			targetPage = buffer.ReadS();
		}
	}
}
