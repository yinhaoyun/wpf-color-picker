﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace WpfColorPicker
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorPicker), new PropertyMetadata(Colors.Red, OnColorChanged));

        public ColorPicker()
        {
            InitializeComponent();
            DataContext = new ColorPickerViewModel();
        }

        private static void OnColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var colorPicker = (ColorPicker)o;
        }
    }
}
