using System.Windows;
using Y.ASIS.App.Database;
using Y.ASIS.App.Models;
using Y.ASIS.Common.ExtensionMethod;
using Y.ASIS.Common.MVVMFoundation;

namespace Y.ASIS.App.Windows
{
    /// <summary>
    /// UpdateTitleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateTitleWindow : PopupWindow
    {
        public Title TitleObject
        {
            get { return (Title)GetValue(TitleObjectProperty); }
            set { SetValue(TitleObjectProperty, value); }
        }

        public static readonly DependencyProperty TitleObjectProperty =
            DependencyProperty.Register("TitleObject", typeof(Title), typeof(UpdateTitleWindow), new PropertyMetadata(default));

        public RelayCommand UpdateTitleCommand { get; set; }

        public UpdateTitleWindow(Title title)
        {
            TitleObject = title;
            UpdateTitleCommand = new RelayCommand(UpdateTitle, CanUpdateTitle);
            InitializeComponent();
            DataContext = this;
        }

        private void UpdateTitle()
        {
            AppDatabase.Instance.UpdateTitle(TitleObject);
            App.Current.Resources[TitleObject.ResourceKey] = TitleObject.Content;
            if (Owner != null)
            {
                DialogResult = true;
            }
            Close();
        }

        private bool CanUpdateTitle(object _)
        {
            return !TitleObject.Content.IsNullOrEmptyOrWhiteSpace()
                && TitleObject.Content.Length <= 20;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
