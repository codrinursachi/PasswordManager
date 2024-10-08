using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PasswordManager.Models
{
    public record TagModel
    {
        public string Text {  get; set; }
        public Brush Brush { get; set; }= new SolidColorBrush(Color.FromRgb((byte)Random.Shared.Next(1, 200), (byte)Random.Shared.Next(1, 200), (byte)Random.Shared.Next(1, 200)));
    }
}
