   M S B A M L   `   `   `  ����E  ACourseWork, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:CourseWork.ViewModelCourseWork.ViewModel  5      1clr-namespace:CourseWork.ViewCourseWork.View  5	          CourseWork.View.Admin   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=4.8.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      2	viewmodel"clr-namespace:CourseWork.ViewModel  5      )localclr-namespace:CourseWork.View  5	      .��5      �   � RowDefinitions  5   
   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $��60 �6   .+�5   
   �� -   5         ��Name$ TableBox��5      $��0��5      $��
20 10 20 0q�5      $	��Top=�5      -  'MaterialDesignThemes.Wpf.ComboBoxAssist   	MaxLength$ 28��5      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Table��5         HintOpacity$	 .26Q�5       �� 
IsEditable$ False��5       �� SelectedItem$ 0��5       �� Text 5      �� *CurrentTable+ �� Mode$ TwoWay=���5      �� *Tables+5      &   System.Windows.Controls.DataGrid  5   
   -   5        	  Name$	 	Datagridd��5      $��1��5       
   CanUserAddRows$

 True��5"         CanUserDeleteRows$
 True��5#         AutoGenerateColumns$
 True��5$      /  )MaterialDesignThemes.Wpf.ValidationAssist   Suppress$
 True��5&         SelectionMode$ Extended=�5)         SelectionUnit$ CellOrRowHeader=�5*      �� 5+   
    �� Orientation$ 
Horizontal=�6   $��2��5,      $��Center=�5-      $��Center=�5.      .+�5/      �� -   5         ��Name$
 Prev��5/      $��<��50      $��50(�51      $��	0 -15 0 0q�52         MaterialDesignIconButton#��[  53      $��50��54      ��55      �� *PrevButtonCommand+�� 57      -   5        $
 Next��57      $��>��58      $��50(�59      $��	700 0 0 0q�5:      $��	0 -15 0 0q�5;      #��[  5<      $��50��5=      ��5>      �� *NextButtonCommand+5@      5A      5B      