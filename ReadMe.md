<!DOCTYPE html>
<html>
	<header>
		<h1> WORK IN PROGRESS WORK IN PROGRESS <br>
			This document is a work in progress and it may not contain all the necessary information
		</h1>
	<header>
		<h1>DashBoard </h1>
		<h2>An ASCOM connected powerbox with PWM controlls, environmental sensors and current measurements built with an Arduino Nano microcrontroller board</h2>
                <p>
		I wanted to solve a problem that I have faced whenever I left my telescope to shoot a target overnight: I want to turn off the power to my equipment after the NINA sequence.<br>
		I don't want to have the dew heaters and mount running until the morning, when I go out and turn them off.<br>
		The best solution would be to have an ASCOM controlled powerbox, so that I can add instructions to turn off the power inside a NINA sequence. <br>
		Of course, there are readily build options out there, like PegasusAstro PocketPower, or integrated inside ASIAIR or EagleLab computers, but I already have a mini-pc that runs wonderfully and I didn't want to either spend a lot for a power box, or swap my computer for a new system.<br>
	        After searching online and only finding wittinobi's project as a DIY powerbox solution, I started this project, called DashBoard (since it contains most controls and information about the power delivery and environment, akin to a car dashboard). <br>
	        This is a fully open project and anyone can replicate and modify the arduino code or the driver to accomodate their needs.
		<br><br><br>
		I will describe this project in the following document.
		</p>
	</header>
	<header>
		<h2>
			Special notes
		</h2>
		<p>I have used wittinobi's driver as inspiration and to understand how an ASCOM driver works, so many thanks to Tobias Wittmann for making this driver available! <br>
	        All the folders and the assembly name is still wittinobi's defaults, since I didn't manage to change them to my own without breaking the functionality.<br><br>
			This is a very young project and has its' shortcomings. I intend on expanding and enhancing it, based on feedback or new ideas, which are all very welcome!<br>
		<h2>
			Contents:
		</h2>
		<p>
			Project description<br>
			Requirements<br>
			Diagram<br>
			Schematic<br>
			Arduino code<br>
			Software control<br>
			Known Issues<br>
			Future improvements<br>
		</p>
	</header>

 <header>
<h2>
	Project description
</h2>
<p>
	This project is meant to create a device capable of controling the power delivery to astrophotography devices (mount, camera, dew heaters, etc.), as well as monitoring the environmental conditions and power consumption.<br> In order to automate an astrophotography sessions, this control and monitoring needs to happen through ASCOM, so that we can use instructions in NINA to turn the power on or off and increase or decrease the power to the dew heaters, as well as use the information we have gathered from the sensors as triggers. <br><br>
	To acheive that, we will require:<br><br><br>
 <b>-4 x 12V Output DC jacks controlled by one relay</b><br>
 <b>-Two RCA PWM controlled via two MOSFETS for the dew heaters</b><br>
 <b>-A DHT22 sensor that measures the outside temperature and humidity conditions</b><br>
 <b>-An ACS712 current sensor to measure the current and power consumption</b><br>
	<b>-An Arduino Nano microcontroller board</b>
	The Arduino will be the connection between the hardware and software and it will communicate with the ASCOM driver via serial commands.<br>
	Both the Arduino and the ASCOM driver will listen and send commands over serial and will execute functions based on said commands. The ASCOM Driver will have some pre-determined function names that will be called by an application like N.I.N.A. (GetSwitch, for example, to check the state of a boolean switch). The functions in the Arduino code can be fully customized, as long as they send the correct data back via serial.<br>
</p>
 </header>
 <header>
	 <h2>Requrements</h2>
	 <p>
		 This project will require the following tools, parts and software:<br><br>
			 <b>PARTS:</b><br>
    			-Arduino NANO<br>
       			-ACS712 (5A/20A/30A, *NOTE1)<br>
	  		-DHT22 sensor module<br>
     			-5V relay (JQK-3FF-S-Z-5V)<br>
			-1 x 1nF SMD capacitor<br>
   			-1 x 100nF SMD capacitor<br>
      			-1 x 470uF TH capacitor( *NOTE2)<br>
			-2 x IR2905 N-MOSFET transistors<br>
   			-3 x 10k SMD resistors<br>
      			-1 x 100k SMD resistor<br>
	 		-2 x 100 SMD resistors<br>
    			-1 x 470-510 SMD resistor<br>
			-1 x S8050(J3Y) SMD transistor<br>
       			-1 x 1N4007 TH diode<br>
	  		-1 x 1N4148 SMD diode<br>
     			-at least two DC Jack (PCB or Panel mount, based on choice), one for 12V input and at least one for 12V output.<br>
			-2 x RCA Plugs<br>
		 	-1 x at least 10cm by 10cm circuit board for etching, or perfboard.<br>
		 	If you chose a perfboard, go with through hole components. It's very difficult to solder SMD parts to perf boards.<br>
		 	Also, if you chose panel mounted jacks, you will need some wires. I find that, for the higher current needs, speaker wire, or PC power supply wires work perfectly. For low current, any wires would do. <br><br><br>
		 	<b>TOOLS:</b><br><br>
		 	-Soldering station<br>
		 	-ESS Safe Tweezers<br>
		 	-Rosin-core solder (the thinner, the better, 1mm or smaller)<br>
		 	-Flux<br>
		 	-Helping hands or electronics vise<br>
	 </p>
 </header>
	<br><b>NOTE! I have not modified the APP for this version, since I intend to only use it inside of NINA. If you want to use a Windows app, you will need to modify the code for that app (It's not compatible with my driver)</b>
<br>
	<br>
	        <br> 
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
