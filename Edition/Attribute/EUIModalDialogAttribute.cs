using System;

namespace Edition.Attribute
{
   // Not connected
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    internal class EModalDialogAttribute : System.Attribute
    {
        public string TitlDialog { get; set; }
        public string MessageDialog { get; set; }
        public string ButtonOkValueDialog { get; set; }
        public string ButtonCancelValueDialog { get; set; }
        public string IdButtonForEventShowDialog { get; set; }
        public string IdFormForSubmit { get; set; }
        public string JscodeOkCloseDialog { get; set; }

        public EModalDialogAttribute(string titlDialog, string messageDialog, string buttonOkValueDialog, string buttonCancelValueDialog, string idButtonForEventShowDialog, string idFormForSubmit, string jscodeOkCloseDialog)
        {
            TitlDialog = titlDialog;
            MessageDialog = messageDialog;
            ButtonOkValueDialog = buttonOkValueDialog;
            ButtonCancelValueDialog = buttonCancelValueDialog;
            IdButtonForEventShowDialog = idButtonForEventShowDialog;
            IdFormForSubmit = idFormForSubmit;
            JscodeOkCloseDialog = jscodeOkCloseDialog;
        }
    }
}