﻿#pragma checksum "C:\Users\Clemens\Documents\Visual Studio 2015\Projects\GraphomatUWP\GraphomatUWP\GraphomatDrawingLibUwp\ZoomButtonsControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9B805DDBA91D6B0259E11A1E44BEB9BC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GraphomatDrawingLibUwp
{
    partial class ZoomButtonsControl : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.UserControl element1 = (global::Windows.UI.Xaml.Controls.UserControl)(target);
                    #line 10 "..\..\..\ZoomButtonsControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.UserControl)element1).Loaded += this.Control_Loaded;
                    #line 10 "..\..\..\ZoomButtonsControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.UserControl)element1).SizeChanged += this.Control_SizeChanged;
                    #line default
                }
                break;
            case 2:
                {
                    this.btnHeightIn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 25 "..\..\..\ZoomButtonsControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnHeightIn).Click += this.btnHeightIn_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.btnHeightOut = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 26 "..\..\..\ZoomButtonsControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnHeightOut).Click += this.btnHeightOut_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.btnWidthIn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 27 "..\..\..\ZoomButtonsControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnWidthIn).Click += this.btnWidthIn_Click;
                    #line default
                }
                break;
            case 5:
                {
                    this.btnWidthOut = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 28 "..\..\..\ZoomButtonsControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnWidthOut).Click += this.btnWidthOut_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.btnBothIn = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 29 "..\..\..\ZoomButtonsControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnBothIn).Click += this.btnBothIn_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.btnBothOut = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 30 "..\..\..\ZoomButtonsControl.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnBothOut).Click += this.btnBothOut_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

