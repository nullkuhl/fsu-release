// Copyright (CPOL) 2011 RikTheVeggie - see http://www.codeproject.com/info/cpol10.aspx
// Tri-State Tree View http://www.codeproject.com/script/Articles/ViewDownloads.aspx?aid=202435

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FreemiumUtilities.TracksEraser
{
	// <summary>
	// A Tri-State TreeView designed for on-demand populating of the tree
	// </summary>
	// <remarks>
	// 'Mixed' nodes retain their checked state, meaning they can be checked or unchecked according to their current state
	// Tree can be navigated by keyboard (cursor keys & space)
	// No need to do anything special in calling code
	// </remarks>
	public class TriStateTreeView : TreeView
	{
		// <remarks>
		// CheckedState is an enum of all allowable nodes states
		// </remarks>

		#region CheckedState enum

		public enum CheckedState
		{
			UnInitialised = -1,
			UnChecked,
			Checked,
			Mixed
		};

		#endregion

		// <remarks>
		// IgnoreClickAction is used to ingore messages generated by setting the node.Checked flag in code
		// Do not set <c>e.Cancel = true</c> in <c>OnBeforeCheck</c> otherwise the Checked state will be lost
		// </remarks>
		// <remarks>

		// TriStateStyles is an enum of all allowable tree styles
		// All styles check children when parent is checked
		// Installer automatically checks parent if all children are checked, and unchecks parent if at least one child is unchecked
		// Standard never changes the checked status of a parent
		// </remarks>

		#region TriStateStyles enum

		public enum TriStateStyles
		{
			Standard = 0,
			Installer
		};

		#endregion

		int IgnoreClickAction;

		// Create a member for the tree style, and allow it to be set on the property sheer
		TriStateStyles TriStateStyle = TriStateStyles.Standard;

		// <summary>
		// Constructor.  Create and populate an image list
		// </summary>
		public TriStateTreeView()
		{
			StateImageList = new ImageList();

			// populate the image list, using images from the System.Windows.Forms.CheckBoxRenderer class
			for (int i = 0; i < 3; i++)
			{
				// Create a bitmap which holds the relevent check box style
				// see http://msdn.microsoft.com/en-us/library/ms404307.aspx and http://msdn.microsoft.com/en-us/library/system.windows.forms.checkboxrenderer.aspx

				var bmp = new Bitmap(16, 16);
				Graphics chkGraphics = Graphics.FromImage(bmp);
				switch (i)
				{
						// 0,1 - offset the checkbox slightly so it positions in the correct place
					case 0:
						CheckBoxRenderer.DrawCheckBox(chkGraphics, new Point(0, 1), CheckBoxState.UncheckedNormal);
						break;
					case 1:
						CheckBoxRenderer.DrawCheckBox(chkGraphics, new Point(0, 1), CheckBoxState.CheckedNormal);
						break;
					case 2:
						CheckBoxRenderer.DrawCheckBox(chkGraphics, new Point(0, 1), CheckBoxState.MixedNormal);
						break;
				}

				StateImageList.Images.Add(bmp);
			}
		}

		[Category("Tri-State Tree View")]
		[DisplayName("Style")]
		[Description("Style of the Tri-State Tree View")]
		public TriStateStyles TriStateStyleProperty
		{
			get { return TriStateStyle; }
			set { TriStateStyle = value; }
		}

		// <summary>
		// Called once before window displayed.  Disables default Checkbox functionality and ensures all nodes display an 'unchecked' image.
		// </summary>
		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			CheckBoxes = false; // Disable default CheckBox functionality if it's been enabled

			// Give every node an initial 'unchecked' image
			IgnoreClickAction++; // we're making changes to the tree, ignore any other change requests
			UpdateChildState(null, Nodes, (int) CheckedState.UnChecked, false, true);
			IgnoreClickAction--;
		}

		// <summary>
		// Called after a node is checked.  Forces all children to inherit current state, and notifies parents they may need to become 'mixed'
		// </summary>
		protected override void OnAfterCheck(TreeViewEventArgs e)
		{
			base.OnAfterCheck(e);

			if (IgnoreClickAction > 0)
			{
				return;
			}

			IgnoreClickAction++; // we're making changes to the tree, ignore any other change requests

			// the checked state has already been changed, we just need to update the state index

			// node is either ticked or unticked.  ignore mixed state, as the node is still only ticked or unticked regardless of state of children
			TreeNode tn = e.Node;
			tn.StateImageIndex = tn.Checked ? (int) CheckedState.Checked : (int) CheckedState.UnChecked;

			// force all children to inherit the same state as the current node
			UpdateChildState(tn, e.Node.Nodes, e.Node.StateImageIndex, e.Node.Checked, false);

			// populate state up the tree, possibly resulting in parents with mixed state
			UpdateParentState(e.Node.Parent);

			IgnoreClickAction--;
		}

		// <summary>
		// Called after a node is expanded.  Ensures any new nodes display an 'unchecked' image
		// </summary>
		protected override void OnAfterExpand(TreeViewEventArgs e)
		{
			// If any child node is new, give it the same check state as the current node
			// So if current node is ticked, child nodes will also be ticked
			base.OnAfterExpand(e);

			IgnoreClickAction++; // we're making changes to the tree, ignore any other change requests
			UpdateChildState(e.Node, e.Node.Nodes, e.Node.StateImageIndex, e.Node.Checked, true);
			IgnoreClickAction--;
		}

		// <summary>
		// Helper function to replace child state with that of the parent
		// </summary>
		protected void UpdateChildState(TreeNode tn, TreeNodeCollection Nodes, int StateImageIndex, bool Checked,
		                                bool ChangeUninitialisedNodesOnly)
		{
			foreach (TreeNode tnChild in Nodes)
			{
				if (!ChangeUninitialisedNodesOnly || tnChild.StateImageIndex == -1)
				{
					// Edited by Mykola
					if (tnChild.ForeColor == Color.DarkGray)
					{
						tnChild.Checked = false;
						if (tn != null && tn.StateImageIndex == (int) CheckedState.Checked)
						{
							tn.StateImageIndex = (int) CheckedState.Mixed;
						}
					}
					else
					{
						tnChild.StateImageIndex = StateImageIndex;
						tnChild.Checked = Checked; // override 'checked' state of child with that of parent
					}

					if (tnChild.Nodes.Count > 0)
					{
						UpdateChildState(tn, tnChild.Nodes, StateImageIndex, Checked, ChangeUninitialisedNodesOnly);
					}
				}
			}
		}

		// <summary>
		// Helper function to notify parent it may need to use 'mixed' state
		// </summary>
		protected void UpdateParentState(TreeNode tn)
		{
			// Node needs to check all of it's children to see if any of them are ticked or mixed
			if (tn == null)
				return;

			int OrigStateImageIndex = tn.StateImageIndex;

			int UnCheckedNodes = 0, CheckedNodes = 0, MixedNodes = 0;

			// The parent needs to know how many of it's children are Checked or Mixed
			foreach (TreeNode tnChild in tn.Nodes)
			{
				if (tnChild.StateImageIndex == (int) CheckedState.Checked)
					CheckedNodes++;
				else if (tnChild.StateImageIndex == (int) CheckedState.Mixed)
				{
					MixedNodes++;
					break;
				}
				else
					UnCheckedNodes++;
			}

			if (TriStateStyle == TriStateStyles.Installer)
			{
				// In Installer mode, if all child nodes are checked then parent is checked
				// If at least one child is unchecked, then parent is unchecked
				if (MixedNodes == 0)
				{
					if (UnCheckedNodes == 0)
					{
						// all children are checked, so parent must be checked
						tn.Checked = true;
					}
					else
					{
						// at least one child is unchecked, so parent must be unchecked
						tn.Checked = false;
					}
				}
			}

			// Determine the parent's new Image State
			if (MixedNodes > 0)
			{
				// at least one child is mixed, so parent must be mixed
				tn.StateImageIndex = (int) CheckedState.Mixed;
			}
			else if (CheckedNodes > 0 && UnCheckedNodes == 0)
			{
				// all children are checked
				if (tn.Checked)
					tn.StateImageIndex = (int) CheckedState.Checked;
				else
					tn.StateImageIndex = (int) CheckedState.Mixed;
			}
			else if (CheckedNodes > 0)
			{
				// some children are checked, the rest are unchecked
				tn.StateImageIndex = (int) CheckedState.Mixed;
			}
			else
			{
				// all children are unchecked

				//commented by Mykola

				//if (tn.Checked)
				//    tn.StateImageIndex = (int)CheckedState.Mixed;
				//else
				tn.StateImageIndex = (int) CheckedState.UnChecked;
			}

			if (OrigStateImageIndex != tn.StateImageIndex && tn.Parent != null)
			{
				// Parent's state has changed, notify the parent's parent
				UpdateParentState(tn.Parent);
			}
		}

		// <summary>
		// Called on keypress.  Used to change node state when Space key is pressed
		// Invokes OnAfterCheck to do the real work
		// </summary>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			// is the keypress a space?  If not, discard it
			if (e.KeyCode == Keys.Space)
			{
				// toggle the node's checked status.  This will then fire OnAfterCheck
				SelectedNode.Checked = !SelectedNode.Checked;
			}
		}

		// <summary>
		// Called when node is clicked by the mouse.  Does nothing unless the image was clicked
		// Invokes OnAfterCheck to do the real work
		// </summary>
		protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
		{
			base.OnNodeMouseClick(e);

			// is the click on the checkbox?  If not, discard it
			TreeViewHitTestInfo info = HitTest(e.X, e.Y);
			if (info == null || info.Location != TreeViewHitTestLocations.StateImage)
			{
				return;
			}

			// toggle the node's checked status.  This will then fire OnAfterCheck
			TreeNode tn = e.Node;
			tn.Checked = !tn.Checked;
		}
	}
}