// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.TriggerBase
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

using System.Globalization;
using System.Windows.Markup;

namespace System.Windows.Interactivity
{
  [ContentProperty("Actions")]
  public abstract class TriggerBase : DependencyObject, IAttachedObject
  {
    private DependencyObject associatedObject;
    private Type associatedObjectTypeConstraint;
    public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register(nameof (Actions), typeof (TriggerActionCollection), typeof (TriggerBase), (PropertyMetadata) null);

    internal TriggerBase(Type associatedObjectTypeConstraint)
    {
      this.associatedObjectTypeConstraint = associatedObjectTypeConstraint;
      TriggerActionCollection actionCollection = new TriggerActionCollection();
      this.SetValue(TriggerBase.ActionsProperty, (object) actionCollection);
    }

    protected DependencyObject AssociatedObject => this.associatedObject;

    protected virtual Type AssociatedObjectTypeConstraint => this.associatedObjectTypeConstraint;

    public TriggerActionCollection Actions => (TriggerActionCollection) this.GetValue(TriggerBase.ActionsProperty);

    public event EventHandler<PreviewInvokeEventArgs> PreviewInvoke;

    protected void InvokeActions(object parameter)
    {
      if (this.PreviewInvoke != null)
      {
        PreviewInvokeEventArgs e = new PreviewInvokeEventArgs();
        this.PreviewInvoke((object) this, e);
        if (e.Cancelling)
          return;
      }
      foreach (TriggerAction action in (DependencyObjectCollection<TriggerAction>) this.Actions)
        action.CallInvoke(parameter);
    }

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
        throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerMultipleTimesExceptionMessage);
      this.associatedObject = dependencyObject == null || this.AssociatedObjectTypeConstraint.IsAssignableFrom(dependencyObject.GetType()) ? dependencyObject : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.TypeConstraintViolatedExceptionMessage, new object[3]
      {
        (object) ((object) this).GetType().Name,
        (object) dependencyObject.GetType().Name,
        (object) this.AssociatedObjectTypeConstraint.Name
      }));
      this.Actions.Attach(dependencyObject);
      this.OnAttached();
    }

    public void Detach()
    {
      this.OnDetaching();
      this.associatedObject = (DependencyObject) null;
      this.Actions.Detach();
    }
  }
}
