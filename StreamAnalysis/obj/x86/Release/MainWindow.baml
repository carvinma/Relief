   M S B A M L   `   `   `  ���� I  EReliefAnalysis, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:ReliefAnalysisReliefAnalysis      ReliefAnalysis.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -Customhttp://fabtab.codeplex.com     7ReliefAnalysisclr-namespace:ReliefAnalysis     -     � Icon$  images/app.ico�� � Title$ 
MainWindow��$	��600��$	��800�� � ForceCursor .  � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=� � WindowStyle$ ThreeDBorderWindow=�����  �� Color# � ��.��]� .+�]� $	��Top=�$	��Top=�.+��� 
����ح�$��0q�$��1q����� # � ��._�o� -    o�Name$ menu1�� o� 
IsMainMenu . $	��Top=����� # � ������ # � ��.@�m� -   $
��File������ # � ������ # � ��.@�m� $��	New Plant��m� $��
Open Plant��m� $��Close Plant��m� $
��Save��m� $��Save As��m� $
��Exit��m� $��Input������ # � ������ # � ��.@�m� $!��Import from simulation file��m� $��Flowsheet configuration��m� $��Output������ # � ������ # � ��.@�m� $��Generate unit report��m� $��Tools������ # � ������ # � ��.@�m� $��Flash Calculation��m� $��Flow Rate Calculation��m� $��Options������ # � ������ # � ��.@�m� $��Tower Calculation Method��.@�m� $��Fluor��m� $��Texaco��m� $��HP��m� -   $��Dictionary Setting��m� -   $��Glossary Setting��m� $��Window������ # � ������ # � ��m� $
��Help������ # � ������ # � ��]� $	��Top=�$	��Top=�.+��� 
����ح�$��0q�$��1q����� # � ��._�`� $
��Left=�$��Center=�	 `� 
Background	 �� # � ��.�c� $	��Top=�$	��Top=�$��30������ # � ��.@��� -   $��btnNewPlant��
 � ToolTip$
 	New Plant��$��Center=�.���� $��images/newplant.ico���� -   $��btnOpenPlant��$
 
Open Plant��$��Center=�.���� $��images/openplant.ico���� -   $��btnClosePlant��$
 Close Plant��$��Center=�.���� $��images/closeplant.ico���� �� -	   $��btnSaveProject��$
 Save Project��$��Center=�.���� $��images/save.png���� �� -
   $��	FlashCalu��$
 Flash Calculation��$��Center=�.���� $��images/calculator.ico��]� -   $��leftdockpanel����. $
��Left=�$	��148��$
��Auto=�$
��Auto=� J� 	AllowDrop . ���� # � ��.+��� 
����ح�$��1q�$��1q�._�R� -   $��navView��$
��Left=�$	��Top=�$
��Auto=�$
��Auto=�$��0q�$��Stretch=�$��200px��.@�Q� -      RightContextMenu � ContextMenu# [  ����  �� Orientation$ 
Horizontal=�$��20��.+��� $��images/plant.ico��$��16��$��16��$
��Left=�$��Center=��� $��Right=�$��Relief Anaysis��$��Center=�$	��0,2q�]� -   $��centerdockpanel����. $
��Left=�.+��� 
����ح�$��1q�$��1q�._��� -   $��mainTab��$
��Left=�  tabRightContextMenu# [  � 	Resources  �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius ��Name �� Triggers P� Property P� Value �� 
TargetName��%V  '��       �� " ��.���  � �� �� " ��.R��� $��Border��$��1,1,1,0q�
�������$ 1,1,0,0��$	��2,0q�._��� $ ContentSite��$��Center=�$��Center=�$��Header��$
��10,2q� P�  �@��
IsSelected . . ��� $ Border�� � ��
 ��·�P�  �@��
IsSelected .  . ��� $ Border�� � ��
 �����