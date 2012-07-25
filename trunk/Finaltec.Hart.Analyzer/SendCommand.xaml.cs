using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Cinch;
using Finaltec.Hart.Analyzer.ViewModel;

namespace Finaltec.Hart.Analyzer.View
{
    /// <summary>
    /// Interaktionslogik für SendCommand.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("SendCommandViewModel", typeof(SendCommand))]
    public partial class SendCommand
    {
        private List<RadioButton> _typeRadioButtons;

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
        private void CommandGotFocus(object sender, System.Windows.RoutedEventArgs e)
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
            ((SendCommandViewModel) DataContext).RawDataCleared += SwitchFocus;
        }

        /// <summary>
        /// Switches the focus.
        /// </summary>
        private void SwitchFocus()
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ListViewItem dependencyObject = FindParent<ListViewItem>((DependencyObject)sender);
            dependencyObject.IsSelected = true;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ListViewItem dependencyObject = FindParent<ListViewItem>((DependencyObject)sender);
            dependencyObject.IsSelected = false;
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

        private void Window_KeyUp(object sender, KeyEventArgs e)
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

        private void ListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ListView listView = (ListView)sender;
            if (listView.Items.Count == 1)
            {
                TextBox textBox = GetVisualChild<TextBox>((DependencyObject) sender);
                Keyboard.Focus(textBox);
            }
        }

        private static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        } 

    }
}
