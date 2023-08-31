// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.TriggerAction
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace System.Windows.Interactivity
{
  [DefaultTrigger(typeof (ButtonBase), typeof (EventTrigger), "Click")]
  [DefaultTrigger(typeof (UIElement), typeof (EventTrigger), "MouseLeftButtonDown")]
  public abstract class TriggerAction : DependencyObject, IAttachedObject
  {
    private bool isHosted;
    private DependencyObject associatedObject;
    private Type associatedObjectTypeConstraint;
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(nameof (IsEnabled), typeof (bool), typeof (TriggerAction), new PropertyMetadata((object) true));

    [DefaultValue(true)]
    public bool IsEnabled
    {
      get => (bool) this.GetValue(TriggerAction.IsEnabledProperty);
      set => this.SetValue(TriggerAction.IsEnabledProperty, (object) value);
    }

    protected DependencyObject AssociatedObject => this.associatedObject;

    protected virtual Type AssociatedObjectTypeConstraint => this.associatedObjectTypeConstraint;

    internal bool IsHosted
    {
      get => this.isHosted;
      set => this.isHosted = value;
    }

    internal TriggerAction(Type associatedObjectTypeConstraint) => this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;

    internal void CallInvoke(object parameter)
    {
      if (!this.IsEnabled)
        return;
      this.Invoke(parameter);
    }

    protected abstract void Invoke(object parameter);

    protected virtual void OnAttached()
    {
    }

    protected virtual void OnDetaching()
    {
    }

    DependencyObject IAttachedObject.AssociatedObject => this.AssociatedObject;

    public void Attach(DependencyObject dependencyObject)
    {
      if (dependencyObject == this.AssociatedObject)
        return;
      if (this.AssociatedObject != null)
        throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
      this.associatedObject = dependencyObject == null || this.AssociatedObjectTypeConstraint.IsAssignableFrom(dependencyObject.GetType()) ? dependencyObject : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.TypeConstraintViolatedExceptionMessage, new object[3]
      {
        (object) ((object) this).GetType().Name,
        (object) dependencyObject.GetType().Name,
        (object) this.AssociatedObjectTypeConstraint.Name
      }));
      this.OnAttached();
    }

    public void Detach()
    {
      this.OnDetaching();
      this.associatedObject = (DependencyObject) null;
    }
  }
}
