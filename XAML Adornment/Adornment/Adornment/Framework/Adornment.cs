namespace Adornment.Framework
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Threading;

    public static class Adornment
    {
        /// <summary>
        ///   Identifies the <c>IsVisible</c> attached property. This property indicates whether the adornment is visible.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.RegisterAttached("IsVisible",
                                                typeof (bool),
                                                typeof (Adornment),
                                                new PropertyMetadata(false, OnIsVisibleChanged));

        /// <summary>
        ///   Identifies the <c>Template</c> attached property. This property indicates template to be used for adornment.
        /// </summary>
        public static readonly DependencyProperty TemplateProperty =
            DependencyProperty.RegisterAttached("Template",
                                                typeof (ControlTemplate),
                                                typeof (Adornment),
                                                new PropertyMetadata(null, OnTemplateChanged));

        private static readonly DependencyProperty AdornerProperty =
            DependencyProperty.RegisterAttached("Adorner",
                                                typeof (TemplatedAdorner),
                                                typeof (Adornment),
                                                new FrameworkPropertyMetadata(null,
                                                                              FrameworkPropertyMetadataOptions.
                                                                                  NotDataBindable));

        /// <summary>
        ///   Gets the value of the <c>IsVisible</c> attached property for a specified <see cref="UIElement" /> .
        /// </summary>
        /// <param name="element"> The element from which the property value is read. </param>
        /// <returns> The <c>IsVisible</c> property value for the element. </returns>
        public static bool GetIsVisible(UIElement element)
        {
            return (bool)element.GetValue(IsVisibleProperty);
        }

        /// <summary>
        ///   Gets the value of the <c>Template</c> attached property for a specified <see cref="UIElement" /> .
        /// </summary>
        /// <param name="element"> The element from which the property value is read. </param>
        /// <returns> The <c>Template</c> property value for the element. </returns>
        public static ControlTemplate GetTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(TemplateProperty);
        }

        /// <summary>
        ///   Sets the value of the <c>IsVisible</c> attached property to a specified <see cref="UIElement" /> .
        /// </summary>
        /// <param name="element"> The element to which the attached property is written. </param>
        /// <param name="value"> The <c>IsVisible</c> property value to set. </param>
        public static void SetIsVisible(UIElement element, bool value)
        {
            element.SetValue(IsVisibleProperty, value);
        }

        /// <summary>
        ///   Sets the value of the <c>Template</c> attached property to a specified <see cref="UIElement" /> .
        /// </summary>
        /// <param name="element"> The element to which the attached property is written. </param>
        /// <param name="value"> The <c>Template</c> property value to set. </param>
        public static void SetTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(TemplateProperty, value);
        }

        private static void OnIsVisibleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ShowAdorner(sender as UIElement, (bool)e.NewValue);
        }

        private static void OnTemplateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ShowAdorner(sender as UIElement, GetIsVisible(sender as UIElement));
        }

        private static void ShowAdorner(UIElement targetElement, bool show, bool retry = true)
        {
            if (targetElement == null)
            {
                return;
            }

            var adornerLayer = AdornerLayer.GetAdornerLayer(targetElement);
            if (adornerLayer == null)
            {
                if (retry)
                {
                    targetElement.Dispatcher.BeginInvoke(
                        DispatcherPriority.Loaded,
                        new Action(() => ShowAdorner(targetElement, show, false)));
                }

                return;
            }

            var adorner = targetElement.ReadLocalValue(AdornerProperty) as TemplatedAdorner;
            if (show && adorner == null)
            {
                var validationTemplate = GetTemplate(targetElement);
                if (validationTemplate != null)
                {
                    adorner = new TemplatedAdorner(targetElement, validationTemplate);
                    adornerLayer.Add(adorner);
                    targetElement.SetValue(AdornerProperty, adorner);
                }
            }
            else if (!show && adorner != null)
            {
                adorner.ClearChild();
                adornerLayer.Remove(adorner);
                targetElement.ClearValue(AdornerProperty);
            }
        }
    }
}