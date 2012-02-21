#region Using

using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class FileMenuWindow : Window
    {
        OpenStorageDialog openStorageDialog;

        public FileMenuWindow(Screen screen)
            : base(screen)
        {
            Padding = new Thickness(16);

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = stackPanel;

            var loadButton = new CustomButton(screen);
            loadButton.TextBlock.Text = "Load";
            loadButton.Click += new RoutedEventHandler(OnLoadButtonClick);
            stackPanel.Children.Add(loadButton);

            loadButton.Focus();
        }

        void OnLoadButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (openStorageDialog == null)
            {
                var showSelectorResult = StorageDevice.BeginShowSelector(null, null);
                showSelectorResult.AsyncWaitHandle.WaitOne();
                var storageDevice = StorageDevice.EndShowSelector(showSelectorResult);
                showSelectorResult.AsyncWaitHandle.Close();

                var openContainerResult = storageDevice.BeginOpenContainer("BlockData", null, null);
                openContainerResult.AsyncWaitHandle.WaitOne();
                var storageContainer = storageDevice.EndOpenContainer(openContainerResult);
                openContainerResult.AsyncWaitHandle.Close();

                openStorageDialog = new OpenStorageDialog(Screen, storageContainer);
            }
            openStorageDialog.Show();
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            base.OnKeyDown(ref context);

            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Escape))
            {
                Hide();
                Owner.Activate();
                context.Handled = true;
            }
        }
    }
}
