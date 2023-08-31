// Decompiled with JetBrains decompiler
// Type: Clarity.Phone.Extensions.VisualTreeExtensions
// Assembly: Yandex.WP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 97C22979-2005-499F-96B3-5A0F26418B8A
// Assembly location: C:\Users\Admin\Desktop\re\Yandex.Metro_WP8\Yandex.WP.dll

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Clarity.Phone.Extensions
{
  public static class VisualTreeExtensions
  {
    [NotNull]
    public static IEnumerable<DependencyObject> GetVisualAncestors(this DependencyObject element) => element != null ? VisualTreeExtensions.GetVisualAncestorsAndSelfIterator(element).Skip<DependencyObject>(1) : throw new ArgumentNullException(nameof (element));

    [NotNull]
    public static IEnumerable<DependencyObject> GetVisualAncestorsAndSelf(
      this DependencyObject element)
    {
      return element != null ? VisualTreeExtensions.GetVisualAncestorsAndSelfIterator(element) : throw new ArgumentNullException(nameof (element));
    }

    [NotNull]
    private static IEnumerable<DependencyObject> GetVisualAncestorsAndSelfIterator(
      DependencyObject element)
    {
      for (DependencyObject obj = element; obj != null; obj = VisualTreeHelper.GetParent(obj))
        yield return obj;
    }

    public static IEnumerable<T> GetVisualChildren<T>(this DependencyObject target) where T : DependencyObject => target.GetVisualChildren().Where<DependencyObject>((Func<DependencyObject, bool>) (child => child is T)).Cast<T>();

    public static IEnumerable<T> GetVisualChildren<T>(this DependencyObject target, bool strict) where T : DependencyObject => target.GetVisualChildren(strict).Where<DependencyObject>((Func<DependencyObject, bool>) (child => child is T)).Cast<T>();

    public static IEnumerable<DependencyObject> GetVisualChildren(
      this DependencyObject target,
      bool strict)
    {
      int count = VisualTreeHelper.GetChildrenCount(target);
      if (count == 0)
      {
        if (!strict && target is ContentControl && ((ContentControl) target).Content is DependencyObject child)
          yield return child;
      }
      else
      {
        for (int i = 0; i < count; ++i)
          yield return VisualTreeHelper.GetChild(target, i);
      }
    }

    public static IEnumerable<DependencyObject> GetVisualChildren(this DependencyObject element) => element != null ? element.GetVisualChildrenAndSelfIterator().Skip<DependencyObject>(1) : throw new ArgumentNullException(nameof (element));

    public static IEnumerable<DependencyObject> GetVisualChildrenAndSelf(
      this DependencyObject element)
    {
      return element != null ? element.GetVisualChildrenAndSelfIterator() : throw new ArgumentNullException(nameof (element));
    }

    private static IEnumerable<DependencyObject> GetVisualChildrenAndSelfIterator(
      this DependencyObject element)
    {
      yield return element;
      int count = VisualTreeHelper.GetChildrenCount(element);
      for (int i = 0; i < count; ++i)
        yield return VisualTreeHelper.GetChild(element, i);
    }

    private static IEnumerable<DependencyObject> GetVisualDecendants(
      DependencyObject target,
      bool strict,
      Queue<DependencyObject> queue)
    {
      foreach (DependencyObject visualChild in target.GetVisualChildren(strict))
        queue.Enqueue(visualChild);
      if (queue.Count != 0)
      {
        DependencyObject node = queue.Dequeue();
        yield return node;
        foreach (DependencyObject decendant in VisualTreeExtensions.GetVisualDecendants(node, strict, queue))
          yield return decendant;
      }
    }

    private static IEnumerable<DependencyObject> GetVisualDecendants(
      DependencyObject target,
      bool strict,
      Stack<DependencyObject> stack)
    {
      foreach (DependencyObject visualChild in target.GetVisualChildren(strict))
        stack.Push(visualChild);
      if (stack.Count != 0)
      {
        DependencyObject node = stack.Pop();
        yield return node;
        foreach (DependencyObject decendant in VisualTreeExtensions.GetVisualDecendants(node, strict, stack))
          yield return decendant;
      }
    }

    public static IEnumerable<DependencyObject> GetVisualDescendants(this DependencyObject element) => element != null ? VisualTreeExtensions.GetVisualDescendantsAndSelfIterator(element).Skip<DependencyObject>(1) : throw new ArgumentNullException(nameof (element));

    public static IEnumerable<DependencyObject> GetVisualDescendantsAndSelf(
      this DependencyObject element)
    {
      return element != null ? VisualTreeExtensions.GetVisualDescendantsAndSelfIterator(element) : throw new ArgumentNullException(nameof (element));
    }

    private static IEnumerable<DependencyObject> GetVisualDescendantsAndSelfIterator(
      DependencyObject element)
    {
      Queue<DependencyObject> remaining = new Queue<DependencyObject>();
      remaining.Enqueue(element);
      while (remaining.Count > 0)
      {
        DependencyObject obj = remaining.Dequeue();
        yield return obj;
        foreach (DependencyObject visualChild in obj.GetVisualChildren())
          remaining.Enqueue(visualChild);
      }
    }

    public static IEnumerable<DependencyObject> GetVisualSiblings(this DependencyObject element) => element.GetVisualSiblingsAndSelf().Where<DependencyObject>((Func<DependencyObject, bool>) (p => p != element));

    public static IEnumerable<DependencyObject> GetVisualSiblingsAndSelf(
      this DependencyObject element)
    {
      DependencyObject element1 = element != null ? VisualTreeHelper.GetParent(element) : throw new ArgumentNullException(nameof (element));
      return element1 != null ? element1.GetVisualChildren() : Enumerable.Empty<DependencyObject>();
    }

    public static Rect? GetBoundsRelativeTo(this FrameworkElement element, UIElement otherElement)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      if (otherElement == null)
        throw new ArgumentNullException(nameof (otherElement));
      try
      {
        GeneralTransform visual = ((UIElement) element).TransformToVisual(otherElement);
        if (visual != null)
        {
          Point point1;
          if (visual.TryTransform(new Point(), ref point1))
          {
            Point point2;
            if (visual.TryTransform(new Point(element.ActualWidth, element.ActualHeight), ref point2))
              return new Rect?(new Rect(point1, point2));
          }
        }
      }
      catch (ArgumentException ex)
      {
      }
      return new Rect?();
    }

    public static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
    {
      for (int index = 0; index < VisualTreeHelper.GetChildrenCount(obj); ++index)
      {
        DependencyObject child = VisualTreeHelper.GetChild(obj, index);
        if (child != null && child is childItem visualChild1)
          return visualChild1;
        childItem visualChild2 = VisualTreeExtensions.FindVisualChild<childItem>(child);
        if ((object) visualChild2 != null)
          return visualChild2;
      }
      return default (childItem);
    }

    public static FrameworkElement FindVisualChild(this FrameworkElement root, string name)
    {
      if (root.FindName(name) is FrameworkElement name1)
        return name1;
      foreach (FrameworkElement visualChild in ((DependencyObject) root).GetVisualChildren())
      {
        if (visualChild.FindName(name) is FrameworkElement name2)
          return name2;
      }
      return (FrameworkElement) null;
    }

    public static IEnumerable<DependencyObject> GetVisuals(this DependencyObject root)
    {
      int count = VisualTreeHelper.GetChildrenCount(root);
      for (int i = 0; i < count; ++i)
      {
        DependencyObject child = VisualTreeHelper.GetChild(root, i);
        yield return child;
        foreach (DependencyObject descendants in child.GetVisuals())
          yield return descendants;
      }
    }

    public static FrameworkElement GetVisualChild(this FrameworkElement node, int index) => VisualTreeHelper.GetChild((DependencyObject) node, index) as FrameworkElement;

    public static FrameworkElement GetVisualParent(this FrameworkElement node) => VisualTreeHelper.GetParent((DependencyObject) node) as FrameworkElement;

    public static VisualStateGroup GetVisualStateGroup(
      this FrameworkElement root,
      string groupName,
      bool searchAncestors)
    {
      foreach (object visualStateGroup1 in (IEnumerable) VisualStateManager.GetVisualStateGroups(root))
      {
        if (visualStateGroup1 is VisualStateGroup visualStateGroup2 && visualStateGroup2.Name == groupName)
          return visualStateGroup2;
      }
      if (searchAncestors)
      {
        FrameworkElement visualParent = root.GetVisualParent();
        if (visualParent != null)
          return visualParent.GetVisualStateGroup(groupName, true);
      }
      return (VisualStateGroup) null;
    }

    [Conditional("DEBUG")]
    public static void GetVisualChildTreeDebugText(this FrameworkElement root, StringBuilder result)
    {
      List<string> results = new List<string>();
      root.GetChildTree("", "  ", results);
      foreach (string str in results)
        result.AppendLine(str);
    }

    private static void GetChildTree(
      this FrameworkElement root,
      string prefix,
      string addPrefix,
      List<string> results)
    {
      string str = (!string.IsNullOrEmpty(root.Name) ? "[" + root.Name + "]" : "[Anonymous]") + " : " + root.GetType().Name;
      results.Add(prefix + str);
      foreach (FrameworkElement visualChild in ((DependencyObject) root).GetVisualChildren())
        visualChild.GetChildTree(prefix + addPrefix, addPrefix, results);
    }

    [Conditional("DEBUG")]
    public static void GetAncestorVisualTreeDebugText(
      this FrameworkElement node,
      StringBuilder result)
    {
      List<string> children = new List<string>();
      node.GetAncestorVisualTree(children);
      string str1 = "";
      foreach (string str2 in children)
      {
        result.AppendLine(str1 + str2);
        str1 += "  ";
      }
    }

    private static void GetAncestorVisualTree(this FrameworkElement node, List<string> children)
    {
      string str = (string.IsNullOrEmpty(node.Name) ? "[Anon]" : node.Name) + ": " + node.GetType().Name;
      children.Insert(0, str);
      FrameworkElement visualParent = node.GetVisualParent();
      if (visualParent == null)
        return;
      visualParent.GetAncestorVisualTree(children);
    }

    public static RequestedTransform GetTransform<RequestedTransform>(
      this UIElement element,
      TransformCreationMode mode)
      where RequestedTransform : Transform, new()
    {
      Transform renderTransform = element.RenderTransform;
      RequestedTransform requestedTransform = default (RequestedTransform);
      switch (renderTransform)
      {
        case null:
          if ((mode & TransformCreationMode.Create) != TransformCreationMode.Create)
            return default (RequestedTransform);
          RequestedTransform transform1 = new RequestedTransform();
          element.RenderTransform = (Transform) transform1;
          return transform1;
        case RequestedTransform transform4:
          return transform4;
        case MatrixTransform matrixTransform:
          if (!matrixTransform.Matrix.IsIdentity || (mode & TransformCreationMode.Create) != TransformCreationMode.Create || (mode & TransformCreationMode.IgnoreIdentityMatrix) != TransformCreationMode.IgnoreIdentityMatrix)
            return default (RequestedTransform);
          RequestedTransform transform2 = new RequestedTransform();
          element.RenderTransform = (Transform) transform2;
          return transform2;
        case TransformGroup transformGroup2:
          foreach (Transform child in (PresentationFrameworkCollection<Transform>) transformGroup2.Children)
          {
            if (child is RequestedTransform)
              return child as RequestedTransform;
          }
          if ((mode & TransformCreationMode.AddToGroup) != TransformCreationMode.AddToGroup)
            return default (RequestedTransform);
          RequestedTransform transform3 = new RequestedTransform();
          ((PresentationFrameworkCollection<Transform>) transformGroup2.Children).Add((Transform) transform3);
          return transform3;
        default:
          if ((mode & TransformCreationMode.CombineIntoGroup) != TransformCreationMode.CombineIntoGroup)
            return default (RequestedTransform);
          TransformGroup transformGroup1 = new TransformGroup();
          ((PresentationFrameworkCollection<Transform>) transformGroup1.Children).Add(renderTransform);
          ((PresentationFrameworkCollection<Transform>) transformGroup1.Children).Add((Transform) transform4);
          element.RenderTransform = (Transform) transformGroup1;
          return transform4;
      }
    }

    public static string GetTransformPropertyPath<RequestedType>(
      this FrameworkElement element,
      string subProperty)
      where RequestedType : Transform
    {
      Transform renderTransform = ((UIElement) element).RenderTransform;
      switch (renderTransform)
      {
        case RequestedType _:
          return string.Format("(RenderTransform).({0}.{1})", (object) typeof (RequestedType).Name, (object) subProperty);
        case TransformGroup _:
          TransformGroup transformGroup = renderTransform as TransformGroup;
          for (int index = 0; index < ((PresentationFrameworkCollection<Transform>) transformGroup.Children).Count; ++index)
          {
            if (((PresentationFrameworkCollection<Transform>) transformGroup.Children)[index] is RequestedType)
              return string.Format("(RenderTransform).(TransformGroup.Children)[" + (object) index + "].({0}.{1})", (object) typeof (RequestedType).Name, (object) subProperty);
          }
          break;
      }
      return "";
    }

    public static PlaneProjection GetPlaneProjection(this UIElement element, bool create)
    {
      Projection projection = element.Projection;
      PlaneProjection planeProjection = (PlaneProjection) null;
      if (projection is PlaneProjection)
        return projection as PlaneProjection;
      if (projection == null && create)
      {
        planeProjection = new PlaneProjection();
        element.Projection = (Projection) planeProjection;
      }
      return planeProjection;
    }

    public static void InvokeOnLayoutUpdated(this FrameworkElement element, Action action)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      EventHandler handler = (EventHandler) null;
      handler = (EventHandler) ((s, e) =>
      {
        element.LayoutUpdated -= handler;
        action();
      });
      element.LayoutUpdated += handler;
    }

    internal static IEnumerable<FrameworkElement> GetLogicalChildren(this FrameworkElement parent)
    {
      if (parent is Popup popup && popup.Child is FrameworkElement popupChild)
        yield return popupChild;
      ItemsControl itemsControl = parent as ItemsControl;
      if (itemsControl != null)
      {
        foreach (FrameworkElement logicalChild in Enumerable.Range(0, ((PresentationFrameworkCollection<object>) itemsControl.Items).Count).Select<int, DependencyObject>((Func<int, DependencyObject>) (index => itemsControl.ItemContainerGenerator.ContainerFromIndex(index))).OfType<FrameworkElement>())
          yield return logicalChild;
      }
      string name = parent.Name;
      Queue<FrameworkElement> queue = new Queue<FrameworkElement>(((DependencyObject) parent).GetVisualChildren().OfType<FrameworkElement>());
      while (queue.Count > 0)
      {
        FrameworkElement element = queue.Dequeue();
        if (element.Parent == parent || element is UserControl)
        {
          yield return element;
        }
        else
        {
          foreach (FrameworkElement frameworkElement in ((DependencyObject) element).GetVisualChildren().OfType<FrameworkElement>())
            queue.Enqueue(frameworkElement);
        }
      }
    }

    internal static IEnumerable<FrameworkElement> GetLogicalDescendents(this FrameworkElement parent) => (IEnumerable<FrameworkElement>) null;
  }
}
