using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Cinch;
using Finaltec.Hart.Analyzer.ViewModel;
using Finaltec.Hart.Analyzer.ViewModel.DataModels;
using InputType = Finaltec.Hart.Analyzer.ViewModel.DataModels.InputType;

namespace Finaltec.Hart.Analyzer.View
{
    /// <summary>
    /// Interaktionslogik für SendCommand.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("SendCommandViewModel", typeof(SendCommand))]
    public partial class SendCommand
    {
        private readonly List<RadioButton> _typeRadioButtons;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendCommand"/> class.
        /// </summary>
        public SendCommand()
        {
            InitializeComponent();
            _typeRadioButtons = new List<RadioButton> { rbByte, rbInt16, rbInt24, rbInt32, rbFloat, rbString };
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// Commands the got focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void CommandGotFocus(object sender, RoutedEventArgs e)
        {
            tbCommand.SelectAll();
        }

        /// <summary>
        /// Window is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void WindowLoaded(object sender, EventArgs e)
        {
            ((SendCommandViewModel)DataContext).RawDataCleared += SwitchFocus;
            DataObject.AddPastingHandler(lvDataList, OnPaste);
        }

        private void WindowUnloaded(object sender, RoutedEventArgs e)
        {
            DataObject.RemovePastingHandler(lvDataList, OnPaste);
        }

        /// <summary>
        /// Switches the focus.
        /// </summary>
        private void SwitchFocus()
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            ListViewItem dependencyObject = FindParent<ListViewItem>((DependencyObject)sender);
            dependencyObject.IsSelected = true;
        }

        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            ListViewItem dependencyObject = FindParent<ListViewItem>((DependencyObject)sender);
            dependencyObject.IsSelected = false;
        }

        private void WindowKeyUp(object sender, KeyEventArgs e)
        {
            for (int i = 0; i < _typeRadioButtons.Count; i++)
            {
                if(_typeRadioButtons[i].IsChecked.HasValue && _typeRadioButtons[i].IsChecked.Value)
                {
                    if (e.Key == Key.Down)
                    {
                        if (i + 1 < _typeRadioButtons.Count)
                        {
                            _typeRadioButtons[i + 1].IsChecked = true;
                            e.Handled = true;
                            return;
                        }
                    }
                    else if (e.Key == Key.Up)
                    {
                        if (i > 0)
                        {
                            _typeRadioButtons[i -1].IsChecked = true;
                            e.Handled = true;
                            return;
                        }
                    }
                }
            }
        }

        private void ListViewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.Items.Count == 1)
            {
                TextBox textBox = GetVisualChild<TextBox>((DependencyObject) sender);
                Keyboard.Focus(textBox);
            }
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject dependencyObject = VisualTreeHelper.GetParent(child);
            if (dependencyObject == null)
                return null;

            T parent = dependencyObject as T;
            if (parent != null)
                return parent;

            return FindParent<T>(dependencyObject);
        }

        private static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);
                
                if (child != null)
                    break;
            }

            return child;
        }
        
        /// <summary>
        /// This handle is binded by <see cref="WindowLoaded"/> and unbind by <see cref="WindowUnloaded"/>. 
        /// </summary>
        /// <param name="sender">The sender object who gets the paste event.</param>
        /// <param name="e">Pasting event args.</param>
        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            bool isText = e.SourceDataObject.GetDataPresent(DataFormats.Text, true);
            if (!isText)
                return;

            string value = e.SourceDataObject.GetData(DataFormats.Text) as string;
            if (string.IsNullOrEmpty(value))
                return;

            string[] dataParts = value.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (dataParts.Length > 1)
            {
                foreach (string dataPart in dataParts)
                {
                    string data = dataPart.Replace("0x", string.Empty);
                    InputDataType dataType = GetDataType(ref data);

                    if (!data.StartsWith("0x"))
                        data = data.Insert(0, "0x");

                    SendCommandViewModel viewModel = (SendCommandViewModel)DataContext;
                    Input input = viewModel.RawValues.FirstOrDefault(item => string.IsNullOrEmpty(item.RawValue));
                    if (input != null)
                    {
                        input.Type = dataType;
                        input.RawValue = data;
                    }
                }

                //We need to clear the clipboard at the moment because they try to override the pasted stuff..
                Clipboard.Clear();

                foreach (Input input in ((SendCommandViewModel)DataContext).RawValues)
                {
                    input.InvokePropertysChanged();
                }

                e.Handled = true;
                SwitchFocus();
            }
        }

        /// <summary>
        /// This method is used to find a simple datatype for the expected data input. <br />
        /// Also the data will expended to a corret count of characters.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private InputDataType GetDataType(ref string data)
        {
            if (data.Length < 2)
                data = data.PadLeft(2, '0');

            if(data.Length > 2)
            {
                if (data.Length < 4)
                    data = data.PadLeft(4, '0');

                if (data.Length > 4)
                {
                    if (data.Length < 6)
                        data = data.PadLeft(6, '0');

                    if (data.Length > 6)
                    {
                        if (data.Length < 8)
                            data = data.PadLeft(8, '0');

                        return new InputDataType(InputType.UInt);
                    }

                    return new InputDataType(InputType.UInt24);
                }

                return new InputDataType(InputType.UShort);
            }

            return new InputDataType(InputType.Byte);
        }
    }
}
