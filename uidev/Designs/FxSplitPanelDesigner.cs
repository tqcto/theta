﻿using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace uidev.Designs
{
    public class FxSplitPanelsDesigner : ParentControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            var fxPanel1 = ((Controls.FxSplitPanels)this.Control).fxPanel1;
            var fxPanel2 = ((Controls.FxSplitPanels)this.Control).fxPanel2;

            this.EnableDesignMode(fxPanel1, "fxPanel1");
            this.EnableDesignMode(fxPanel2, "fxPanel2");
        }

        public override bool CanParent(Control control)
        {
            return false;
        }
        protected override void OnDragOver(DragEventArgs de)
        {
            de.Effect = DragDropEffects.None;
        }
        protected override IComponent[] CreateToolCore(ToolboxItem tool, int x,
            int y, int width, int height, bool hasLocation, bool hasSize)
        {
            return null;
        }
    }

    public class PanelDesigner : ParentControlDesigner
    {

        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                selectionRules &= ~SelectionRules.AllSizeable;
                selectionRules &= SelectionRules.Locked;
                return selectionRules;
            }
        }
        protected override void PostFilterAttributes(IDictionary attributes)
        {
            base.PostFilterAttributes(attributes);
            attributes[typeof(DockingAttribute)] =
                new DockingAttribute(DockingBehavior.Never);
        }
        protected override void PostFilterProperties(IDictionary properties)
        {
            base.PostFilterProperties(properties);
            var propertiesToRemove = new string[] {
            "Dock", "Anchor", "Size", "Location", "Width", "Height",
            "MinimumSize", "MaximumSize", "AutoSize", "AutoSizeMode",
            "Visible", "Enabled",
        };
            foreach (var item in propertiesToRemove)
            {
                if (properties.Contains(item))
                    properties[item] = TypeDescriptor.CreateProperty(this.Component.GetType(),
                        (PropertyDescriptor)properties[item],
                        new BrowsableAttribute(false));
            }
        }

    }
}