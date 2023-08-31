// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.Interaction
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

using System.Windows.Media;

namespace System.Windows.Interactivity
{
  public static class Interaction
  {
    public static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached("Triggers", typeof (TriggerCollection), typeof (Interaction), new PropertyMetadata(new PropertyChangedCallback(Interaction.OnTriggersChanged)));
    public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("Behaviors", typeof (BehaviorCollection), typeof (Interaction), new PropertyMetadata(new PropertyChangedCallback(Interaction.OnBehaviorsChanged)));

    public static TriggerCollection GetTriggers(DependencyObject obj)
    {
      TriggerCollection triggers = (TriggerCollection) obj.GetValue(Interaction.TriggersProperty);
      if (triggers == null)
      {
        triggers = new TriggerCollection();
        obj.SetValue(Interaction.TriggersProperty, (object) triggers);
      }
      return triggers;
    }

    public static BehaviorCollection GetBehaviors(DependencyObject obj)
    {
      BehaviorCollection behaviors = (BehaviorCollection) obj.GetValue(Interaction.BehaviorsProperty);
      if (behaviors == null)
      {
        behaviors = new BehaviorCollection();
        obj.SetValue(Interaction.BehaviorsProperty, (object) behaviors);
      }
      return behaviors;
    }

    private static void OnBehaviorsChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      BehaviorCollection oldValue = (BehaviorCollection) args.OldValue;
      BehaviorCollection newValue = (BehaviorCollection) args.NewValue;
      if (oldValue == newValue)
        return;
      if (oldValue != null && ((IAttachedObject) oldValue).AssociatedObject != null)
        oldValue.Detach();
      if (newValue == null || obj == null)
        return;
      if (((IAttachedObject) newValue).AssociatedObject != null)
        throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorCollectionMultipleTimesExceptionMessage);
      newValue.Attach(obj);
    }

    private static void OnTriggersChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      TriggerCollection oldValue = args.OldValue as TriggerCollection;
      TriggerCollection newValue = args.NewValue as TriggerCollection;
      if (oldValue == newValue)
        return;
      if (oldValue != null && ((IAttachedObject) oldValue).AssociatedObject != null)
        oldValue.Detach();
      if (newValue == null || obj == null)
        return;
      if (((IAttachedObject) newValue).AssociatedObject != null)
        throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerCollectionMultipleTimesExceptionMessage);
      newValue.Attach(obj);
    }

    internal static bool IsElementLoaded(FrameworkElement element)
    {
      UIElement rootVisual = Application.Current.RootVisual;
      if ((element.Parent ?? VisualTreeHelper.GetParent((DependencyObject) element)) != null)
        return true;
      return rootVisual != null && element == rootVisual;
    }
  }
}
