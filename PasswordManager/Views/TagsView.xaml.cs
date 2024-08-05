using PasswordManager.Models;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager.Views
{
    /// <summary>
    /// Interaction logic for LabelsView.xaml
    /// </summary>
    public partial class TagsView : UserControl
    {
        public TagsView()
        {
            InitializeComponent();
        }

        private void Tags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = (TagsViewModel)this.DataContext;
            if (viewModel != null && Tags.SelectedItems != null)
            {
                List<string> tags = [];
                foreach (var tag in Tags.SelectedItems)
                {
                    tags.Add(tag.ToString());
                }

                viewModel.Filter = tags;
            }
        }
    }
}
