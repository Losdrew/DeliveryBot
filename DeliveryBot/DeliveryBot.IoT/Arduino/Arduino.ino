#include <SoftwareSerial.h>
#include <ArduinoUniqueID.h>
#include <ArduinoJson.h>
#include <Battery.h>
#include <TinyGPS++.h>
#include "robotstatus.h"
#include "commands.h"

const int SPEAKER_PIN = A0;
const int BATTERY_PIN = A2;
const int LID_DANGER_PIN = 4; // Simulates robot lid force opening (power on == lid closed, power off == lid open)
const int START_DELIVERY_PIN = 6; // Simulates delivery start button (power on == pressed, power off == not pressed)

const int DEFAULT_DELAY = 3000;
const int DANGER_DELAY = 500;

SoftwareSerial espSerial(8, 9); // Serial to transfer data to ESP8266 module
SoftwareSerial gpsSerial(11, 12); // Serial to transfer data to GPS module

Battery battery(3000, 4800, BATTERY_PIN);
TinyGPSPlus gps;

String deviceId;

int currentDelay;
RobotStatus currentStatus;
String lidStatus;

bool isDeliveryActive;
bool isDestinationReached;
bool wasLidOpen;

unsigned long currentMillis = 0;
unsigned long previousMillis = 0;

void setup() {
  Serial.begin(115200);
  espSerial.begin(115200);
  gpsSerial.begin(9600);

  while (!Serial) {
  ; // wait for serial port to connect. Needed for native USB port only
  }

  pinMode(LID_DANGER_PIN, INPUT);
  pinMode(SPEAKER_PIN, OUTPUT);
  pinMode(LED_BUILTIN, OUTPUT);

  battery.begin(3300, 1.47, &sigmoidal);

  deviceId = getDeviceId();
  currentDelay = DEFAULT_DELAY;
  currentStatus = RobotStatus::Idle;
  lidStatus = LID_CLOSED_COMMAND;
}

void loop() {
  currentMillis = millis();
  handleRobotStatus();
  sendRobotData();
  smartDelay(currentDelay);
}

void handleRobotStatus() {
  Serial.println("Current status: " + String((int)currentStatus));

  if (currentStatus == RobotStatus::Idle) {
    if (isLidOpen()) {
      Serial.println("Waiting for cargo");
      currentStatus = RobotStatus::WaitingForCargo;
    }
  }

  if (currentStatus == RobotStatus::WaitingForCargo) {
    if (!isLidOpen() && isStartDeliveryPressed() && isDeliveryActive) {
      Serial.println("Delivery started");
      currentStatus = RobotStatus::Delivering;
    }
  }

  if (currentStatus == RobotStatus::Delivering) {
    if (isDestinationReached) {
      Serial.println("Delivery is ready for pickup");
      currentStatus = RobotStatus::ReadyForPickup;
    }
  }

  if (isLidForceOpened()) {
    Serial.println("Cargo lid is force opened!");
    currentStatus = RobotStatus::Danger;
  }

  if (currentStatus == RobotStatus::Danger) {
    handleDangerState();
  }

  if (currentStatus == RobotStatus::ReadyForPickup) {
    if (isLidOpen()) {
      wasLidOpen = true;
      return;
    }
    if (!isLidOpen() && wasLidOpen) {
      wasLidOpen = false;
      Serial.println("Delivery is ready for pickup");
      currentStatus = RobotStatus::Returning;
    }
  }
}

bool isLidOpen() { 
  if (lidStatus == LID_OPEN_COMMAND) {
    Serial.println("Lid open");
    return true;
  }
  if (lidStatus == LID_CLOSED_COMMAND) {
    Serial.println("Lid closed");
    return false;
  }
  return false;
}

bool isStartDeliveryPressed() {
  return digitalRead(START_DELIVERY_PIN) == HIGH;
}

bool isLidForceOpened() {
  return digitalRead(LID_DANGER_PIN) == LOW;
}

void handleDangerState() {
  if (currentDelay != DANGER_DELAY) {
    currentDelay = DANGER_DELAY;
  }
  tone(SPEAKER_PIN, 700, 300);
  digitalWrite(LED_BUILTIN, !digitalRead(LED_BUILTIN));
}

void smartDelay(unsigned long interval) {
  if (currentMillis - previousMillis >= interval) {
    previousMillis = currentMillis;
    unsigned long start = millis();
    do {
      readFromGPS();
      readFromESP();
    } while (millis() - start < interval);
  }
}

void readFromESP() {
  if (!Serial.available()) {
    return;
  }

  String response = Serial.readString();
  response.trim();

  if (response.length() > 0) {
    Serial.println("Received data: " + response);

    if (response.indexOf(LID_OPEN_COMMAND) != - 1 || response.indexOf(LID_CLOSED_COMMAND) != -1) {
      lidStatus = response.substring(response.indexOf(LID_OPEN_COMMAND), response.indexOf("\n"));
    }
    if (response.indexOf(DELIVERY_ACTIVE_COMMAND) != -1) {
      isDeliveryActive = true;
    }
     if (response.indexOf(DESTINATION_REACHED_COMMAND) != -1) {
      isDestinationReached = true;
    }
  }
}

void readFromGPS() {
  while (gpsSerial.available()) {
    gps.encode(gpsSerial.read());
  }
}

void sendRobotData() {
  StaticJsonDocument<96> doc;

  doc["deviceId"] = deviceId;
  doc["status"] = (int)currentStatus;
  doc["batteryCharge"] = battery.level();
  doc["isCargoLidOpen"] = (lidStatus == LID_OPEN_COMMAND);

  JsonObject location = doc.createNestedObject("location");
  
  location["x"] = gps.location.lng();
  location["y"] = gps.location.lat();

  char jsonBuffer[192]; 
  serializeJson(doc, jsonBuffer, 192);
  Serial.println(jsonBuffer);
  espSerial.println(jsonBuffer);
}

String getDeviceId() {
  String deviceId;
  for (size_t i = 0; i < UniqueIDsize; i++) {
    deviceId += String(UniqueID[i], HEX);
  }
  deviceId.toUpperCase();
  return deviceId;
}