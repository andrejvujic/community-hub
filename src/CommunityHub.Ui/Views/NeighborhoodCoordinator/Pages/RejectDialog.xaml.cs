using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CommunityHub.Ui.Views.NeighborhoodCoordinator.Pages
{
    public partial class RejectDialog : Window
    {
        public string Comment { get; private set; }

        public RejectDialog(string ime, string prezime, string kvart)
        {
            InitializeComponent();
            MessageLabel.Text = $"Napišite razlog odbijanja zahteva korisnika {ime} {prezime} za pristup kvartu {kvart}:";
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            Comment = CommentTextBox.Text;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}