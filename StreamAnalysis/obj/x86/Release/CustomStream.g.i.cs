﻿#pragma checksum "..\..\..\CustomStream.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "408D3FC5D605BEFF7A154E6FEE7D8372"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17929
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ReliefAnalysis {
    
    
    /// <summary>
    /// CustomStream
    /// </summary>
    public partial class CustomStream : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\CustomStream.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid1;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\CustomStream.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtName;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\CustomStream.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtDescription;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\CustomStream.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtVabFrac;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\CustomStream.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtTemp;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\CustomStream.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPres;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\CustomStream.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtWf;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\CustomStream.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSph;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\CustomStream.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtH;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\CustomStream.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOK;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\CustomStream.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ReliefAnalysis;component/customstream.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\CustomStream.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 5 "..\..\..\CustomStream.xaml"
            ((ReliefAnalysis.CustomStream)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded_1);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grid1 = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.txtName = ((System.Windows.Controls.TextBox)(target));
            
            #line 16 "..\..\..\CustomStream.xaml"
            this.txtName.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtDescription = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtVabFrac = ((System.Windows.Controls.TextBox)(target));
            
            #line 21 "..\..\..\CustomStream.xaml"
            this.txtVabFrac.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtTemp = ((System.Windows.Controls.TextBox)(target));
            
            #line 23 "..\..\..\CustomStream.xaml"
            this.txtTemp.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txtPres = ((System.Windows.Controls.TextBox)(target));
            
            #line 28 "..\..\..\CustomStream.xaml"
            this.txtPres.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.txtWf = ((System.Windows.Controls.TextBox)(target));
            
            #line 29 "..\..\..\CustomStream.xaml"
            this.txtWf.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.txtSph = ((System.Windows.Controls.TextBox)(target));
            
            #line 30 "..\..\..\CustomStream.xaml"
            this.txtSph.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.txtH = ((System.Windows.Controls.TextBox)(target));
            
            #line 31 "..\..\..\CustomStream.xaml"
            this.txtH.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            this.btnOK = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\CustomStream.xaml"
            this.btnOK.Click += new System.Windows.RoutedEventHandler(this.btnOK_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\CustomStream.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.btnCancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

