// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.TargetedTriggerAction
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

using System.Globalization;

namespace System.Windows.Interactivity
{
  public abstract class TargetedTriggerAction : TriggerAction
  {
    private Type targetTypeConstraint;
    private bool isTargetChangedRegistered;
    private NameResolver targetResolver;
    public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(nameof (TargetObject), typeof (object), typeof (TargetedTriggerAction), new PropertyMetadata(new PropertyChangedCallback(TargetedTriggerAction.OnTargetObjectChanged)));
    public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register(nameof (TargetName), typeof (string), typeof (TargetedTriggerAction), new PropertyMetadata(new PropertyChangedCallback(TargetedTriggerAction.OnTargetNameChanged)));

    public object TargetObject
    {
      get => this.GetValue(TargetedTriggerAction.TargetObjectProperty);
      set => this.SetValue(TargetedTriggerAction.TargetObjectProperty, value);
    }

    public string TargetName
    {
      get => (string) this.GetValue(TargetedTriggerAction.TargetNameProperty);
      set => this.SetValue(TargetedTriggerAction.TargetNameProperty, (object) value);
    }

    protected object Target
    {
      get
      {
        object obj = (object) this.AssociatedObject;
        if (this.TargetObject != null)
          obj = this.TargetObject;
        else if (this.IsTargetNameSet)
          obj = (object) this.TargetResolver.Object;
        return obj == null || this.TargetTypeConstraint.IsAssignableFrom(obj.GetType()) ? obj : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.RetargetedTypeConstraintViolatedExceptionMessage, (object) ((object) this).GetType().Name, (object) obj.GetType(), (object) this.TargetTypeConstraint, (object) nameof (Target)));
      }
    }

    protected override sealed Type AssociatedObjectTypeConstraint
    {
      get
      {
        object[] customAttributes = ((object) this).GetType().GetCustomAttributes(typeof (TypeConstraintAttribute), true);
        int index = 0;
        return index < customAttributes.Length ? ((TypeConstraintAttribute) customAttributes[index]).Constraint : typeof (DependencyObject);
      }
    }

    protected Type TargetTypeConstraint => this.targetTypeConstraint;

    private bool IsTargetNameSet => !string.IsNullOrEmpty(this.TargetName) || this.ReadLocalValue(TargetedTriggerAction.TargetNameProperty) != DependencyProperty.UnsetValue;

    private NameResolver TargetResolver => this.targetResolver;

    private bool IsTargetChangedRegistered
    {
      get => this.isTargetChangedRegistered;
      set => this.isTargetChangedRegistered = value;
    }

    internal TargetedTriggerAction(Type targetTypeConstraint)
      : base(typeof (DependencyObject))
    {
      this.targetTypeConstraint = targetTypeConstraint;
      this.targetResolver = new NameResolver();
      this.RegisterTargetChanged();
    }

    internal virtual void OnTargetChangedImpl(object oldTarget, object newTarget)
    {
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      DependencyObject associatedObject = this.AssociatedObject;
      Behavior behavior = associatedObject as Behavior;
      this.RegisterTargetChanged();
      if (behavior != null)
      {
        associatedObject = ((IAttachedObject) behavior).AssociatedObject;
        behavior.AssociatedObjectChanged += new EventHandler(this.OnBehaviorHostChanged);
      }
      this.TargetResolver.NameScopeReferenceElement = associatedObject as FrameworkElement;
    }

    protected override void OnDetaching()
    {
      Behavior associatedObject = this.AssociatedObject as Behavior;
      base.OnDetaching();
      this.OnTargetChangedImpl((object) this.TargetResolver.Object, (object) null);
      this.UnregisterTargetChanged();
      if (associatedObject != null)
        associatedObject.AssociatedObjectChanged -= new EventHandler(this.OnBehaviorHostChanged);
      this.TargetResolver.NameScopeReferenceElement = (FrameworkElement) null;
    }

    private void OnBehaviorHostChanged(object sender, EventArgs e) => this.TargetResolver.NameScopeReferenceElement = ((IAttachedObject) sender).AssociatedObject as FrameworkElement;

    private void RegisterTargetChanged()
    {
      if (this.IsTargetChangedRegistered)
        return;
      this.TargetResolver.ResolvedElementChanged += new EventHandler<NameResolvedEventArgs>(this.OnTargetChanged);
      this.IsTargetChangedRegistered = true;
    }

    private void UnregisterTargetChanged()
    {
      if (!this.IsTargetChangedRegistered)
        return;
      this.TargetResolver.ResolvedElementChanged -= new EventHandler<NameResolvedEventArgs>(this.OnTargetChanged);
      this.IsTargetChangedRegistered = false;
    }

    private static void OnTargetObjectChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      ((TargetedTriggerAction) obj).OnTargetChanged((object) obj, new NameResolvedEventArgs(args.OldValue, args.NewValue));
    }

    private static void OnTargetNameChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      ((TargetedTriggerAction) obj).TargetResolver.Name = (string) args.NewValue;
    }

    private void OnTargetChanged(object sender, NameResolvedEventArgs e)
    {
      if (this.AssociatedObject == null)
        return;
      this.OnTargetChangedImpl(e.OldObject, e.NewObject);
    }
  }
}
