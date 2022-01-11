using DevExpress.Web;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Kairos.Library.ControlHelper
{
    public static class ASPxGeneral
    {
        /// <summary>
        /// (ASPx Control Only) Disable all children control of the specified control.
        /// </summary>
        /// <param name="parent">The specified control to be the parent control.</param>
        public static void DisableAllControls(this Control parent)
        {
            var allCtr = General.GetAllControls(parent);
            foreach (var ctr in allCtr)
            {
                if (ctr is ASPxTextBox)
                {
                    (ctr as ASPxTextBox).Enabled = false;
                    (ctr as ASPxTextBox).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxTextBox).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxComboBox)
                {
                    (ctr as ASPxComboBox).Enabled = false;
                    (ctr as ASPxComboBox).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxComboBox).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxCheckBoxList)
                {
                    (ctr as ASPxCheckBoxList).Enabled = false;
                    (ctr as ASPxCheckBoxList).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxCheckBoxList).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxCheckBox)
                {
                    (ctr as ASPxCheckBox).Enabled = false;
                    (ctr as ASPxCheckBox).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxCheckBox).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxDateEdit)
                {
                    (ctr as ASPxDateEdit).Enabled = false;
                    (ctr as ASPxDateEdit).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxDateEdit).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxDropDownEdit)
                {
                    (ctr as ASPxDropDownEdit).Enabled = false;
                    (ctr as ASPxDropDownEdit).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxDropDownEdit).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxGridView)
                {
                    (ctr as ASPxGridView).Enabled = false;
                    (ctr as ASPxGridView).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxGridView).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxListBox)
                {
                    (ctr as ASPxListBox).Enabled = false;
                    (ctr as ASPxListBox).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxListBox).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxListEdit)
                {
                    (ctr as ASPxListEdit).Enabled = false;
                    (ctr as ASPxListEdit).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxListEdit).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxMemo)
                {
                    (ctr as ASPxMemo).Enabled = false;
                    (ctr as ASPxMemo).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxMemo).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxRadioButton)
                {
                    (ctr as ASPxRadioButton).Enabled = false;
                    (ctr as ASPxRadioButton).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxRadioButton).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxRadioButtonList)
                {
                    (ctr as ASPxRadioButtonList).Enabled = false;
                    (ctr as ASPxRadioButtonList).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxRadioButtonList).DisabledStyle.ForeColor = Color.Black;
                }
                else if (ctr is ASPxSpinEdit)
                {
                    (ctr as ASPxSpinEdit).Enabled = false;
                    (ctr as ASPxSpinEdit).DisabledStyle.BackColor = ColorTranslator.FromHtml("#EEEEEE");
                    (ctr as ASPxSpinEdit).DisabledStyle.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// (ASPx Control Only) Hide all children control of the specified control.
        /// </summary>
        /// <param name="parent">The specified control to be the parent control.</param>
        public static void HideAllControls(this Control parent)
        {
            var allCtr = General.GetAllControls(parent);
            foreach (var ctr in allCtr)
            {
                if (ctr is ASPxTextBox)
                {
                    (ctr as ASPxTextBox).Visible = false;
                }
                else if (ctr is ASPxLabel)
                {
                    (ctr as ASPxLabel).Visible = false;
                }
                else if (ctr is ASPxComboBox)
                {
                    (ctr as ASPxComboBox).Visible = false;
                }
                else if (ctr is ASPxCheckBoxList)
                {
                    (ctr as ASPxCheckBoxList).Visible = false;
                }
                else if (ctr is ASPxCheckBox)
                {
                    (ctr as ASPxCheckBox).Visible = false;
                }
                else if (ctr is ASPxDateEdit)
                {
                    (ctr as ASPxDateEdit).Visible = false;
                }
                else if (ctr is ASPxDropDownEdit)
                {
                    (ctr as ASPxDropDownEdit).Visible = false;
                }
                else if (ctr is ASPxGridView)
                {
                    (ctr as ASPxGridView).Visible = false;
                }
                else if (ctr is ASPxListBox)
                {
                    (ctr as ASPxListBox).Visible = false;
                }
                else if (ctr is ASPxListEdit)
                {
                    (ctr as ASPxListEdit).Visible = false;
                }
                else if (ctr is ASPxMemo)
                {
                    (ctr as ASPxMemo).Visible = false;
                }
                else if (ctr is ASPxRadioButton)
                {
                    (ctr as ASPxRadioButton).Visible = false;
                }
                else if (ctr is ASPxRadioButtonList)
                {
                    (ctr as ASPxRadioButtonList).Visible = false;
                }
                else if (ctr is ASPxSpinEdit)
                {
                    (ctr as ASPxSpinEdit).Visible = false;
                }
            }
        }
    }
}
