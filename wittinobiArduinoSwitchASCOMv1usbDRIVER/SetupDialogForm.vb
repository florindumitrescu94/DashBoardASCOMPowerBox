Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ASCOM.Utilities
Imports ASCOM.wittinobiArduinoSwitchASCOMv1usbDRIVER

<ComVisible(False)>
Public Class SetupDialogForm

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click ' OK button event handler
        ' Persist new values of user settings to the ASCOM profile
        Try
            Switch.comPort = ComboBoxComPort.SelectedItem ' Update the state variables with results from the dialogue
        Catch
            ' Ignore any errors here in case the PC does not have any COM ports that can be selected
        End Try
        Switch.traceState = chkTrace.Checked
        Switch.ConnectionDelay = ComboBoxConnectionDelay.SelectedItem ' Update the state variables with results from the dialogue
        Switch.numSwitch = 10 ' Update the state variables with results from the dialogue

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click 'Cancel button event handler
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ShowAscomWebPage(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.DoubleClick, PictureBox1.Click
        ' Click on ASCOM logo event handler
        Try
            System.Diagnostics.Process.Start("https://ascom-standards.org/")
        Catch noBrowser As System.ComponentModel.Win32Exception
            If noBrowser.ErrorCode = -2147467259 Then
                MessageBox.Show(noBrowser.Message)
            End If
        Catch other As System.Exception
            MessageBox.Show(other.Message)
        End Try
    End Sub

    Private Sub SetupDialogForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load ' Form load event handler
        ' Retrieve current values of user settings from the ASCOM Profile
        InitUI()
    End Sub

    Private Sub InitUI()
        chkTrace.Checked = Switch.traceState
        ' set the list of com ports to those that are currently available
        ComboBoxComPort.Items.Clear()
        ComboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames())       ' use System.IO because it's static
        ' select the current port if possible
        If ComboBoxComPort.Items.Contains(Switch.comPort) Then
            ComboBoxComPort.SelectedItem = Switch.comPort
        End If
        ComboBoxConnectionDelay.SelectedItem = Switch.ConnectionDelay
    End Sub

End Class
