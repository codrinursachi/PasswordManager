using PasswordManager.CustomControls;
using PasswordManager.Interfaces;
using PasswordManager.Models;
using PasswordManager.Services;
using PasswordManager.ViewModels;
using PasswordManager.Views.Dialogs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(
            MainViewModel mainViewModel,
            IAutoLockerService autoLockerService,
            IUserControlProviderService userControlProviderService,
            IDialogOverlayService dialogOverlayService,
            INavigationService navigationService,
            IRefreshService refreshService)
        {
            InitializeComponent();
            MouseMove += autoLockerService.OnActivity;
            KeyDown += autoLockerService.OnActivity;
            DataContext = mainViewModel;
            var dialog = userControlProviderService.ProvideUserControl<DialogOverlay>();
            dialogOverlay.Content = dialog;
            dialogOverlayService.MainViewOverlay=(DialogOverlay)dialog;
            navigationService.CurrentView = childView;
            navigationService.NavigateTo<AllPasswordsViewModel>();

            importFormat.ToolTip = """
                                      [
                                        {
                                            "Username": "import1",
                                            "Password": "import1",
                                            "Url": "import1",
                                            "ExpirationDate": "2024-11-27T00:00:00+02:00",
                                            "CategoryPath": "Category\\Subcategory",
                                            "Tags": "tag1 tag2 tag3",
                                            "Favorite": false,
                                            "Notes": "My notes"
                                        }
                                      ]
                                      """;
        }

        private void CategoriesSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var viewModel = (MainViewModel)DataContext;
            viewModel.Filter = (CategoryNodeModel)e.NewValue;
        }

        private void Categories_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((MainViewModel)DataContext).navigationService.CurrentViewModel is not CategoryViewModel)
            {
                categoryRadio.IsChecked = true;
                ((MainViewModel)DataContext).ShowCategoryViewCommand.Execute(null);
                ((TreeViewItem)Categories.ItemContainerGenerator.ContainerFromIndex(0)).IsExpanded = true;
                ((TreeViewItem)Categories.ItemContainerGenerator.ContainerFromIndex(0)).IsSelected = true;
            }
        }
    }
}