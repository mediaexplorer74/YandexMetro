// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.Behavior
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

using System.Globalization;

namespace System.Windows.Interactivity
{
  public abstract class Behavior : DependencyObject, IAttachedObject
  {
    private Type associatedType;
    private DependencyObject associatedObject;

    internal event EventHandler AssociatedObjectChanged;

    protected Type AssociatedType => this.associatedType;

    protected DependencyObject AssociatedObject => this.associatedObject;

    internal Behavior(Type associatedType) => this.associatedType = associatedType;

    protected virtual void OnAttached()
    {
    }

    protected virtual void OnDetaching()
    {
    }

    private void OnAssociatedObjectChanged()
    {
      if (this.AssociatedObjectChanged == null)
        return;
      this.AssociatedObjectChanged((object) this, new EventArgs());
    }

    DependencyObject IAttachedObject.AssociatedObject => this.AssociatedObject;

    public void Attach(DependencyObject dependencyObject)
    {
      if (dependencyObject == this.AssociatedObject)
        return;
      if (this.AssociatedObject != null)
        throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorMultipleTimesExceptionMessage);
      this.associatedObject = dependencyObject == null || this.AssociatedType.IsAssignableFrom(dependencyObject.GetType()) ? dependencyObject : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.TypeConstraintViolatedExceptionMessage, new object[3]
      {
        (object) ((object) this).GetType().Name,
        (object) dependencyObject.GetType().Name,
        (object) this.AssociatedType.Name
      }));
      this.OnAssociatedObjectChanged();
      this.OnAttached();
    }

    public void Detach()
    {
      this.OnDetaching();
      this.associatedObject = (DependencyObject) null;
      this.OnAssociatedObjectChanged();
    }
  }
}
