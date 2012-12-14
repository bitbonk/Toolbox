namespace Adornment.Framework
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class TemplatedAdorner : Adorner
    {
        private Control child;

        public TemplatedAdorner(UIElement adornedElement, ControlTemplate adornerTemplate)
            : base(adornedElement)
        {
            var control = new Control
            {
                IsTabStop = false,
                IsEnabled = false,
                Template = adornerTemplate
            };

            var adornedFrameworkElement = adornedElement as FrameworkElement;
            if (adornedFrameworkElement != null)
            {
                control.DataContext = adornedFrameworkElement.DataContext;
            }

            this.child = control;
            this.AddVisualChild(this.child);
        }

        public FrameworkElement ReferenceElement { get; set; }

        protected override int VisualChildrenCount
        {
            get { return this.child != null ? 1 : 0; }
        }

        public void ClearChild()
        {
            this.RemoveVisualChild(this.child);
            this.child = null;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            if (this.ReferenceElement == null)
            {
                return transform;
            }

            var group = new GeneralTransformGroup();
            group.Children.Add(transform);

            var t = this.TransformToDescendant(this.ReferenceElement);
            if (t != null)
            {
                group.Children.Add(t);
            }
            return group;
        }

        protected override Size ArrangeOverride(Size size)
        {
            var finalSize = base.ArrangeOverride(size);
            if (this.child != null)
            {
                this.child.Arrange(new Rect(new Point(), finalSize));
            }
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (this.child == null || index != 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return this.child;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (this.ReferenceElement != null && this.AdornedElement != null &&
                this.AdornedElement.IsMeasureValid &&
                this.ReferenceElement.DesiredSize != this.AdornedElement.DesiredSize)
            {
                this.ReferenceElement.InvalidateMeasure();
            }

            (this.child).Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            return (this.child).DesiredSize;
        }
    }
}