/* Written by: Florin Dumitrescu
 *  for ascom_powerbox_and_ambientconditions built with
 *  Arduino NANO V3
 *  January 2024
 */
//INCLUDE LIBRARIES

#include <DHTStable.h>;
DHTStable DHT;


//DECLARE VARIABLES
int DC_JACK_STATE = 0;
int PWM1_STATE = 0;
int PWM1_PWR = 0;
int PWM2_STATE = 0;
int PWM2_PWR = 0;
float TEMP = 0.0;
int HUM_REL = 0;
float HUM_ABS = 0.0;
float DEWPOINT = 0.0;
float VOLT = 0.00;
float VOLT_TEMP = 0.00;
int PIN_VALUE_A = 0;
int PIN_VALUE_V = 0;
float CURRENT_SAMPLE_SUM = 0.0;
int AVERAGE_COUNT = 0;
float AMP_AVERAGE[150];
float AVGAMP;
float AMP = 0.00;
float PWR = 0.00;
long int time1=0;
long int time2=0;
float PWR_TOTAL=0.00;

const int DC_JACK = 4;
const int PWM1 = 5;
const int PWM2 = 6;
const int DHT22_VCC = 7;
const int DHT22_DATA = 8;
const int DHT22_GND = 9;
const int VM = A0;
const int AM = A1;
//ARDUINO INITIALIZATION
void setup()
{
    pinMode(DC_JACK, OUTPUT);
    pinMode(PWM1, OUTPUT);
    pinMode(PWM2, OUTPUT);
    pinMode(DHT22_VCC, OUTPUT);
    pinMode(DHT22_GND, OUTPUT);
    pinMode(DHT22_DATA, INPUT);
    pinMode(VM, INPUT);
    pinMode(AM, INPUT);

    digitalWrite(DC_JACK, LOW);
    digitalWrite(PWM1, LOW);
    digitalWrite(PWM2, LOW);
    digitalWrite(DHT22_VCC, HIGH);
    digitalWrite(DHT22_GND, LOW);
    Serial.begin(9600);
    Serial.flush();
}

//LOOP TO READ SERIAL COMMANDS
void loop()
{

  GET_POWER();
    String cmd;

    if (Serial.available() > 0) {
        cmd = Serial.readStringUntil('#');
        if (cmd == "GETSTATUSDCJACK") 
        {
            Serial.print(DC_JACK_STATE);
            Serial.println("#");
        }
        else if (cmd == "SETSTATUSDCJACK_OFF") DC_JACK_OFF();
        else if (cmd == "SETSTATUSDCJACK_ON") DC_JACK_ON();
        else if (cmd == "GETSTATUSPWM1") 
        {
            Serial.print(PWM1_STATE);
            Serial.println("#");
        }
        else if (cmd == "SETSTATUSPWM1_0") PWM1_POWER_0();
        else if (cmd == "SETSTATUSPWM1_20") PWM1_POWER_1();
        else if (cmd == "SETSTATUSPWM1_40") PWM1_POWER_2();
        else if (cmd == "SETSTATUSPWM1_60") PWM1_POWER_3();
        else if (cmd == "SETSTATUSPWM1_80") PWM1_POWER_4();
        else if (cmd == "SETSTATUSPWM1_100") PWM1_POWER_5();
        else if (cmd == "GETSTATUSPWM2") 
        {
            Serial.print(PWM2_STATE);
            Serial.println("#");
        }
        else if (cmd == "SETSTATUSPWM2_0") PWM2_POWER_0();
        else if (cmd == "SETSTATUSPWM2_20") PWM2_POWER_1();
        else if (cmd == "SETSTATUSPWM2_40") PWM2_POWER_2();
        else if (cmd == "SETSTATUSPWM2_60") PWM2_POWER_3();
        else if (cmd == "SETSTATUSPWM2_80") PWM2_POWER_4();
        else if (cmd == "SETSTATUSPWM2_100")PWM2_POWER_5();
        else if (cmd == "GETTEMPERATURE")
        { 
           GET_AMBIENT();
           Serial.print(TEMP);
           Serial.println("#");
        }
        else if (cmd == "GETHUMIDITY")
        {
           Serial.print(HUM_REL);
           Serial.println("#");
        }
        else if (cmd == "GETDEWPOINT")
        {
           Serial.print(DEWPOINT);
           Serial.println("#");
        }
        else if (cmd == "GETVOLTAGE")
        {
           Serial.print(VOLT);
           Serial.println("#");
        }
        else if (cmd == "GETCURRENT")
        { 
           Serial.print(AMP);
           Serial.println("#");
        }
        else if (cmd == "GETPOWER") 
        {
           Serial.print(PWR);
           Serial.println("#");
        }
        else if (cmd == "GETUSAGE")
        {
           Serial.print(PWR_TOTAL);
           Serial.println("#");
        }
    }
    
//    Serial.print(PIN_VALUE_V);
//    Serial.print(" - ");
//    Serial.print(VOLT);
//    Serial.print(" - ");
//    Serial.print(CURRENT_AVG);
//    Serial.print(" - ");
//    Serial.print(CURRENT_MID);
//    Serial.print(" - ");
//    Serial.print(CURRENT_TOTAL);
//    Serial.print(" = ");
//  Serial.print(AMP);
//    Serial.println();
} 

// END READ SERIAL COMMANDS


//SET/GET FUNCTIONS


//SET DC JACK STATE
  //DC JACK OFF
  void DC_JACK_OFF() {
     digitalWrite(DC_JACK, LOW);
     DC_JACK_STATE = 0;
     Serial.print(DC_JACK_STATE);
     Serial.println("#");
      }

  //DC JACK ON
  void DC_JACK_ON() {
     digitalWrite(DC_JACK, HIGH);
     DC_JACK_STATE = 1;
     Serial.print(DC_JACK_STATE);
     Serial.println("#");
     }
//END SET DC JACK STATE

//SET PWM1 POWER
  //PWM1_OFF
  void PWM1_POWER_0() {
    analogWrite(PWM1, 0);
    PWM1_STATE = 0;
    Serial.print(PWM1_STATE);
    Serial.println("#");
  }
  //PWM1_20
  void PWM1_POWER_1() {
    analogWrite(PWM1, 50);
    PWM1_STATE = 20;
    Serial.print(PWM1_STATE);
    Serial.println("#");
  }
   //PWM1_OFF
  void PWM1_POWER_2() {
    analogWrite(PWM1, 100);
    PWM1_STATE = 40;
    Serial.print(PWM1_STATE);
    Serial.println("#");
  }
   //PWM1_OFF
  void PWM1_POWER_3() {
    analogWrite(PWM1, 150);
    PWM1_STATE = 60;
    Serial.print(PWM1_STATE);
    Serial.println("#");
  }
   //PWM1_OFF
  void PWM1_POWER_4() {
    analogWrite(PWM1, 200);
    PWM1_STATE = 80;
    Serial.print(PWM1_STATE);
    Serial.println("#");
  }
   //PWM1_OFF
  void PWM1_POWER_5() {
    analogWrite(PWM1, 255);
    PWM1_STATE = 100;
    Serial.print(PWM1_STATE);
    Serial.println("#");
  }
//END SET PWM1 POWER


//SET PWM2 POWER
  //PWM2_OFF
  void PWM2_POWER_0() {
    analogWrite(PWM2, 0);
    PWM2_STATE = 0;
    Serial.print(PWM2_STATE);
    Serial.println("#");
  }
  //PWM2_20
  void PWM2_POWER_1() {
    analogWrite(PWM2, 50);
    PWM2_STATE = 20;
    Serial.print(PWM2_STATE);
    Serial.println("#");
  }
   //PWM2_40
  void PWM2_POWER_2() {
    analogWrite(PWM2, 100);
    PWM2_STATE = 40;
    Serial.print(PWM2_STATE);
    Serial.println("#");
  }
   //PWM2_60
  void PWM2_POWER_3() {
    analogWrite(PWM2, 150);
    PWM2_STATE = 60;
    Serial.print(PWM2_STATE);
    Serial.println("#");
  }
   //PWM2_80
  void PWM2_POWER_4() {
    analogWrite(PWM2, 200);
    PWM2_STATE = 80;
    Serial.print(PWM2_STATE);
    Serial.println("#");
  }
   //PWM2_100
  void PWM2_POWER_5() {
    analogWrite(PWM2, 255);
    PWM2_STATE = 100;
    Serial.print(PWM2_STATE);
    Serial.println("#");
  }
//END SET PWM2 POWER

//GET POWER USAGE
  void GET_POWER() {
    time1=millis();
    if (AVERAGE_COUNT == 150)
    {
      AVERAGE_COUNT = 0;
    }
    PIN_VALUE_V = analogRead(VM);     
    VOLT_TEMP = (PIN_VALUE_V * 5.0) / 1024.0;   
    VOLT = VOLT_TEMP / 0.092;        
    if (VOLT < 0.1)   
      {     
        VOLT=0.0;    
      }
    CURRENT_SAMPLE_SUM=0;
    for (int i=0;i<150;i++)
    {
      PIN_VALUE_A= analogRead(AM);
      CURRENT_SAMPLE_SUM = CURRENT_SAMPLE_SUM + PIN_VALUE_A;
    }
    AMP_AVERAGE[AVERAGE_COUNT] = ((2.494 - ((CURRENT_SAMPLE_SUM/150)*(5.0/1024.0)))*-1) / 0.10;
    AVGAMP=0;
    for (int c=0;c<150;c++)
    {
    AVGAMP += AMP_AVERAGE[c];
    }
    AMP = AVGAMP/150;
    if (AMP< 0.01)
    {
      AMP=0.0;
    }
    
    PWR = VOLT * AMP;
    AVERAGE_COUNT = AVERAGE_COUNT + 1;
    time2=millis();
    PWR_TOTAL=PWR_TOTAL+(PWR*((time2-time1)/3600000)); // Calculate total power used each cycle, then add to power usage. since switch has been connected.
}
//END GET POWER USAGE

//GET AMBIENT CONDITIONS
  void GET_AMBIENT() {
  digitalWrite(DHT22_GND,LOW);
  int PIN_VALUE_T = DHT.read22(DHT22_DATA);
   HUM_REL = DHT.getHumidity();
   TEMP = DHT.getTemperature();
   DEWPOINT  = (TEMP - (100 - HUM_REL) / 5);
 }

 