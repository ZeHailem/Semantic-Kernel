using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SemantickKerenelDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.ViewModel();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !e.KeyboardDevice.IsKeyDown(Key.LeftShift) && !e.KeyboardDevice.IsKeyDown(Key.RightShift))
            {
                var vm = DataContext as ViewModel.ViewModel;
                if (vm?.Kernel != null && !vm.UserInput.Equals(string.Empty))
                {
                    vm.ChatHistory.AddUserMessage(vm.UserInput);
                    
                    vm.Response = await vm.ChatCompletionService?.GetChatMessageContentAsync(
        vm.ChatHistory,
        executionSettings: vm.OpenAIPromptExecutionSettings,
        kernel: vm.Kernel);

                }
                else
                {
                    MessageBox.Show("Please fill all required fields and select a provider before sending input.");
                }
                e.Handled = true;
            }
        }
    }
}