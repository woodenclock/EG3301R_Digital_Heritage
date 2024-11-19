// Define diffuser pins
const int diffuser1 = 5;
const int diffuser2 = 6;
const int diffuser3 = 9;
const int diffuser4 = 10;

// Store the state and timer for each diffuser
bool diffuser1State = false;
bool diffuser2State = false;
bool diffuser3State = false;
bool diffuser4State = false;

unsigned long diffuser1StartTime = 0;
unsigned long diffuser2StartTime = 0;
unsigned long diffuser3StartTime = 0;
unsigned long diffuser4StartTime = 0;

const unsigned long duration = 2000; // Duration for each diffuser to stay on (2 seconds)
const int delayTime =100;            // 50 millisecond


void setup() {
  Serial.begin(9600);

  pinMode(diffuser1, OUTPUT);
  pinMode(diffuser2, OUTPUT);
  pinMode(diffuser3, OUTPUT);
  pinMode(diffuser4, OUTPUT);

  digitalWrite(diffuser1, HIGH);
  digitalWrite(diffuser2, HIGH);
  digitalWrite(diffuser3, HIGH);
  digitalWrite(diffuser4, HIGH);
}


void loop() {
  if (Serial.available() > 0) {
    int diffuserNumber = Serial.parseInt();
    
    activateDiffuser(diffuserNumber);   // Activate the diffuser based on the received number
  }

  updateDiffuserStates();               // Check each diffuser to see if the duration has passed, and turn it off if needed
}


void activateDiffuser(int diffuserNumber) {
  // Activate the diffuser and start its timer based on the received number
  switch (diffuserNumber) {
    case 6:
      diffuser1State = true;
      diffuser1StartTime = millis(); // Record the start time
      digitalWrite(diffuser1, LOW);
      Serial.print("Activating diffuser 1 at ");
      Serial.println(diffuser1StartTime);
      break;
    case 7:
      diffuser2State = true;
      diffuser2StartTime = millis();
      digitalWrite(diffuser2, LOW);
      Serial.print("Activating diffuser 2 at ");
      Serial.println(diffuser2StartTime);
      break;
    case 8:
      diffuser3State = true;
      diffuser3StartTime = millis();
      digitalWrite(diffuser3, LOW);
      Serial.print("Activating diffuser 3 at ");
      Serial.println(diffuser3StartTime);
      break;
    case 9:
      diffuser4State = true;
      diffuser4StartTime = millis();
      digitalWrite(diffuser4, LOW);
      Serial.print("Activating diffuser 4 at ");
      Serial.println(diffuser4StartTime);
      break;
    default:
      Serial.println("Unknown diffuser number");
      break;
  }
}


void updateDiffuserStates() {
  // Turn off diffusers after 2 seconds if they are on

  if (diffuser1State && (millis() - diffuser1StartTime >= duration)) {
    diffuser1State = false; // Turn off diffuser 1
    digitalWrite(diffuser1, LOW);
    delay(delayTime);
    digitalWrite(diffuser1, HIGH);
    delay(delayTime);
    digitalWrite(diffuser1, LOW);
    delay(delayTime);
    digitalWrite(diffuser1, HIGH);
    delay(delayTime);
    digitalWrite(diffuser1, LOW);
    delay(delayTime);
    digitalWrite(diffuser1, HIGH);
    Serial.println("Diffuser 1 turned off");
  }

  if (diffuser2State && (millis() - diffuser2StartTime >= duration)) {
    diffuser2State = false; // Turn off diffuser 2
    digitalWrite(diffuser2, LOW);
    delay(delayTime);
    digitalWrite(diffuser2, HIGH);
    delay(delayTime);
    digitalWrite(diffuser2, LOW);
    delay(delayTime);
    digitalWrite(diffuser2, HIGH);
    delay(delayTime);
    digitalWrite(diffuser2, LOW);
    delay(delayTime);
    digitalWrite(diffuser2, HIGH);
    Serial.println("Diffuser 2 turned off");
  }

  if (diffuser3State && (millis() - diffuser3StartTime >= duration)) {
    diffuser3State = false; // Turn off diffuser 3
    digitalWrite(diffuser3, LOW);
    delay(delayTime);
    digitalWrite(diffuser3, HIGH);
    delay(delayTime);
    digitalWrite(diffuser3, LOW);
    delay(delayTime);
    digitalWrite(diffuser3, HIGH);
    delay(delayTime);
    digitalWrite(diffuser3, LOW);
    delay(delayTime);
    digitalWrite(diffuser3, HIGH);
    Serial.println("Diffuser 3 turned off");
  }

  if (diffuser4State && (millis() - diffuser4StartTime >= duration)) {
    diffuser4State = false; // Turn off diffuser 4
    digitalWrite(diffuser4, LOW);
    delay(delayTime);
    digitalWrite(diffuser4, HIGH);
    delay(delayTime);
    digitalWrite(diffuser4, LOW);
    delay(delayTime);
    digitalWrite(diffuser4, HIGH);
    delay(delayTime);
    digitalWrite(diffuser4, LOW);
    delay(delayTime);
    digitalWrite(diffuser4, HIGH);
    Serial.println("Diffuser 4 turned off");
  }
}