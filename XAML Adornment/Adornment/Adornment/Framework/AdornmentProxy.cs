namespace Adornment.Framework
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;

    [ContentProperty("Child")]
    public class AdornmentProxy : FrameworkElement
    {
        private UIElement child;
        private TemplatedAdorner templatedAdorner;

        public UIElement AdornedElement
        {
            get
            {
                var adorner = this.TemplatedAdorner;
                return adorner == null ? null : this.TemplatedAdorner.AdornedElement;
            }
        }

        [DefaultValue(null)]
        public virtual UIElement Child
        {
            get { return this.child; }

            set
            {
                var old = this.child;
                if (old != value)
                {
                    this.RemoveVisualChild(old);
                    this.RemoveLogicalChild(old);
                    this.child = value;
                    this.AddVisualChild(this.child);
                    this.AddLogicalChild(value);
                    this.InvalidateMeasure();
                }
            }
        }

        protected override IEnumerator LogicalChildren
        {
            get { return new[] { this.child }.GetEnumerator(); }
        }

        protected override int VisualChildrenCount
        {
            get { return (this.child == null) ? 0 : 1; }
        }

        private TemplatedAdorner TemplatedAdorner
        {
            get
            {
                if (this.templatedAdorner == null)
                {
                    var templateParent = this.TemplatedParent as FrameworkElement;
                    if (templateParent != null)
                    {
                        this.templatedAdorner = VisualTreeHelper.GetParent(templateParent) as TemplatedAdorner;
                        if (this.templatedAdorner != null && this.templatedAdorner.ReferenceElement == null)
                        {
                            this.templatedAdorner.ReferenceElement = this;
                        }
                    }
                }
                return this.templatedAdorner;
            }
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var c = this.Child;
            if (c != null)
            {
                c.Arrange(new Rect(arrangeBounds));
            }

            return arrangeBounds;
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
            if (this.TemplatedParent == null)
            {
                throw new InvalidOperationException();
            }

            if (this.AdornedElement == null)
            {
                return new Size(0, 0);
            }

            var desiredSize = this.AdornedElement.RenderSize;
            var c = this.Child;
            if (c != null)
            {
                c.Measure(desiredSize);
            }

            return desiredSize;
        }

        protected override void OnInitialized(EventArgs e)
        {
            if (this.TemplatedParent == null)
            {
                throw new InvalidOperationException();
            }

            base.OnInitialized(e);
        }
    }
}