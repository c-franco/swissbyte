using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace swissbyte.Models
{
    public class ListItemModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action Changed;

        private string text;
        private bool isChecked;

        public string Text
        {
            get => text;
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged();
                    Changed?.Invoke();
                }
            }
        }

        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnPropertyChanged();
                    Changed?.Invoke();
                }
            }
        }

        public bool IsNew { get; set; } = false;

        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}