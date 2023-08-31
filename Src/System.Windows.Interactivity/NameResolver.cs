// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.NameResolver
// Assembly: System.Windows.Interactivity, Version=3.8.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 64F8F5D1-A658-42A7-95DE-C44551E7B70F
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\System.Windows.Interactivity.dll

namespace System.Windows.Interactivity
{
  internal sealed class NameResolver
  {
    private string name;
    private FrameworkElement nameScopeReferenceElement;

    public event EventHandler<NameResolvedEventArgs> ResolvedElementChanged;

    public string Name
    {
      get => this.name;
      set
      {
        DependencyObject oldObject = this.Object;
        this.name = value;
        this.UpdateObjectFromName(oldObject);
      }
    }

    public DependencyObject Object => string.IsNullOrEmpty(this.Name) && this.HasAttempedResolve ? (DependencyObject) this.NameScopeReferenceElement : this.ResolvedObject;

    public FrameworkElement NameScopeReferenceElement
    {
      get => this.nameScopeReferenceElement;
      set
      {
        FrameworkElement referenceElement = this.NameScopeReferenceElement;
        this.nameScopeReferenceElement = value;
        this.OnNameScopeReferenceElementChanged(referenceElement);
      }
    }

    private FrameworkElement ActualNameScopeReferenceElement => this.NameScopeReferenceElement == null || !Interaction.IsElementLoaded(this.NameScopeReferenceElement) ? (FrameworkElement) null : this.GetActualNameScopeReference(this.NameScopeReferenceElement);

    private DependencyObject ResolvedObject { get; set; }

    private bool PendingReferenceElementLoad { get; set; }

    private bool HasAttempedResolve { get; set; }

    private void OnNameScopeReferenceElementChanged(FrameworkElement oldNameScopeReference)
    {
      if (this.PendingReferenceElementLoad)
      {
        oldNameScopeReference.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
        this.PendingReferenceElementLoad = false;
      }
      this.HasAttempedResolve = false;
      this.UpdateObjectFromName(this.Object);
    }

    private void UpdateObjectFromName(DependencyObject oldObject)
    {
      DependencyObject dependencyObject = (DependencyObject) null;
      this.ResolvedObject = (DependencyObject) null;
      if (this.NameScopeReferenceElement != null)
      {
        if (!Interaction.IsElementLoaded(this.NameScopeReferenceElement))
        {
          this.NameScopeReferenceElement.Loaded += new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
          this.PendingReferenceElementLoad = true;
          return;
        }
        if (!string.IsNullOrEmpty(this.Name))
        {
          FrameworkElement referenceElement = this.ActualNameScopeReferenceElement;
          if (referenceElement != null)
            dependencyObject = referenceElement.FindName(this.Name) as DependencyObject;
        }
      }
      this.HasAttempedResolve = true;
      this.ResolvedObject = dependencyObject;
      if (oldObject == this.Object)
        return;
      this.OnObjectChanged(oldObject, this.Object);
    }

    private void OnObjectChanged(DependencyObject oldTarget, DependencyObject newTarget)
    {
      if (this.ResolvedElementChanged == null)
        return;
      this.ResolvedElementChanged((object) this, new NameResolvedEventArgs((object) oldTarget, (object) newTarget));
    }

    private FrameworkElement GetActualNameScopeReference(FrameworkElement initialReferenceElement)
    {
      FrameworkElement nameScopeReference = initialReferenceElement;
      if (this.IsNameScope(initialReferenceElement))
      {
        if (!(initialReferenceElement.Parent is FrameworkElement frameworkElement))
          frameworkElement = nameScopeReference;
        nameScopeReference = frameworkElement;
      }
      return nameScopeReference;
    }

    private bool IsNameScope(FrameworkElement frameworkElement) => frameworkElement.Parent is FrameworkElement parent && parent.FindName(this.Name) != null;

    private void OnNameScopeReferenceLoaded(object sender, RoutedEventArgs e)
    {
      this.PendingReferenceElementLoad = false;
      this.NameScopeReferenceElement.Loaded -= new RoutedEventHandler(this.OnNameScopeReferenceLoaded);
      this.UpdateObjectFromName(this.Object);
    }
  }
}
