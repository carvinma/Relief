   M S B A M L   `   `   `  ����   ReliefAnalysis!    ReliefAnalysis.UC_CaseStudy   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     $	��500��$	��680��.��� $��0,0,0,0q�.+��� -   $��txtDescription��$
��Left=�$��60��$��	95,16,0,0q�  �� AcceptsReturn  .  �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��577���� $��Description:��$
��Left=�$��5,16,0,0q�$	��Top=��� -   $
��Left=�$	��225��$��5,94,0,0q�$	��Top=�$	��667��.@��� $
��Feed��.��� ���	#FFE5E5E5.+�&   System.Windows.Controls.DataGrid  -   $��gridStreamIn��   CanUserAddRows .     HeadersVisibility$ Column=�   CanUserResizeColumns .     AutoGenerateColumns .  $
��Left=�$	��Top=�$	��202��$	��657��   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 �� *
streamname+4  .System.Windows.Controls.DataGridCheckBoxColumn  $ 	Flow Stop��$ 80 	 �� *
flowstop+  $ Flow Calc Factor��$	 120 	 �� *flowcalcfactor+  $ FlowRate(Kg/hr)��$	 120 	 �� *
flowrate+  $ Specific Enthalpy(KJ/Kg)��$	 160 	 �� *specificenthalpy+�� $��Product��.��� ���	#FFE5E5E5
 � ColumnDefinitions
 �� $	��56* ��� $
��163* �.+�  -   $��gridStreamOut�� .  $ Column=� .   .  $
��Left=�$	��Top=�$	��202��$	��657��$��2��$��0,0,0,-9q�   $
 Name��$	 120 	 �� *
streamname+  $ ProdType��$	 100 	 �� *
prodtype+  $ Flow Calc Factor��$	 120 	 �� *flowcalcfactor+  $ FlowRate(Kg/hr)��$	 120 	 �� *
flowrate+  $ Specific Enthalpy(KJ/Kg)��$	 160 	 �� *specificenthalpy+�� $��Reboiler��.��� ���	#FFE5E5E5.+�  -   $��
gridHeatIn�� .  $ Column=� .   .  $
��Left=�$	��Top=�$	��202��$	��657��   $
 Name��$	 120 	 �� *
heatername+  $ Dutylost��	 �� *
dutylost+  $ Duty Calc Factor��	 �� *dutycalcfactor+  $ Duty(MW)��	 �� *
heaterduty+�� $��	Condenser��.��� ���	#FFE5E5E5.+�  -   $��gridHeatOut�� .  $ Column=� .   .  $
��Left=�$	��Top=�$	��202��$	��657��   $
 Name��$	 120 	 �� *
heatername+  $ Dutylost��$	 120 	 �� *
dutylost+  $ Duty Calc Factor��$	 120 	 �� *dutycalcfactor+  $ Duty(MW)��$	 120 	 �� *
heaterduty+�� $��Result��$
��Left=�$��	9,396,0,0q�$	��Top=�$��91��$	��667��.��� $
��Left=�$��78��$	��Top=�$	��647��$��	10,0,-2,0q�.+��� $��Relief Rate(Kg/hr):��$
��Left=�$��0,10,0,0q�$	��Top=��� -   $��txtReliefRate��$
��Left=�$��25��$��
120,12,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��162���� $��Relief Temp(C):��$
��Left=�$��
335,10,0,0q�$	��Top=��� -   $��txtReliefTemp��$
��Left=�$��25��$��
437,10,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��170���� $��Relief Press(Mpag):��$
��Left=�$��
312,39,0,0q�$	��Top=��� -	   $��txtReliefPress��$
��Left=�$��25��$��
437,40,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��170���� $��
Relief MW:��$
��Left=�$��	43,39,0,0q�$	��Top=� J� RenderTransformOrigin$ 0.694,0.423&��� -
   $��txtReliefMW��$
��Left=�$��25��$��
120,41,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��162���� -   $��btnRunCalculation��$��Run Calculation��$
��Left=�$��259,369,0,0q�$	��Top=�$	��168���� -   $��btnCheckData��$��
Check Data��$
��Left=�$��
83,369,0,0q�$	��Top=�$	��115���� -   $��chkRunreboiler��$�� ��$
��Left=�$��
56,326,0,0q�$	��Top=�$��34���� -   $��btnRunreboiler��$��Run Reboiler Pinch��$
��Left=�$��
83,326,0,0q�$	��Top=�$	��129���� -   $��chkHeatExchange��$�� ��$
��Left=�$��237,326,0,0q�$	��Top=��� -   $��btnHeatExchange��$'��!Feed/Product Heat Exchange Modify��$
��Left=�$��259,326,0,0q�$	��Top=�$	��176���� -   $��chkAjustment��$�� ��$
��Left=�$��457,326,0,0q�$	��Top=��� -   $��btnAjustment��$"��Product Flow Rate Adjustment��$
��Left=�$��479,326,0,0q�$	��Top=�$	��181���� -    ��Name$ btnRunCalculation2��$��Run Calculation2��$
��Left=�$��479,369,0,0q�$	��Top=�$	��168��