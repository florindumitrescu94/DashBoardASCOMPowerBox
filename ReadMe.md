<!DOCTYPE html>
<html>
<body>
	        I have used wittinobi's driver as inspiration and to understand how an ASCOM driver works, so many thanks to Tobias Wittmann for making this driver available! 
<br>
	        All the folders and the assembly name is still wittinobi's defaults, since I didn't manage to change them to my own without breaking the functionality.
<br>
	<br>
	        <br> 
	        After searching online and only finding wittinobi's project as a DIY powerbox solution, I started this project, called DashBoard (since it contains most controls and information about the power delivery and environment, akin to a car dashboard). <br>
	        This is a fully open project and anyone can replicate and modify the arduino code or the driver to accomodate their needs. <br>
		This driver contains code to read the environmental sensor (DHT22), control two PWM outputs for dew heaters, measure and display power consumption stats (voltage, current, power) and control one relay (connected to four DC Jacks, in my case. Arduino code is in the INO folder. <br>
		<br>
		<br>
		The schematic is listed below.
		The chip on the left is the current sensing ACS712. I have used a 20A flavour of this chip. 
  		<br>
		There are three variants of this chip: 5A, 20A and 30A. If you use a different one, the Arduino code needs to be modified as follows: <br>
	<br>
	<br>
		 <table>
  <tr>
    <th>Variant</th>
    <th>Resolution</th>
    <th>Arduino Code</th>
  </tr>
  <tr>
    <td>5A</td>
    <td>185mV/A</td>
    <td>AMP_AVERAGE[AVERAGE_COUNT] = ((2.494 - ((CURRENT_SAMPLE_SUM/150)*(5.0/1024.0)))*-1) / 0.185;</td>
  </tr>
  <tr>
    <td>20A</td>
    <td>100mV/A</td>
    <td>AMP_AVERAGE[AVERAGE_COUNT] = ((2.494 - ((CURRENT_SAMPLE_SUM/150)*(5.0/1024.0)))*-1) / 0.10;</td>
  </tr>
    <tr>
    <td>30A</td>
    <td>66mV/A</td>
    <td>AMP_AVERAGE[AVERAGE_COUNT] = ((2.494 - ((CURRENT_SAMPLE_SUM/150)*(5.0/1024.0)))*-1) / 0.066;</td>
  </tr>
</table> 
	<br>
		<br>
		The voltage sensing is done through a voltage divider (so the input voltage can be brought down to safe levels for an Arduino ADC input). The voltmeter has to be tuned in code based on your resistor ratio. <br>
		An Arduino Nano has a 10 bit ADC (1024 values) and the input values are between 0 and 5V. So, 5V divided by the ADC resolution / 1024 is roughly 5mV per ADC Unit (0.00488V / ADC unit)<br>
		To get the equivalent voltage at the analog input, we multiply the values in ADC Units at PIN_VALUE_V (connected to the voltage divider) by 5, then divide it by 1024.0 (you can also multiply it by 0.0048, but this equation is a bit more precise).<br> <br>
		As we have a 100k and 10k voltage divider that provides the input for the analog pin, we need to divide the value obtained above by this ration: 10k/(10k+100k). <b>As no resistor is perfect, please measure your resistors and replace the 10k and 100k values in this equation.</b> <br>
		With my resistors, that value comes around 0.092. Replace the 0.092 value in the code with yours.
	        <br>
	        <br>
                <br>
		VOLT_TEMP = (PIN_VALUE_V * 5.0) / 1024.0;  <br> 
                VOLT = VOLT_TEMP / <b>>0.092</b>
	        <br>
	        <br>
	        The total power consumption is calculated by measuring the time it takes to run the GET_POWER Function, then multiplying the PWR parameter with the time difference in hours (time2-time1)*3600000)<br>
	        PWR_TOTAL=PWR_TOTAL+(PWR*((time2-time1)/3600000));<br><br><br>
		<img src="https://github.com/florindumitrescu94/DashBoardASCOMPowerBox/assets/16653100/1a9b26e9-b54e-47ae-b7e0-ee424dd57b13"></img>
                <br>
		<br>
	        D1 Diode is used as a flyback diode for the relay coil and D2 diode is a reverse voltage protection diode <br>
	        <br>
	        You can use the same 5V relay, supplied by the Arduino, or you can chose a 12V relay and supply the coil voltage directly from the 12V input, if your USB cannot supply the required current.<br> 
	        The JQC-3FF-S-Z 5V Relay I have used has a 66 ohms coild and uses up around 72mA when switched on. An Arduino is more than capable of supplying that voltage, but it might pose some problems with an unpowered USB2 hub.<br>
	        I've used it on an unpowered USB3 hub and didn't have any trobule keeping things running. <br>
	<br>
	<br>
	<br>
	        A known issue for me is that, whenever the power supply is plugged in AFTER the USB has been connected to the PC, the connection drops and I have to either restart the arduino or re-plug the USB cable. <br> 
	        This is, most probably, due to the C3 470uF capacitor in the schematic, since I have not encountered this problem without it. If you know your power supply is well filtered, you can skip the capacitor. <br>
                <br>
	        NINA Menu shown below:<br>
	        As of NINA 2.x, the values in the Gauge portions cannot be used inside the Sequencer. However, with Nina 3.x comes a new plugin called Sequence Powerups <url>https://marcblank.github.io/</url>. Using this plugin, you can create functions based on values avaliable on your devices. <br>
                Therefore, you can, say, automate dew heaters based on dew point or humidity using  the conditional functions in this plugin. <br>
		<br>
		<img src="https://github.com/florindumitrescu94/DashBoardASCOMPowerBox/assets/16653100/b05b2a44-b6aa-48bc-8d2d-8836e7c5a1fd"></img>
                <br>
		<img src="https://github.com/florindumitrescu94/DashBoardASCOMPowerBox/assets/16653100/fb170d53-3f2e-4bbd-a954-7fa60f2098cf"></img>
  <br>
  <br>
  <br>
		<br>
                <br>
		

                



</html>
