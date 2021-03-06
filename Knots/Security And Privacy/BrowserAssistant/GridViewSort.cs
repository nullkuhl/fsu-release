﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Documents;

namespace BrowserAssistant
{
    /// <summary>
    /// <c>GridView</c> sorting helper
    /// </summary>
    public class GridViewSort
    {
        #region Public attached properties

        /// <summary>
        /// Gets the command
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets the command
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for Command. This enables animation, styling, binding, etc
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command",
                typeof(ICommand),
                typeof(GridViewSort),
                new UIPropertyMetadata(
                    null,
                    (o, e) =>
                    {
                        ItemsControl listView = o as ItemsControl;
                        if (listView != null)
                        {
                            if (!GetAutoSort(listView)) // Don't change click handler if AutoSort enabled
                            {
                                if (e.OldValue != null && e.NewValue == null)
                                {
                                    listView.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                                if (e.OldValue == null && e.NewValue != null)
                                {
                                    listView.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                            }
                        }
                    }
                )
            );

        /// <summary>
        /// Checks if <see cref="AutoSortProperty"/> exists
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetAutoSort(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoSortProperty);
        }

        /// <summary>
        /// Sets the <see cref="AutoSortProperty"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetAutoSort(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoSortProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for AutoSort. This enables animation, styling, binding, etc
        /// </summary>
        public static readonly DependencyProperty AutoSortProperty =
            DependencyProperty.RegisterAttached(
                "AutoSort",
                typeof(bool),
                typeof(GridViewSort),
                new UIPropertyMetadata(
                    false,
                    (o, e) =>
                    {
                        ListView listView = o as ListView;
                        if (listView != null)
                        {
                            if (GetCommand(listView) == null) // Don't change click handler if a command is set
                            {
                                bool oldValue = (bool)e.OldValue;
                                bool newValue = (bool)e.NewValue;
                                if (oldValue && !newValue)
                                {
                                    listView.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                                if (!oldValue && newValue)
                                {
                                    listView.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(ColumnHeader_Click));
                                }
                            }
                        }
                    }
                )
            );

        /// <summary>
        /// Gets property name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetPropertyName(DependencyObject obj)
        {
            return (string)obj.GetValue(PropertyNameProperty);
        }

        /// <summary>
        /// Sets property name
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetPropertyName(DependencyObject obj, string value)
        {
            obj.SetValue(PropertyNameProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for PropertyName. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.RegisterAttached(
                "PropertyName",
                typeof(string),
                typeof(GridViewSort),
                new UIPropertyMetadata(null)
            );

        /// <summary>
        /// Gets ShowSortGlyph
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetShowSortGlyph(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowSortGlyphProperty);
        }

        /// <summary>
        /// Sets ShowSortGlyph
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetShowSortGlyph(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowSortGlyphProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for ShowSortGlyph. This enables animation, styling, binding, etc
        /// </summary>
        public static readonly DependencyProperty ShowSortGlyphProperty =
            DependencyProperty.RegisterAttached("ShowSortGlyph", typeof(bool), typeof(GridViewSort), new UIPropertyMetadata(true));

        /// <summary>
        /// Gets SortGlyphAscending
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ImageSource GetSortGlyphAscending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphAscendingProperty);
        }

        /// <summary>
        /// Sets SortGlyphAscending
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void SetSortGlyphAscending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphAscendingProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for SortGlyphAscending. This enables animation, styling, binding, etc
        /// </summary>
        public static readonly DependencyProperty SortGlyphAscendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphAscending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets SortGlyphDescending
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ImageSource GetSortGlyphDescending(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(SortGlyphDescendingProperty);
        }

        /// <summary>
        /// Sets SortGlyphDescending
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void SetSortGlyphDescending(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(SortGlyphDescendingProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for SortGlyphDescending. This enables animation, styling, binding, etc
        /// </summary>
        public static readonly DependencyProperty SortGlyphDescendingProperty =
            DependencyProperty.RegisterAttached("SortGlyphDescending", typeof(ImageSource), typeof(GridViewSort), new UIPropertyMetadata(null));

        #endregion

        #region attached properties

        static GridViewColumnHeader GetSortedColumnHeader(DependencyObject obj)
        {
            return (GridViewColumnHeader)obj.GetValue(SortedColumnHeaderProperty);
        }

        static void SetSortedColumnHeader(DependencyObject obj, GridViewColumnHeader value)
        {
            obj.SetValue(SortedColumnHeaderProperty, value);
        }

        // Using a DependencyProperty as the backing store for SortedColumn.  This enables animation, styling, binding, etc...
        static readonly DependencyProperty SortedColumnHeaderProperty =
            DependencyProperty.RegisterAttached("SortedColumnHeader", typeof(GridViewColumnHeader), typeof(GridViewSort), new UIPropertyMetadata(null));

        #endregion

        #region Column header click event handler

        static void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null && headerClicked.Column != null)
            {
                string propertyName = GetPropertyName(headerClicked.Column);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    ListView listView = GetAncestor<ListView>(headerClicked);
                    if (listView != null)
                    {
                        ICommand command = GetCommand(listView);
                        if (command != null)
                        {
                            if (command.CanExecute(propertyName))
                            {
                                command.Execute(propertyName);
                            }
                        }
                        else if (GetAutoSort(listView))
                        {
                            ApplySort(listView.Items, propertyName, listView, headerClicked);
                        }
                    }
                }
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets ancestor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static T GetAncestor<T>(DependencyObject reference) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(reference);
            while (!(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return (T)parent;
        }

        /// <summary>
        /// Applies sort
        /// </summary>
        /// <param name="view"></param>
        /// <param name="propertyName"></param>
        /// <param name="listView"></param>
        /// <param name="sortedColumnHeader"></param>
        public static void ApplySort(ICollectionView view, string propertyName, ListView listView, GridViewColumnHeader sortedColumnHeader)
        {
            ListSortDirection direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count > 0)
            {
                SortDescription currentSort = view.SortDescriptions[0];
                if (currentSort.PropertyName == propertyName)
                {
                    direction = currentSort.Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                }
                view.SortDescriptions.Clear();

                GridViewColumnHeader currentSortedColumnHeader = GetSortedColumnHeader(listView);
                if (currentSortedColumnHeader != null)
                {
                    RemoveSortGlyph(currentSortedColumnHeader);
                }
            }
            if (!string.IsNullOrEmpty(propertyName))
            {
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
                if (GetShowSortGlyph(listView))
                    AddSortGlyph(
                        sortedColumnHeader,
                        direction,
                        direction == ListSortDirection.Ascending ? GetSortGlyphAscending(listView) : GetSortGlyphDescending(listView));
                SetSortedColumnHeader(listView, sortedColumnHeader);
            }
        }

        static void AddSortGlyph(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            adornerLayer.Add(
                new SortGlyphAdorner(
                    columnHeader,
                    direction,
                    sortGlyph
                    ));
        }

        static void RemoveSortGlyph(GridViewColumnHeader columnHeader)
        {
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(columnHeader);
            Adorner[] adorners = adornerLayer.GetAdorners(columnHeader);
            if (adorners != null)
            {
                foreach (Adorner adorner in adorners)
                {
                    if (adorner is SortGlyphAdorner)
                        adornerLayer.Remove(adorner);
                }
            }
        }

        #endregion

        #region SortGlyphAdorner nested class

        /// <summary>
        /// SortGlyph adorner
        /// </summary>
        class SortGlyphAdorner : Adorner
        {
            GridViewColumnHeader _columnHeader;
            ListSortDirection _direction;
            ImageSource _sortGlyph;

            /// <summary>
            /// <see cref="SortGlyphAdorner"/> constructor
            /// </summary>
            /// <param name="columnHeader"></param>
            /// <param name="direction"></param>
            /// <param name="sortGlyph"></param>
            public SortGlyphAdorner(GridViewColumnHeader columnHeader, ListSortDirection direction, ImageSource sortGlyph)
                : base(columnHeader)
            {
                _columnHeader = columnHeader;
                _direction = direction;
                _sortGlyph = sortGlyph;
            }

            Geometry GetDefaultGlyph()
            {
                double x1 = _columnHeader.ActualWidth - 13;
                double x2 = x1 + 10;
                double x3 = x1 + 5;
                double y1 = _columnHeader.ActualHeight / 2 - 3;
                double y2 = y1 + 5;

                if (_direction == ListSortDirection.Ascending)
                {
                    double tmp = y1;
                    y1 = y2;
                    y2 = tmp;
                }

                PathSegmentCollection pathSegmentCollection = new PathSegmentCollection
                                            	{
                                            		new LineSegment(new Point(x2, y1), true),
                                            		new LineSegment(new Point(x3, y2), true)
                                            	};

                PathFigure pathFigure = new PathFigure(
                    new Point(x1, y1),
                    pathSegmentCollection,
                    true);

                PathFigureCollection pathFigureCollection = new PathFigureCollection { pathFigure };

                PathGeometry pathGeometry = new PathGeometry(pathFigureCollection);
                return pathGeometry;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (_sortGlyph != null)
                {
                    double x = _columnHeader.ActualWidth - 13;
                    double y = _columnHeader.ActualHeight / 2 - 5;
                    Rect rect = new Rect(x, y, 10, 10);
                    drawingContext.DrawImage(_sortGlyph, rect);
                }
                else
                {
                    drawingContext.DrawGeometry(Brushes.LightGray, new Pen(Brushes.Gray, 1.0), GetDefaultGlyph());
                }
            }
        }

        #endregion
    }
}