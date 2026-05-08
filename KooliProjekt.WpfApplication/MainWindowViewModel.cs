using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel
    {
        public IList<User> Data 
        { 
            get
            {
                var items = new List<User>
                {
                    new User { Id = 1, Title = "Test 1" },
                    new User { Id = 2, Title = "Test 2" },
                    new User{ Id = 3, Title = "Test 3" }
                };

                return items;
            }
        }

        public object SelectedItem { get; set; }
    }
}
