#pragma checksum "C:\Users\watso\source\repos\UWPEngine-Win2D\UWPEngine\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4F988702463697F0E8B7FBB3C41C2D21"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UWPEngine
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 13
                {
                    this.grid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3: // MainPage.xaml line 14
                {
                    this.canvas = (global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)(target);
                    ((global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)this.canvas).CreateResources += this.canvas_CreateResources;
                    ((global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)this.canvas).Draw += this.canvas_Draw;
                    ((global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl)this.canvas).Update += this.canvas_Update;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

