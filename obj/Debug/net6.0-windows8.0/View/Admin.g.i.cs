﻿#pragma checksum "..\..\..\..\View\Admin.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "611702C11448F5AB48B0656CFCB067CFAFC6C1BF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CourseWork.View;
using CourseWork.ViewModel;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace CourseWork.View {
    
    
    /// <summary>
    /// Admin
    /// </summary>
    public partial class Admin : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\..\View\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox TableBox;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\View\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid Datagridd;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\View\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Prev;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\..\View\Admin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Next;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.14.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CourseWork;V1.0.0.0;component/view/admin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\Admin.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.14.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.TableBox = ((System.Windows.Controls.ComboBox)(target));
            
            #line 29 "..\..\..\..\View\Admin.xaml"
            this.TableBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.TableBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Datagridd = ((System.Windows.Controls.DataGrid)(target));
            
            #line 33 "..\..\..\..\View\Admin.xaml"
            this.Datagridd.Sorting += new System.Windows.Controls.DataGridSortingEventHandler(this.Datagrid_Sorting);
            
            #line default
            #line hidden
            
            #line 37 "..\..\..\..\View\Admin.xaml"
            this.Datagridd.AutoGeneratingColumn += new System.EventHandler<System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs>(this.Datagridd_AutoGeneratingColumn);
            
            #line default
            #line hidden
            
            #line 39 "..\..\..\..\View\Admin.xaml"
            this.Datagridd.CellEditEnding += new System.EventHandler<System.Windows.Controls.DataGridCellEditEndingEventArgs>(this.Datagrid_CellEditEnding);
            
            #line default
            #line hidden
            
            #line 40 "..\..\..\..\View\Admin.xaml"
            this.Datagridd.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.Datagrid_PreviewKeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Prev = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.Next = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
