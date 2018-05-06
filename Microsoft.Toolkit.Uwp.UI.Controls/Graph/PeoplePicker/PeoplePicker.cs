// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.ObjectModel;
using Microsoft.Graph;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// The PeoplePicker Control is a simple control that allows for selection of one or more users from an organizational AD.
    /// </summary>
    public partial class PeoplePicker : Control
    {
        private TextBox _searchBox;
        private ProgressRing _loading;
        private ListBox _searchResultListBox;
        private TextBlock _selectionsCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeoplePicker"/> class.
        /// </summary>
        public PeoplePicker()
        {
            DefaultStyleKey = typeof(PeoplePicker);
        }

        /// <summary>
        /// Called when applying the control template.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _searchBox = GetTemplateChild("SearchBox") as TextBox;
            _loading = GetTemplateChild("Loading") as ProgressRing;
            _searchResultListBox = GetTemplateChild("SearchResultListBox") as ListBox;
            _selectionsCounter = GetTemplateChild("SelectionsCounter") as TextBlock;

            SearchResultList = new ObservableCollection<Person>();
            Selections = Selections ?? new ObservableCollection<Person>();
            _selectionsCounter.Text = $"{Selections.Count} selected";
            if (!AllowMultiple)
            {
                _selectionsCounter.Visibility = Visibility.Collapsed;
            }

            _searchBox.TextChanged += SearchBox_OnTextChanged;
            _searchResultListBox.SelectionChanged += SearchResultListBox_OnSelectionChanged;
        }

        private static void GraphAccessTokenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PeoplePicker;
            control?.SignInCurrentUserAsync();
        }

        private async void SignInCurrentUserAsync()
        {
            GraphClient = Common.GetAuthenticatedClient(GraphAccessToken);
            if (GraphClient != null)
            {
                var me = await GraphClient.Me.Request().GetAsync();
            }
        }
    }
}