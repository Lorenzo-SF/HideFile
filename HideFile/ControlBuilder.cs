using System.Windows;
using System.Windows.Controls;

namespace HideFile
{
    public static class ControlBuilder
    {
        /// <summary>
        /// Funcion que genera un TextBox generico
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public static TextBox GetTextBox(string name, string text, int width, int height, Thickness thickness)
        {
            return new TextBox()
            {
                Name = name,
                Width = width,
                Height = height,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                TextWrapping = TextWrapping.Wrap,
                Margin = thickness,
                Text = text
            };
        }

        /// <summary>
        /// Funcion que genera un TextBlock generico
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public static TextBlock GetTextBlock(string name, string text, int width, int height, Thickness thickness)
        {
            return new TextBlock()
            {
                Name = name,
                Width = width,
                Height = height,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = thickness,
                Text = text
            };
        }

        /// <summary>
        /// Funcion que genera un Button generico.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="thickness"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static Button GetButton(string name, string text, int width, int height, Thickness thickness, string tag)
        {
            return new Button()
            {
                Name = name,
                Width = width,
                Height = height,
                Content = text,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = thickness,
                Tag = tag
            };
        }
    }
}