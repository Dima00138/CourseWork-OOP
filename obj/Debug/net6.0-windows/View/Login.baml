   M S B A M L   `   `   `  ����E  ACourseWork, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CourseWork.ViewCourseWork.View  5   	   ;"clr-namespace:CourseWork.ViewModelCourseWork.ViewModel  5   	       CourseWork.View.Login   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:CourseWork.View     5   	   _ [MaterialDesignThemes.Wpf, Version=4.8.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   2	viewModel"clr-namespace:CourseWork.ViewModel  5   	   -   5           MaterialDesignBody#���   5
   	   $��Medium"�5   	   $��14(�5   	     � Title$  Login��5   	   $	��500��5   	   $	��450��5   	    � AllowsTransparency$
 True��5   	    � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	   
������  � 
ResizeMode$ NoResize=�5   	    � DataContext 5      )  #CourseWork.ViewModel.LoginViewModel  5   
   .��5      � .+�5   
   ��   MaterialDesignPaper#���  6    �� CornerRadius$ 20��5      � 5"   
    � RowDefinitions 5#      �� 5$      $
��0.8* �6    �� 5%      $��6* �6    .+�5(      � $��0��6   ���#004c9e.+�5)      �� $"��../Resources/logo_bzhd_w.png��6   $��80��5*      $��
-330 0 0 0q�5+      � 5-      $��Right=�6   $��10q�63    � ColumnDefinitions 5.      �� 5/      $��25 �6+   �� 50      $��25 �6+   .+�52      �� $��0 0 5 0q�6   $��25��53      $��25��6(   ��54      �� *HideButtonCommand+��55      �� 56      	 �� 
TargetType"	 ��6$   .�57   "   �� 
 �� Property
 � �� �� Value$ 25��6:   �� 58   "   
 � ��$ 25��6;   �� 59   "   
 � ��
 ���� 6?   �� 5:   "   
 � ��$ 0q�6D   �� 5;   "   
 � �� 5<   &   �� 5=   *    �� 
TargetType" ��6:   .R�5>   .   � .+�5?   2   �� 
������ $��25��6O   $��25��6Z    J� Opacity$ 0Q�6f   �� 5@   2    ��Name$
 Line��67   
�������$��3��6T   	 �� X1$ 0��6h   	 �� Y1$ 17��6o   	 �� X2$ 25��6w   	 �� Y2$ 17��6   5A   /   5B   +   5D   #   5E      5G      �� 5H      $��1��5J      $��25��5K      $��25��6$   $��5 0 0 0q�5L      ��5I      �� *CloseButtonCommand+��5M      �� 5N      "	 ��6$   .�5O   "   �� 
 � ��$ 25��6:   �� 5P   "   
 � ��$ 25��6;   �� 5Q   "   
 � ��
 ���� 6?   �� 5R   "   
 � ��$ 0q�6D   �� 5S   "   
 � �� 5T   &   �� 5U   *   " ��6:   .R�5V   .   � .+�5W   2   �� 
������ $��25��6O   $��25��6Z   $ 0Q�6f   F� 5X   2    F�Name$ Cross��67   
�������$��3��6U    J� SnapsToDevicePixels$
 True��6i   +���� 	=  	= ���!��!� 	= ��!���! 	= 	6�   5Y   /   5Z   +   5\   #   5]      5_      5`      5a      � 5c      $��1��6   .+�5e      �� ��5f      �� *	LoginView+5h      5j      5l      5m      5n      