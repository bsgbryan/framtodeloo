using UnityEngine;

using UnityEngine.UIElements;

namespace MayB.Games.UI.Elements.Bounded.Int {
  public class VectorThree : VisualElement {
    private Sparse3DVolume Source;
    private Label Property;
    private string[] Labels = new string[3];
    private Slider[] Sliders = new Slider[3];
    private VisualElement[] Buttons = new VisualElement[3];

    public VectorThree(
      string prop,
      string[] labels,
      int min,
      int max,
      Vector3 src,
      EventCallback<Vector3> cb
    ) {
      string property               = prop.ToLower().Trim();
      string PropertFieldDescriptor = $"{property}-bounded-int-vector3";

      contentContainer.name = PropertFieldDescriptor;

      contentContainer.AddToClassList(property);
      contentContainer.AddToClassList("bounded-int");
      contentContainer.AddToClassList("vector3"); 

      Property = new Label {
        name = $"{PropertFieldDescriptor}-header",
        text = prop
      };

      for (int i = 0; i < 3; i++) {
        int v = i;

        Labels[v] = labels[v];

        Sliders[v] = new Slider(src[v], min, max, delegate(int val) {
          ((Button) Buttons[v].ElementAt(0)).text = FormatButtonText(v);

          Vector3 updated = new Vector3();

          updated.x = v == 0 ? val : src.x;
          updated.y = v == 1 ? val : src.y;
          updated.z = v == 2 ? val : src.z;

          cb(updated);

          src = updated;
        });

        Buttons[v] = new VisualElement();
        Buttons[v].Add(new Button {
          name = $"{PropertFieldDescriptor}-{Labels[v].ToLower()}-tab",
          text = FormatButtonText(v)
        });

        string side = v == 0 ? "left-rounded" : v == 2 ? "right-rounded" : "";

        Buttons[v].AddToClassList(side);
        Buttons[v].AddToClassList("tab");

        ((Button) Buttons[v].ElementAt(0)).clicked += delegate() {
          for (int s = 0; s < Sliders.Length; s++)
            Sliders[s].EnableInClassList("active", s == v);
          
          for (int b = 0; b < Buttons.Length; b++) {
            Buttons[b].EnableInClassList("active",        b == v);
            Buttons[b].EnableInClassList("right-rounded", b == v - 1);
            Buttons[b].EnableInClassList("left-rounded",  b == v + 1);

            ((Button) Buttons[b].ElementAt(0)).text = FormatButtonText(b);
          }
        };
      }

      var TabBar = new VisualElement {
        name = $"{PropertFieldDescriptor}-tab-bar"
      };

      TabBar.AddToClassList("tab-bar");

      foreach (var btn in Buttons)
        TabBar.Add(btn);
      
      contentContainer.Add(Property);
      contentContainer.Add(TabBar);

      foreach (var slider in Sliders)
        contentContainer.Add(slider);
    }

    private string FormatButtonText(int i) => $"{Labels[i]}\n{Sliders[i].Value}";
  }
}