using SuGarToolkit.Controls.Dialogs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuGarToolkit.Sample.Dialogs.ViewModels;

public partial class MessageBoxLikeExampleViewModel
{
    public string Title { get; set; } = "MessageBox";
    public string Content { get; set; } = "Lorem ipsum dolor sit amet.";
    public MessageBoxButtons Buttons { get; set; } = MessageBoxButtons.OK;
    public MessageBoxDefaultButton DefaultButton { get; set; } = MessageBoxDefaultButton.Button1;
    public bool IsChild { get; set; } = true;
    public bool IsModal { get; set; } = true;
}
