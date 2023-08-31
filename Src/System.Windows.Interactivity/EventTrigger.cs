// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.EventTrigger
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

namespace System.Windows.Interactivity
{
  public class EventTrigger : EventTriggerBase<object>
  {
    public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(nameof (EventName), typeof (string), typeof (EventTrigger), new PropertyMetadata((object) "Loaded", new PropertyChangedCallback(EventTrigger.OnEventNameChanged)));

    public EventTrigger()
    {
    }

    public EventTrigger(string eventName) => this.EventName = eventName;

    public string EventName
    {
      get => (string) this.GetValue(EventTrigger.EventNameProperty);
      set => this.SetValue(EventTrigger.EventNameProperty, (object) value);
    }

    protected override string GetEventName() => this.EventName;

    private static void OnEventNameChanged(object sender, DependencyPropertyChangedEventArgs args) => ((EventTriggerBase) sender).OnEventNameChanged((string) args.OldValue, (string) args.NewValue);
  }
}
