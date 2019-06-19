using MvvmDialogs;
using System;

namespace MvvmWpfApp.ViewModels
{
    class AboutViewModel : ViewModelBase, IModalDialogViewModel
    {
        public bool? DialogResult { get { return false; } }

        public string Content
        {
            get
            {
                return "MvvmWpfApp" + Environment.NewLine +
                        "Created by user" + Environment.NewLine +
                        "Address" + Environment.NewLine +
                        "2019";
            }
        }

        public string VersionText
        {
            get
            {
                var version1 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                return "MvvmWpfApp v" + version1.ToString();
            }
        }
    }
}
