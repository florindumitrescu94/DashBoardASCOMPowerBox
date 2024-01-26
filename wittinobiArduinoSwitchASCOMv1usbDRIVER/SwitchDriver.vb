
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
'
' Implements:	ASCOM Switch interface version: 1.0
' Author:		Tobias Wittmann (wittinobi) <wittinobi@wittinobi.de>
' Modified by:  Florin Dumitrescu <dumitrescu.florin94@gmail.com>
'
' Edit Log:
'
' Date			Who	                         Vers	Description
' -----------	---------------------------	 -----	-------------------------------
' 27-06-2022	Tobias Wittmann (wittinobi)	 1.0.0	Initial edit, from Switch template
' ---------------------------------------------------------------------------------
' 25-01-2024    Florin Dumitrescu            1.0.1  Modified code to work with given Arduino code, modified driver name
' ---------------------------------------------------------------------------------
'
'
' Your driver's ID is ASCOM.DashBoardPowerBox.Switch
'
' The Guid attribute sets the CLSID for ASCOM.DeviceName.Switch
' The ClassInterface/None attribute prevents an empty interface called
' _Switch from being created and used as the [default] interface
'

' This definition is used to select code that's only applicable for one device type
#Const Device = "Switch"

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
Imports ASCOM
Imports ASCOM.Astrometry
Imports ASCOM.Astrometry.AstroUtils
Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

<Guid("e7d8f0f2-9d4c-4396-a40b-a1d21d6bef7d")>
<ClassInterface(ClassInterfaceType.None)>
Public Class Switch

    ' The Guid attribute sets the CLSID for ASCOM.wittinobiArduinoSwitchASCOMv1usbDRIVER.Switch
    ' The ClassInterface/None attribute prevents an empty interface called
    ' _wittinobiArduinoSwitchASCOMv1usbDRIVER from being created and used as the [default] interface

    ' TODO Replace the not implemented exceptions with code to implement the function or
    ' throw the appropriate ASCOM exception.
    '
    Implements ISwitchV2

    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Friend Shared driverID As String = "ASCOM.wittinobiArduinoSwitchASCOMv1usbDRIVER.Switch"
    Private Shared driverDescription As String = "DashBoard PowerBox Switch"
    Friend Shared comPortProfileName As String = "COM Port" 'Constants used for Profile persistence
    Friend Shared traceStateProfileName As String = "Trace Level"
    Friend Shared comPortDefault As String = "COM1"
    Friend Shared traceStateDefault As String = "False"

    Friend Shared comPort As String ' Variables to hold the current device configuration
    Friend Shared traceState As Boolean

    Private connectedState As Boolean ' Private variable to hold the connected state
    Private utilities As Util ' Private variable to hold an ASCOM Utilities object
    Private astroUtilities As AstroUtils ' Private variable to hold an AstroUtils object to provide the Range method
    Private TL As TraceLogger ' Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)

    Private objSerial As ASCOM.Utilities.Serial

    Friend Shared ConnectionDelayProfileName As String = "Connection Delay (ms)"
    Friend Shared ConnectionDelayDefault As String = "5000"
    Friend Shared ConnectionDelay As String

    Friend Shared numSwitchProfileName As String = "Max Switches"
    Friend Shared numSwitchDefault As String = "10"
    Friend Shared numSwitch As String

    Friend Shared SwitchName0ProfileName As String = "DC Jacks"
    Friend Shared SwitchName1ProfileName As String = "PWM 1 - Main"
    Friend Shared SwitchName2ProfileName As String = "PWM 2 - Guide"
    Friend Shared SwitchName3ProfileName As String = "Temperature sensor"
    Friend Shared SwitchName4ProfileName As String = "Humidity sensor"
    Friend Shared SwitchName5ProfileName As String = "Dew Point sensor"
    Friend Shared SwitchName6ProfileName As String = "Voltage sensor"
    Friend Shared SwitchName7ProfileName As String = "Current sensor"
    Friend Shared SwitchName8ProfileName As String = "Power sensor"
    Friend Shared SwitchName9ProfileName As String = "Total power usage"
    Friend Shared SwitchName0Default As String = "DC Jacks"
    Friend Shared SwitchName1Default As String = "PWM 1 - Main"
    Friend Shared SwitchName2Default As String = "PWM 2 - Guide"
    Friend Shared SwitchName3Default As String = "Temperature sensor"
    Friend Shared SwitchName4Default As String = "Humidity sensor"
    Friend Shared SwitchName5Default As String = "Dew Point sensor"
    Friend Shared SwitchName6Default As String = "Voltage sensor"
    Friend Shared SwitchName7Default As String = "Current sensor"
    Friend Shared SwitchName8Default As String = "Power sensor"
    Friend Shared SwitchName9Default As String = "Total power consumption"
    Friend Shared SwitchName0 As String
    Friend Shared SwitchName1 As String
    Friend Shared SwitchName2 As String
    Friend Shared SwitchName3 As String
    Friend Shared SwitchName4 As String
    Friend Shared SwitchName5 As String
    Friend Shared SwitchName6 As String
    Friend Shared SwitchName7 As String
    Friend Shared SwitchName8 As String
    Friend Shared SwitchName9 As String

    Friend Shared SwitchState0ProfileName As String = "DC Jack state"
    Friend Shared SwitchState1ProfileName As String = "PWM 1 state"
    Friend Shared SwitchState2ProfileName As String = "PWM 2 state"
    Friend Shared SwitchState3ProfileName As String = "Temperature reading"
    Friend Shared SwitchState4ProfileName As String = "Humidity reading"
    Friend Shared SwitchState5ProfileName As String = "Dew point reading"
    Friend Shared SwitchState6ProfileName As String = "Voltage reading"
    Friend Shared SwitchState7ProfileName As String = "Current reading"
    Friend Shared SwitchState8ProfileName As String = "Power reading"
    Friend Shared SwitchState9ProfileName As String = "Total power usage reading"
    Friend Shared SwitchState0Default As String = "OFF"
    Friend Shared SwitchState1Default As String = "0"
    Friend Shared SwitchState2Default As String = "0"
    Friend Shared SwitchState3Default As String = "0.00"
    Friend Shared SwitchState4Default As String = "0.0"
    Friend Shared SwitchState5Default As String = "0.00"
    Friend Shared SwitchState6Default As String = "0.00"
    Friend Shared SwitchState7Default As String = "0.00"
    Friend Shared SwitchState8Default As String = "0.00"
    Friend Shared SwitchState9Default As String = "0.00"
    Friend Shared SwitchState0 As String
    Friend Shared SwitchState1 As String
    Friend Shared SwitchState2 As String
    Friend Shared SwitchState3 As String
    Friend Shared SwitchState4 As String
    Friend Shared SwitchState5 As String
    Friend Shared SwitchState6 As String
    Friend Shared SwitchState7 As String
    Friend Shared SwitchState8 As String
    Friend Shared SwitchState9 As String

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()

        ReadProfile() ' Read device configuration from the ASCOM Profile store
        TL = New TraceLogger("", "DashBoardPowerBox")
        TL.Enabled = traceState
        TL.LogMessage("Switch", "Starting initialisation")

        connectedState = False ' Initialise connected to false
        utilities = New Util() ' Initialise util object
        astroUtilities = New AstroUtils 'Initialise new astro utilities object

        'TODO: Implement your additional construction here

        TL.LogMessage("Switch", "Completed initialisation")
    End Sub

    '
    ' PUBLIC COM INTERFACE ISwitchV2 IMPLEMENTATION
    '

#Region "Common properties and methods"
    ''' <summary>
    ''' Displays the Setup Dialog form.
    ''' If the user clicks the OK button to dismiss the form, then
    ''' the new settings are saved, otherwise the old values are reloaded.
    ''' THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
    ''' </summary>
    Public Sub SetupDialog() Implements ISwitchV2.SetupDialog
        ' consider only showing the setup dialog if not connected
        ' or call a different dialog if connected
        If IsConnected Then
            System.Windows.Forms.MessageBox.Show("Already connected, just press OK")
        End If

        Using F As SetupDialogForm = New SetupDialogForm()
            Dim result As System.Windows.Forms.DialogResult = F.ShowDialog()
            If result = DialogResult.OK Then
                WriteProfile() ' Persist device configuration values to the ASCOM Profile store
            End If
        End Using
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements ISwitchV2.SupportedActions
        Get
            TL.LogMessage("SupportedActions Get", "Returning empty arraylist")
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ISwitchV2.Action
        Throw New ActionNotImplementedException("Action " & ActionName & " is not supported by this driver")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ISwitchV2.CommandBlind
        CheckConnected("CommandBlind")
        ' TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
        ' If implemented, CommandBlind must send the supplied command to the mount And return immediately without waiting for a response

        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean _
        Implements ISwitchV2.CommandBool
        CheckConnected("CommandBool")
        ' TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
        ' If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True Or False value

        ' Dim retString as String = CommandString(command, raw) ' Send the command And wait for the response
        ' Dim retBool as Boolean = XXXXXXXXXXXXX ' Parse the returned string And create a boolean True / False value
        ' Return retBool ' Return the boolean value to the client

        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String _
        Implements ISwitchV2.CommandString
        CheckConnected("CommandString")
        ' TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
        ' If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client

        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements ISwitchV2.Connected
        Get
            TL.LogMessage("Connected Get", IsConnected.ToString())
            Return IsConnected
        End Get
        Set(value As Boolean)
            TL.LogMessage("Connected Set", value.ToString())
            If value = IsConnected Then
                Return
            End If

            If value Then
                connectedState = True
                TL.LogMessage("Connected Set", "Connecting to port " + comPort)
                ' TODO connect to the device
                objSerial = New ASCOM.Utilities.Serial
                Dim numComPort As String
                numComPort = comPort.Replace("COM", "")
                objSerial.Port = CShort(numComPort)
                objSerial.Speed = 9600
                objSerial.ReceiveTimeout = 5
                objSerial.Connected = True
                Threading.Thread.Sleep(CShort(ConnectionDelay))
                objSerial.ClearBuffers()
                If CShort(numSwitch) >= 1 Then
                    SwitchState0 = "OFF"
                ElseIf CShort(numSwitch) >= 2 Then
                    SwitchState1 = "0"
                ElseIf CShort(numSwitch) >= 3 Then
                    SwitchState2 = "0"
                ElseIf CShort(numSwitch) >= 4 Then
                    SwitchState3 = "0"
                ElseIf CShort(numSwitch) >= 5 Then
                    SwitchState4 = "0"
                ElseIf CShort(numSwitch) >= 6 Then
                    SwitchState5 = "0"
                ElseIf CShort(numSwitch) >= 7 Then
                    SwitchState6 = "0"
                ElseIf CShort(numSwitch) >= 8 Then
                    SwitchState7 = "0"
                ElseIf CShort(numSwitch) >= 9 Then
                    SwitchState8 = "0"
                ElseIf CShort(numSwitch) >= 10 Then
                    SwitchState9 = "0"
                Else
                    TL.LogMessage("Switch" + numSwitch.ToString(), "Invalid Value")
                    Throw New InvalidValueException("Switch", numSwitch.ToString(), String.Format("0 to {0}", numSwitches - 1))
                End If
            Else
                connectedState = False
                TL.LogMessage("Connected Set", "Disconnecting from port " + comPort)
                ' TODO disconnect from the device
                objSerial.Connected = False
            End If
        End Set
    End Property

    Public ReadOnly Property Description As String Implements ISwitchV2.Description
        Get
            ' this pattern seems to be needed to allow a public property to return a private field
            Dim d As String = driverDescription
            TL.LogMessage("Description Get", d)
            Return d
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements ISwitchV2.DriverInfo
        Get
            Dim m_version As Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
            ' TODO customise this driver description
            Dim s_driverInfo As String = "Information about the driver itself. Version: " + m_version.Major.ToString() + "." + m_version.Minor.ToString()
            TL.LogMessage("DriverInfo Get", s_driverInfo)
            Return s_driverInfo
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ISwitchV2.DriverVersion
        Get
            ' Get our own assembly and report its version number
            TL.LogMessage("DriverVersion Get", Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(2))
            Return Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(2)
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ISwitchV2.InterfaceVersion
        Get
            TL.LogMessage("InterfaceVersion Get", "1")
            Return 1
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ISwitchV2.Name
        Get
            Dim s_name As String = "DashBoard PowerBox"
            TL.LogMessage("Name Get", s_name)
            Return s_name
        End Get
    End Property

    Public Sub Dispose() Implements ISwitchV2.Dispose
        ' Clean up the trace logger and util objects
        TL.Enabled = False
        TL.Dispose()
        TL = Nothing
        utilities.Dispose()
        utilities = Nothing
        astroUtilities.Dispose()
        astroUtilities = Nothing
        objSerial.Dispose()
        objSerial = Nothing
    End Sub

#End Region

#Region "ISwitchV2 Implementation"

    Dim numSwitches As Short = CShort(numSwitch)

    ''' <summary>
    ''' The number of switches managed by this driver
    ''' </summary>
    Public ReadOnly Property MaxSwitch As Short Implements ISwitchV2.MaxSwitch
        Get
            TL.LogMessage("MaxSwitch Get", numSwitches.ToString())
            Return CShort(numSwitches)
        End Get
    End Property

    ''' <summary>
    ''' Return the name of switch n
    ''' </summary>
    ''' <param name="id">The switch number to return</param>
    ''' <returns>The name of the switch</returns>
    Public Function GetSwitchName(id As Short) As String Implements ISwitchV2.GetSwitchName
        Validate("GetSwitchName", id)
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Switch"
            If id = 0 And CShort(numSwitch) >= 1 Then
                TL.LogMessage("GetSwitchName " + id.ToString(), SwitchName0)
                Return SwitchName0
            ElseIf id = 1 And CShort(numSwitch) >= 2 Then
                TL.LogMessage("GetSwitchName " + id.ToString(), SwitchName1)
                Return SwitchName1
            ElseIf id = 2 And CShort(numSwitch) >= 3 Then
                TL.LogMessage("GetSwitchName " + id.ToString(), SwitchName2)
                Return SwitchName2
            ElseIf id = 3 And CShort(numSwitch) >= 4 Then
                TL.LogMessage("GetSwitchName " + id.ToString(), SwitchName3)
                Return SwitchName3
            ElseIf id = 4 And CShort(numSwitch) >= 5 Then
                TL.LogMessage("GetSwitchName " + id.ToString(), SwitchName4)
                Return SwitchName4
            ElseIf id = 5 And CShort(numSwitch) >= 6 Then
                TL.LogMessage("GetSwitchName " + id.ToString(), SwitchName5)
                Return SwitchName5
            ElseIf id = 6 And CShort(numSwitch) >= 7 Then
                TL.LogMessage("GetSwitchName " + id.ToString(), SwitchName6)
                Return SwitchName6
            ElseIf id = 7 And CShort(numSwitch) >= 8 Then
                TL.LogMessage("GetSwitchName " + id.ToString(), SwitchName7)
                Return SwitchName7
            ElseIf id = 8 And CShort(numSwitch) >= 9 Then
                TL.LogMessage("GetSwitchName " + id.ToString(), SwitchName8)
                Return SwitchName8
            ElseIf id = 9 And CShort(numSwitch) >= 10 Then
                TL.LogMessage("GetSwitchName " + id.ToString(), SwitchName9)
                Return SwitchName9
            Else
                TL.LogMessage("GetSwitchName", "Not Implemented")
                Throw New ASCOM.MethodNotImplementedException("GetSwitchName")
            End If
        End Using
    End Function

    ''' <summary>
    ''' Sets a switch name to a specified value
    ''' </summary>
    ''' <param name="id">The number of the switch whose name is to be set</param>
    ''' <param name="name">The name of the switch</param>
    Sub SetSwitchName(id As Short, name As String) Implements ISwitchV2.SetSwitchName
        Validate("SetSwitchName", id)
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Switch"
            If id = 0 And CShort(numSwitch) >= 1 Then
                TL.LogMessage("SetSwitchName " + id.ToString(), name)
                SwitchName0 = name
                driverProfile.WriteValue(driverID, SwitchName0ProfileName, SwitchName0.ToString(), "DC Jacks")
            ElseIf id = 1 And CShort(numSwitch) >= 2 Then
                TL.LogMessage("SetSwitchName " + id.ToString(), name)
                SwitchName1 = name
                driverProfile.WriteValue(driverID, SwitchName1ProfileName, SwitchName1.ToString(), "PWM 1 - Main")
            ElseIf id = 2 And CShort(numSwitch) >= 3 Then
                TL.LogMessage("SetSwitchName " + id.ToString(), name)
                SwitchName2 = name
                driverProfile.WriteValue(driverID, SwitchName2ProfileName, SwitchName2.ToString(), "PWM 2 - Guide")
            ElseIf id = 3 And CShort(numSwitch) >= 4 Then
                TL.LogMessage("SetSwitchName " + id.ToString(), name)
                SwitchName3 = name
                driverProfile.WriteValue(driverID, SwitchName3ProfileName, SwitchName3.ToString(), "Temperature sensor")
            ElseIf id = 4 And CShort(numSwitch) >= 5 Then
                TL.LogMessage("SetSwitchName " + id.ToString(), name)
                SwitchName4 = name
                driverProfile.WriteValue(driverID, SwitchName4ProfileName, SwitchName4.ToString(), "Humidity sensor")
            ElseIf id = 5 And CShort(numSwitch) >= 6 Then
                TL.LogMessage("SetSwitchName " + id.ToString(), name)
                SwitchName5 = name
                driverProfile.WriteValue(driverID, SwitchName5ProfileName, SwitchName5.ToString(), "Dew Point sensor")
            ElseIf id = 6 And CShort(numSwitch) >= 7 Then
                TL.LogMessage("SetSwitchName " + id.ToString(), name)
                SwitchName6 = name
                driverProfile.WriteValue(driverID, SwitchName6ProfileName, SwitchName6.ToString(), "Voltage sensor")
            ElseIf id = 7 And CShort(numSwitch) >= 8 Then
                TL.LogMessage("SetSwitchName " + id.ToString(), name)
                SwitchName7 = name
                driverProfile.WriteValue(driverID, SwitchName7ProfileName, SwitchName7.ToString(), "Current sensor")
            ElseIf id = 8 And CShort(numSwitch) >= 9 Then
                TL.LogMessage("SetSwitchName " + id.ToString(), name)
                SwitchName8 = name
                driverProfile.WriteValue(driverID, SwitchName8ProfileName, SwitchName8.ToString(), "Power sensor")
            ElseIf id = 9 And CShort(numSwitch) >= 10 Then
                TL.LogMessage("SetSwitchName " + id.ToString(), name)
                SwitchName9 = name
                driverProfile.WriteValue(driverID, SwitchName9ProfileName, SwitchName9.ToString(), "Total Power Consumption")
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Gets the description of the specified switch. This is to allow a fuller description of the switch to be returned, for example for a tool tip.
    ''' </summary>
    ''' <param name="id">The number of the switch whose description is to be returned</param><returns></returns>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    Public Function GetSwitchDescription(id As Short) As String Implements ISwitchV2.GetSwitchDescription
        Validate("GetSwitchDescription", id)
        TL.LogMessage("GetSwitchDescription " + id.ToString(), "Port " + id.ToString())
        Dim s_GetSwitchDescription As String = ""
        If id = 0 And CShort(numSwitch) >= 1 Then
            s_GetSwitchDescription = "DC Jacks x4 12V"
        ElseIf id = 1 And CShort(numSwitch) >= 2 Then
            s_GetSwitchDescription = "PWM 1 Port 0%-100% in 20% steps"
        ElseIf id = 2 And CShort(numSwitch) >= 3 Then
            s_GetSwitchDescription = "PWM 2 Port 0%-100% in 20% steps"
        ElseIf id = 3 And CShort(numSwitch) >= 4 Then
            s_GetSwitchDescription = "Temperature in Celsius"
        ElseIf id = 4 And CShort(numSwitch) >= 5 Then
            s_GetSwitchDescription = "Humidity %"
        ElseIf id = 5 And CShort(numSwitch) >= 6 Then
            s_GetSwitchDescription = "Dew point calculation in Celsius"
        ElseIf id = 6 And CShort(numSwitch) >= 7 Then
            s_GetSwitchDescription = "Voltage at input in Volts"
        ElseIf id = 7 And CShort(numSwitch) >= 8 Then
            s_GetSwitchDescription = "Current used by system in Amps"
        ElseIf id = 8 And CShort(numSwitch) >= 9 Then
            s_GetSwitchDescription = "Momentary power usage in Watts"
        ElseIf id = 9 And CShort(numSwitch) >= 10 Then
            s_GetSwitchDescription = "Total power consumption since connected in Watts*Hour"
        End If
        Return s_GetSwitchDescription
    End Function

    ''' <summary>
    ''' Reports if the specified switch can be written to.
    ''' This is false if the switch cannot be written to, for example a limit switch or a sensor.
    ''' </summary>
    ''' <param name="id">The number of the switch whose write state is to be returned</param>
    ''' <returns>
    ''' <c>true</c> if the switch can be set, otherwise <c>false</c>.
    ''' </returns>
    ''' <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
    ''' <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
    Public Function CanWrite(id As Short) As Boolean Implements ISwitchV2.CanWrite
        Validate("CanWrite", id)
        Dim PortCanWrite As Boolean
        If id = 0 And CShort(numSwitch) >= 1 Then
            PortCanWrite = True
        ElseIf id = 1 And CShort(numSwitch) >= 2 Then
            PortCanWrite = True
        ElseIf id = 2 And CShort(numSwitch) >= 3 Then
            PortCanWrite = True
        ElseIf id = 3 And CShort(numSwitch) >= 4 Then
            PortCanWrite = False
        ElseIf id = 4 And CShort(numSwitch) >= 5 Then
            PortCanWrite = False
        ElseIf id = 5 And CShort(numSwitch) >= 6 Then
            PortCanWrite = False
        ElseIf id = 6 And CShort(numSwitch) >= 7 Then
            PortCanWrite = False
        ElseIf id = 7 And CShort(numSwitch) >= 8 Then
            PortCanWrite = False
        ElseIf id = 8 And CShort(numSwitch) >= 9 Then
            PortCanWrite = False
        ElseIf id = 9 And CShort(numSwitch) >= 10 Then
            PortCanWrite = False
        End If
        TL.LogMessage("CanWrite to Port ", id.ToString(), PortCanWrite)
        Return PortCanWrite
    End Function

#Region "boolean members"
    ''' <summary>
    ''' Return the state of switch n as a boolean.
    ''' A multi-value switch must throw a MethodNotImplementedException.
    ''' </summary>
    ''' <param name="id">The switch number to return</param>
    ''' <returns>True or false</returns>
    Function GetSwitch(id As Short) As Boolean Implements ISwitchV2.GetSwitch
        Validate("GetSwitch", 0, True)
        Dim DCJackState As String = ""
        Dim ValueState As Double = 0
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Switch"
            If id = 0 And CShort(numSwitch) >= 1 Then
                objSerial.Transmit("GETSTATUSDCJACK#")
                DCJackState = objSerial.ReceiveTerminated("#")
                ValueState = Replace(DCJackState, "#", "")
                If ValueState = 1 Then
                    TL.LogMessage("GetSwitch " + id.ToString(), "True")
                    Return True
                Else
                    TL.LogMessage("GetSwitch " + id.ToString(), "False")
                    Return False
                End If
            End If
        End Using
    End Function

    ''' <summary>
    ''' Sets a switch to the specified state, true or false.
    ''' If the switch cannot be set then throws a MethodNotImplementedException.
    ''' </summary>
    ''' <param name="ID">The number of the switch to set</param>
    ''' <param name="State">The required switch state</param>
    Sub SetSwitch(id As Short, state As Boolean) Implements ISwitchV2.SetSwitch
        Validate("SetSwitch", id, True)
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Switch"
            Dim numSetSwitch As String = ""
            If state = False Then
                TL.LogMessage("SetSwitch", id.ToString(), state.ToString())
                If id = 0 And CShort(numSwitch) >= 1 Then
                    SwitchState0 = "OFF"
                    driverProfile.WriteValue(driverID, SwitchState0ProfileName, SwitchState0.ToString(), "DC Jacks")
                    numSetSwitch = "SETSTATUSDCJACK_OFF#"
                Else
                    TL.LogMessage("SetSwitch" + id.ToString(), "Invalid Value")
                    Throw New InvalidValueException("SetSwitch", id.ToString(), String.Format("0 to {0}", numSwitches - 1))
                End If
            ElseIf state = True Then
                TL.LogMessage("SetSwitch", id.ToString(), state.ToString())
                If id = 0 And CShort(numSwitch) >= 1 Then
                    SwitchState0 = "ON"
                    driverProfile.WriteValue(driverID, SwitchState0ProfileName, SwitchState0.ToString(), "DC Jacks")
                    numSetSwitch = "SETSTATUSDCJACK_ON#"
                Else
                    TL.LogMessage("SetSwitch" + id.ToString(), "Invalid Value")
                    Throw New InvalidValueException("SetSwitch", id.ToString(), String.Format("0 to {0}", numSwitches - 1))
                End If
            Else
                TL.LogMessage("SetSwitch", "Not Implemented")
                Throw New ASCOM.MethodNotImplementedException("SetSwitch")
            End If
            objSerial.Transmit(numSetSwitch)
            objSerial.ReceiveTerminated("#")
        End Using
    End Sub

#End Region

#Region "Analogue members"
    ''' <summary>
    ''' Returns the maximum analogue value for this switch
    ''' Boolean switches must return 1.0
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function MaxSwitchValue(id As Short) As Double Implements ISwitchV2.MaxSwitchValue
        Validate("MaxSwitchValue", id)
        Dim MaxValue As Double
        If id = 0 And CShort(numSwitch) >= 1 Then
            MaxValue = 1
        ElseIf id = 1 And CShort(numSwitch) >= 2 Then
            MaxValue = 100
        ElseIf id = 2 And CShort(numSwitch) >= 3 Then
            MaxValue = 100
        ElseIf id = 3 And CShort(numSwitch) >= 4 Then
            MaxValue = 100
        ElseIf id = 4 And CShort(numSwitch) >= 5 Then
            MaxValue = 100
        ElseIf id = 5 And CShort(numSwitch) >= 6 Then
            MaxValue = 100
        ElseIf id = 6 And CShort(numSwitch) >= 7 Then
            MaxValue = 30
        ElseIf id = 7 And CShort(numSwitch) >= 8 Then
            MaxValue = 10
        ElseIf id = 8 And CShort(numSwitch) >= 9 Then
            MaxValue = 250
        ElseIf id = 9 And CShort(numSwitch) >= 10 Then
            MaxValue = 2500
        End If
        TL.LogMessage("MaxSwitchValue ", id.ToString(), MaxValue)
        Return MaxValue
    End Function

    ''' <summary>
    ''' Returns the minimum analogue value for this switch
    ''' Boolean switches must return 0.0
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function MinSwitchValue(id As Short) As Double Implements ISwitchV2.MinSwitchValue
        Validate("MinSwitchValue", id)
        Dim MinValue As Double
        If id = 0 And CShort(numSwitch) >= 1 Then
            MinValue = 0
        ElseIf id = 1 And CShort(numSwitch) >= 2 Then
            MinValue = 0
        ElseIf id = 2 And CShort(numSwitch) >= 3 Then
            MinValue = 0
        ElseIf id = 3 And CShort(numSwitch) >= 4 Then
            MinValue = -50
        ElseIf id = 4 And CShort(numSwitch) >= 5 Then
            MinValue = 0
        ElseIf id = 5 And CShort(numSwitch) >= 6 Then
            MinValue = -50
        ElseIf id = 6 And CShort(numSwitch) >= 7 Then
            MinValue = 0
        ElseIf id = 7 And CShort(numSwitch) >= 8 Then
            MinValue = 0
        ElseIf id = 8 And CShort(numSwitch) >= 9 Then
            MinValue = 0
        ElseIf id = 9 And CShort(numSwitch) >= 10 Then
            MinValue = 0
        End If
        TL.LogMessage("MinSwitchValue ", id.ToString(), MinValue)
        Return MinValue
    End Function

    ''' <summary>
    ''' returns the step size that this switch supports. This gives the difference between successive values of the switch.
    ''' The number of values is ((MaxSwitchValue - MinSwitchValue) / SwitchStep) + 1
    ''' boolean switches must return 1.0, giving two states.
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function SwitchStep(id As Short) As Double Implements ISwitchV2.SwitchStep
        Validate("SwitchStep", id)
        Dim SwitchStepValue As Double
        If id = 0 And CShort(numSwitch) >= 1 Then
            SwitchStepValue = 1
        ElseIf id = 1 And CShort(numSwitch) >= 2 Then
            SwitchStepValue = 20
        ElseIf id = 2 And CShort(numSwitch) >= 3 Then
            SwitchStepValue = 20
        ElseIf id = 3 And CShort(numSwitch) >= 4 Then
            SwitchStepValue = 0.01
        ElseIf id = 4 And CShort(numSwitch) >= 5 Then
            SwitchStepValue = 0.1
        ElseIf id = 5 And CShort(numSwitch) >= 6 Then
            SwitchStepValue = 0.01
        ElseIf id = 6 And CShort(numSwitch) >= 7 Then
            SwitchStepValue = 0.01
        ElseIf id = 7 And CShort(numSwitch) >= 8 Then
            SwitchStepValue = 0.01
        ElseIf id = 8 And CShort(numSwitch) >= 9 Then
            SwitchStepValue = 0.01
        ElseIf id = 9 And CShort(numSwitch) >= 10 Then
            SwitchStepValue = 0.01
        End If
        TL.LogMessage("SwitchStep ", id.ToString(), SwitchStepValue)
        Return SwitchStepValue
    End Function

    ''' <summary>
    ''' Returns the analogue switch value for switch id
    ''' Boolean switches must throw a MethodNotImplementedException
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function GetSwitchValue(id As Short) As Double Implements ISwitchV2.GetSwitchValue
        Validate("GetSwitchValue", id, False)
        Dim SerialRead As String = ""
        Dim ReturnValue As Double
        If id = 1 And CShort(numSwitch) >= 2 Then
            objSerial.Transmit("GETSTATUSPWM1#")
            SerialRead = objSerial.ReceiveTerminated("#")
        ElseIf id = 2 And CShort(numSwitch) >= 3 Then
            objSerial.Transmit("GETSTATUSPWM2#")
            SerialRead = objSerial.ReceiveTerminated("#")
        ElseIf id = 3 And CShort(numSwitch) >= 4 Then
            objSerial.Transmit("GETTEMPERATURE#")
            SerialRead = objSerial.ReceiveTerminated("#")
        ElseIf id = 4 And CShort(numSwitch) >= 5 Then
            objSerial.Transmit("GETHUMIDITY#")
            SerialRead = objSerial.ReceiveTerminated("#")
        ElseIf id = 5 And CShort(numSwitch) >= 6 Then
            objSerial.Transmit("GETDEWPOINT#")
            SerialRead = objSerial.ReceiveTerminated("#")
        ElseIf id = 6 And CShort(numSwitch) >= 7 Then
            objSerial.Transmit("GETVOLTAGE#")
            SerialRead = objSerial.ReceiveTerminated("#")
        ElseIf id = 7 And CShort(numSwitch) >= 8 Then
            objSerial.Transmit("GETCURRENT#")
            SerialRead = objSerial.ReceiveTerminated("#")
        ElseIf id = 8 And CShort(numSwitch) >= 9 Then
            objSerial.Transmit("GETPOWER#")
            SerialRead = objSerial.ReceiveTerminated("#")
        ElseIf id = 9 And CShort(numSwitch) >= 10 Then
            objSerial.Transmit("GETUSAGE#")
            SerialRead = objSerial.ReceiveTerminated("#")
        Else
            TL.LogMessage("GetSwitchValue", "Not Implemented")
            Throw New MethodNotImplementedException("GetSwitchValue")
        End If
        ReturnValue = Replace(SerialRead, "#", "")
        TL.LogMessage("GetSwitchValue ", id.ToString(), ReturnValue)
        Return ReturnValue
    End Function

    ''' <summary>
    ''' Set the analogue value for this switch.
    ''' A MethodNotImplementedException should be thrown if CanWrite returns False
    ''' If the value is not between the maximum and minimum then throws an InvalidValueException
    ''' boolean switches must throw a MethodNotImplementedException
    ''' </summary>
    ''' <param name="id"></param>
    ''' <param name="value"></param>
    Sub SetSwitchValue(id As Short, value As Double) Implements ISwitchV2.SetSwitchValue
        Validate("SetSwitchValue", id, value)
        Dim SwitchValue As String = ""
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Switch"
            If id = 1 And CShort(numSwitch) >= 2 Then
                TL.LogMessage("SetSwitchValue ", id.ToString(), value)
                SwitchValue = ("SETSTATUSPWM1_" + value.ToString() + "#")
                objSerial.Transmit(SwitchValue)
                objSerial.ReceiveTerminated("#")
            ElseIf id = 2 And CShort(numSwitch) >= 3 Then
                TL.LogMessage("SetSwitchValue ", id.ToString(), value)
                SwitchValue = ("SETSTATUSPWM2_" + value.ToString() + "#")
                objSerial.Transmit(SwitchValue)
                objSerial.ReceiveTerminated("#")
            ElseIf value < MinSwitchValue(id) Or value > MaxSwitchValue(id) Then
                Throw New InvalidValueException("", value.ToString(), String.Format("{0} to {1}", MinSwitchValue(id), MaxSwitchValue(id)))
            End If
        End Using
    End Sub

#End Region

#End Region

#Region "Private methods"

    ''' <summary>
    ''' Checks that the switch id is in range and throws an InvalidValueException if it isn't
    ''' </summary>
    ''' <param name="message">The message.</param>
    ''' <param name="id">The id.</param>
    Private Sub Validate(message As String, id As Short)
        If (id < 0 Or id >= numSwitches) Then
            Throw New InvalidValueException(message, id.ToString(), String.Format("0 to {0}", numSwitches - 1))
        End If
    End Sub

    ''' <summary>
    ''' Checks that the number of states for the switch is correct and throws a methodNotImplemented exception if not.
    ''' Boolean switches must have 2 states and multi-value switches more than 2.
    ''' </summary>
    ''' <param name="message"></param>
    ''' <param name="id"></param>
    ''' <param name="expectBoolean"></param>
    Private Sub Validate(message As String, id As Short, expectBoolean As Boolean)
        Validate(message, id)
        Dim ns As Integer = (((MaxSwitchValue(id) - MinSwitchValue(id)) / SwitchStep(id)) + 1)
        If (expectBoolean And ns <> 2) Or (Not expectBoolean And ns <= 2) Then
            TL.LogMessage(message, String.Format("Switch {0} has the wrong number of states", id, ns))
            Throw New MethodNotImplementedException(String.Format("{0}({1})", message, id))
        End If
    End Sub

    ''' <summary>
    ''' Checks that the switch id and value are in range and throws an
    ''' InvalidValueException if they are not.
    ''' </summary>
    ''' <param name="message">The message.</param>
    ''' <param name="id">The id.</param>
    ''' <param name="value">The value.</param>
    Private Sub Validate(message As String, id As Short, value As Double)
        Validate(message, id, False)
        Dim min = MinSwitchValue(id)
        Dim max = MaxSwitchValue(id)
        If (value < min Or value > max) Then
            TL.LogMessage(message, String.Format("Value {1} for Switch {0} is out of the allowed range {2} to {3}", id, value, min, max))
            Throw New InvalidValueException(message, value.ToString(), String.Format("Switch({0}) range {1} to {2}", id, min, max))
        End If
    End Sub
#End Region

#Region "Private properties and methods"
    ' here are some useful properties and methods that can be used as required
    ' to help with

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Using P As New Profile() With {.DeviceType = "Switch"}
            If bRegister Then
                P.Register(driverID, driverDescription)
            Else
                P.Unregister(driverID)
            End If
        End Using

    End Sub

    <ComRegisterFunction()>
    Public Shared Sub RegisterASCOM(ByVal T As Type)

        RegUnregASCOM(True)

    End Sub

    <ComUnregisterFunction()>
    Public Shared Sub UnregisterASCOM(ByVal T As Type)

        RegUnregASCOM(False)

    End Sub

#End Region

    ''' <summary>
    ''' Returns true if there is a valid connection to the driver hardware
    ''' </summary>
    Private ReadOnly Property IsConnected As Boolean
        Get
            ' TODO check that the driver hardware connection exists and is connected to the hardware
            If Not objSerial Is Nothing Then
                If objSerial.Connected Then
                    connectedState = True
                Else
                    connectedState = False
                End If
            Else
                connectedState = False
            End If
            Return connectedState
        End Get
    End Property

    ''' <summary>
    ''' Use this function to throw an exception if we aren't connected to the hardware
    ''' </summary>
    ''' <param name="message"></param>
    Private Sub CheckConnected(ByVal message As String)
        If Not IsConnected Then
            Throw New NotConnectedException(message)
        End If
    End Sub

    Friend Sub ReadProfile()
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Switch"
            traceState = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, String.Empty, traceStateDefault))
            comPort = driverProfile.GetValue(driverID, comPortProfileName, String.Empty, comPortDefault)
            ConnectionDelay = driverProfile.GetValue(driverID, ConnectionDelayProfileName, String.Empty, ConnectionDelayDefault)
            numSwitch = driverProfile.GetValue(driverID, numSwitchProfileName, String.Empty, numSwitchDefault)
            SwitchName0 = driverProfile.GetValue(driverID, SwitchName0ProfileName, "DC Jacks", SwitchName0Default)
            SwitchState0 = driverProfile.GetValue(driverID, SwitchState0ProfileName, "DC Jacks", SwitchState0Default)
            SwitchName1 = driverProfile.GetValue(driverID, SwitchName1ProfileName, "PWM 1 - Main", SwitchName1Default)
            SwitchState1 = driverProfile.GetValue(driverID, SwitchState1ProfileName, "PWM 1 - Main", SwitchState1Default)
            SwitchName2 = driverProfile.GetValue(driverID, SwitchName2ProfileName, "PWM 2 - Guide", SwitchName2Default)
            SwitchState2 = driverProfile.GetValue(driverID, SwitchState2ProfileName, "PWM 2 - Guide", SwitchState2Default)
            SwitchName3 = driverProfile.GetValue(driverID, SwitchName3ProfileName, "Temperature sensor", SwitchName3Default)
            SwitchState3 = driverProfile.GetValue(driverID, SwitchState3ProfileName, "Temperature sensor", SwitchState3Default)
            SwitchName4 = driverProfile.GetValue(driverID, SwitchName4ProfileName, "Humidity sensor", SwitchName4Default)
            SwitchState4 = driverProfile.GetValue(driverID, SwitchState4ProfileName, "Humidity sensor", SwitchState4Default)
            SwitchName5 = driverProfile.GetValue(driverID, SwitchName5ProfileName, "Dew point sensor", SwitchName5Default)
            SwitchState5 = driverProfile.GetValue(driverID, SwitchState5ProfileName, "Dew point sensor", SwitchState5Default)
            SwitchName6 = driverProfile.GetValue(driverID, SwitchName6ProfileName, "Voltage sensor", SwitchName6Default)
            SwitchState6 = driverProfile.GetValue(driverID, SwitchState6ProfileName, "Voltage sensor", SwitchState6Default)
            SwitchName7 = driverProfile.GetValue(driverID, SwitchName7ProfileName, "Current sensor", SwitchName7Default)
            SwitchState7 = driverProfile.GetValue(driverID, SwitchState7ProfileName, "Current sensor", SwitchState7Default)
            SwitchName8 = driverProfile.GetValue(driverID, SwitchName8ProfileName, "Power sensor", SwitchName8Default)
            SwitchState8 = driverProfile.GetValue(driverID, SwitchState8ProfileName, "Power sensor", SwitchState8Default)
            SwitchName9 = driverProfile.GetValue(driverID, SwitchName9ProfileName, "Total power usage", SwitchName9Default)
            SwitchState9 = driverProfile.GetValue(driverID, SwitchState9ProfileName, "Total power usage", SwitchState9Default)
        End Using
    End Sub

    ''' <summary>
    ''' Write the device configuration to the  ASCOM  Profile store
    ''' </summary>
    Friend Sub WriteProfile()
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Switch"
            driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString())
            If comPort IsNot Nothing Then
                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString())
            End If
            driverProfile.WriteValue(driverID, ConnectionDelayProfileName, ConnectionDelay.ToString())
            driverProfile.WriteValue(driverID, numSwitchProfileName, numSwitch.ToString())
            driverProfile.WriteValue(driverID, SwitchName0ProfileName, SwitchName0.ToString(), "DC Jacks")
            driverProfile.WriteValue(driverID, SwitchState0ProfileName, SwitchState0.ToString(), "DC Jacks")
            driverProfile.WriteValue(driverID, SwitchName1ProfileName, SwitchName1.ToString(), "PWM 1 - Main")
            driverProfile.WriteValue(driverID, SwitchState1ProfileName, SwitchState1.ToString(), "PWM 1 - Main")
            driverProfile.WriteValue(driverID, SwitchName2ProfileName, SwitchName2.ToString(), "PWM 2 - Guide")
            driverProfile.WriteValue(driverID, SwitchState2ProfileName, SwitchState2.ToString(), "PWM 2 - Guide")
            driverProfile.WriteValue(driverID, SwitchName3ProfileName, SwitchName3.ToString(), "Temperature sensor")
            driverProfile.WriteValue(driverID, SwitchState3ProfileName, SwitchState3.ToString(), "Temperature sensor")
            driverProfile.WriteValue(driverID, SwitchName4ProfileName, SwitchName4.ToString(), "Humidity sensor")
            driverProfile.WriteValue(driverID, SwitchState4ProfileName, SwitchState4.ToString(), "Humidity sensor")
            driverProfile.WriteValue(driverID, SwitchName5ProfileName, SwitchName5.ToString(), "Dew point sensor")
            driverProfile.WriteValue(driverID, SwitchState5ProfileName, SwitchState5.ToString(), "Dew point sensor")
            driverProfile.WriteValue(driverID, SwitchName6ProfileName, SwitchName6.ToString(), "Voltage sensor")
            driverProfile.WriteValue(driverID, SwitchState6ProfileName, SwitchState6.ToString(), "Voltage sensor")
            driverProfile.WriteValue(driverID, SwitchName7ProfileName, SwitchName7.ToString(), "Current sensor")
            driverProfile.WriteValue(driverID, SwitchState7ProfileName, SwitchState7.ToString(), "Current sensor")
            driverProfile.WriteValue(driverID, SwitchName8ProfileName, SwitchName8.ToString(), "Power sensor")
            driverProfile.WriteValue(driverID, SwitchState8ProfileName, SwitchState8.ToString(), "Power sensor")
            driverProfile.WriteValue(driverID, SwitchName9ProfileName, SwitchName9.ToString(), "Total power usage")
            driverProfile.WriteValue(driverID, SwitchState9ProfileName, SwitchState9.ToString(), "Total power usage")
        End Using
    End Sub

#End Region

End Class
