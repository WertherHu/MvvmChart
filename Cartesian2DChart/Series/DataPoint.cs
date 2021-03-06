﻿using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MvvmCharting
{
    /// <summary>
    /// Represents the scatter dot on the the series Chart.
    /// </summary>
    public class DataPoint: Control
    {
        static DataPoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataPoint), new FrameworkPropertyMetadata(typeof(DataPoint)));
        }

        public DataPoint()
        {
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.SizeChanged += ItemPoint_SizeChanged;
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = Shape.FillProperty.AddOwner(typeof(DataPoint), new PropertyMetadata(Brushes.Blue));


        public Style PointStyle
        {
            get { return (Style)GetValue(PointStyleProperty); }
            set { SetValue(PointStyleProperty, value); }
        }
        public static readonly DependencyProperty PointStyleProperty =
            DependencyProperty.Register("PointStyle", typeof(Style), typeof(DataPoint), new PropertyMetadata(null));

        protected virtual Point GetOffsetForSizeChangedOverride(Size newSize)
        {
            // The center of EllipseGeometry is (0,0) be default, so it does not need any offset.
            return default(Point);

            // But if it is an Ellipse of other Shape, then there should be an offset.
            //return new Point(-newSize.Width / 2, -newSize.Height / 2);
        }

        private void ItemPoint_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Offset = GetOffsetForSizeChangedOverride(e.NewSize);
        }

        public Point Position
        {
            get { return (Point)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Point), typeof(DataPoint), new PropertyMetadata(PointHelper.EmptyPoint, OnPositionPropertyChanged));

        private static void OnPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
             ((DataPoint)d).OnPositionChanged((Point)e.NewValue);
        }
        private void OnPositionChanged(Point newValue)
        {
            UpdateActualPosition();
        }

        public Point Offset
        {
            get { return (Point)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }
        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register("Offset", typeof(Point), typeof(DataPoint), new PropertyMetadata(PointHelper.EmptyPoint, OnOffsetPropertyChanged));

        private static void OnOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DataPoint)d).OnOffsetChanged();
        }

        private void OnOffsetChanged()
        {
            UpdateActualPosition();
        }

        private void UpdateActualPosition()
        {
            if (this.Position.IsEmpty() || this.Offset.IsEmpty())
            {
                return;
            }


            var pt = new Point(this.Position.X + this.Offset.X, this.Position.Y + this.Offset.Y);  
              
            var translateTransform = this.RenderTransform as TranslateTransform;
            if (translateTransform == null)
            {
                TranslateTransform a = new TranslateTransform(pt.X, pt.Y);
                this.RenderTransform = a;
            }
            else
            {
                translateTransform.Y = pt.Y;
                translateTransform.X = pt.X;
            }


        }
    }

 
}