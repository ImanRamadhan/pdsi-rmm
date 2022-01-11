using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kairos.Library.ControlHelper
{
    public static class General
    {
        public static void SetChildControlReadOnly(Control root, bool value)
        {
            if (root.HasControls())
            {
                foreach (Control control in root.Controls)
                {
                    if (control is TextBox)
                        ((TextBox)control).ReadOnly = value;
                    else if (control is DropDownList)
                        ((DropDownList)control).Enabled = !value;
                    else if (control is CheckBoxList)
                        ((CheckBoxList)control).Enabled = !value;
                    else if (control is CheckBox)
                        ((CheckBox)control).Enabled = !value;
                    else if (control is Button)
                        ((Button)control).Enabled = !value;
                    else if (control is Image)
                        ((Image)control).Visible = !value;
                    else if (control is FileUpload)
                        ((FileUpload)control).Enabled = !value;
                    else if (control is GridView)
                    {
                        GridView gv = (GridView)control;
                        foreach (GridViewRow gvr in gv.Rows)
                        {
                            SetChildControlReadOnly(gvr, value);
                        }
                    }
                    SetChildControlReadOnly(control, value);
                }
            }
        }

        /// <summary>
        /// Get all children controls of the specified control
        /// </summary>
        /// <param name="parent">The specified control to be the parent control.</param>
        public static IEnumerable<Control> GetAllControls(Control parent)
        {
            var result = new List<Control>();
            foreach (Control control in parent.Controls)
            {
                result.Add(control);
                if (control.HasControls())
                {
                    result.AddRange(GetAllControls(control));
                }
            }
            return result;
        }

        /// <summary>
        /// Get control by ID
        /// </summary>
        /// <param name="container">The specified control to be the container control.</param>
        public static Control GetControlByID(Control container, string controlID)
        {
            var result = new List<Control>();
            foreach (Control control in container.Controls)
            {
                result.Add(control);
                if (control.HasControls())
                {
                    result.AddRange(GetAllControls(control));
                }
            }

            foreach (Control control in result)
            {
                if (control.ID == controlID)
                {
                    return control;
                }
            }

            return null;
        }

        /// <summary>
        /// Get all children controls of the specified control with a specific type
        /// </summary>
        /// <param name="parent">The specified control to be the parent control.</param>
        public static IEnumerable<T> GetAllControlsOfType<T>(Control parent) where T : Control
        {
            var result = new List<T>();
            foreach (Control control in parent.Controls)
            {
                if (control is T)
                {
                    result.Add((T)control);
                }
                
                if (control.HasControls())
                {
                    result.AddRange(GetAllControlsOfType<T>(control));
                }
            }
            return result;
        }

    }
}
