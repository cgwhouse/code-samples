using Xamarin.Forms;

namespace CristianSampleApp.Models
{
    public class Animal
    {
        public bool CanFly { get; set; }
        public Color Color { get; }
        public string Cry { get; set; }
        public bool IsEndangered { get; set; }
        public string Name { get; }

        public Animal(string name, Color color)
        {
            Name = name;
            Color = color;
        }
    }
}
