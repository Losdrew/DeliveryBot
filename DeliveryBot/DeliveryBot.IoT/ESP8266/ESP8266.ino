#include <ESP8266WiFi.h>
#include <WiFiClientSecure.h>
#include <ESP8266HTTPClient.h>
#include <ArduinoJson.h>
#include "certificate.h"
#include "robotstatus.h"
#include "commands.h"

const String WIFI_SSID = "Artem";
const String WIFI_PASS = "12345678";

const String HOST = "https://deliverybot.azurewebsites.net";
const String GET_ACTIVE_DELIVERY_LOCATION_ENDPOINT = "/api/delivery/active-delivery-location?deviceId=";
const String UPDATE_ROBOT_ENDPOINT = "/api/robot/update";
const String GET_CARGO_LID_STATUS_ENDPOINT = "/api/robot/lid-status?deviceId=";
const String GET_DELIVERY_ROUTE_ENDPOINT = "/api/geolocation/route/";
const String GET_NEAREST_COMPANY_ROUTE_ENDPOINT = "/api/Robot/nearest-company-route?deviceId=";

const int DEFAULT_DELAY = 3000;

WiFiClientSecure client;
HTTPClient https;

String currentData;
DynamicJsonDocument currentDataJSON(256);

String activeDeliveryLocation;
DynamicJsonDocument activeDeliveryLocationJSON(48);

String currentRoute;
DynamicJsonDocument currentRouteJSON(8192);

RobotStatus currentStatus;
bool isLidOpen;

void setup() {
  connectToWiFi();
  updateSystemTime();

  client.setTrustAnchors(&certificate);

  Serial.begin(115200);
  Serial1.begin(115200);
  
  while (!Serial) {
  ; // wait for serial port to connect. Needed for native USB port only
  }
}

void loop() {
  receiveCurrentData();

  if (!currentData.isEmpty()) {
    if (deserializeCurrentData()) {
      currentStatus = (RobotStatus)(currentDataJSON["status"].as<int>());
      
      if (currentStatus == RobotStatus::Idle || 
          currentStatus == RobotStatus::WaitingForCargo ||
          currentStatus == RobotStatus::ReadyForPickup) 
      {
        handleCargoLidStatus();
      }

      if (currentStatus == RobotStatus::WaitingForCargo) {
        handleActiveDelivery();
      }

      if (currentStatus == RobotStatus::Delivering) {
        handleDeliveryRoute();
        handleDestination();
      }

      if (currentStatus == RobotStatus::Returning) {
        handleNearestCompanyRoute();
      }

      sendRobotUpdate();
    }
  }
  
  delay(DEFAULT_DELAY);
}

void connectToWiFi() {
  WiFi.begin(WIFI_SSID, WIFI_PASS);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\nConnected to " + WiFi.localIP().toString());
}

void updateSystemTime() {
  // Set time via NTP, as required for x.509 validation
  configTime(3 * 3600, 0, "pool.ntp.org", "time.nist.gov");
  time_t now = time(nullptr);
  while (now < 8 * 3600 * 2) {
    delay(500);
    now = time(nullptr);
  }
  struct tm timeinfo;
  gmtime_r(&now, &timeinfo);
}

void receiveCurrentData() {
  if (Serial.available()) {
    currentData = Serial.readString();
    currentData.trim();
    Serial.println("Received data: " + currentData);
  }
}

void handleCargoLidStatus() {
  String response = getCargoLidStatus();
  String command;
  if (response == "LID_OPEN") {
    isLidOpen = true;
    command = LID_OPEN_COMMAND;
  } else if (response == "LID_CLOSED") {
    isLidOpen = false;
    command = LID_CLOSED_COMMAND;
  }
  Serial1.println(command);
}

void handleActiveDelivery() {
  activeDeliveryLocation = getActiveDeliveryLocation();
  if (deserializeActiveDeliveryLocation()) {
    Serial1.println(DELIVERY_ACTIVE_COMMAND);
  }
}

void handleDeliveryRoute() {
  if (currentRouteJSON.isNull() && !activeDeliveryLocationJSON.isNull()) {
    currentRoute = getDeliveryRoute();
    deserializeCurrentRoute();
    Serial.println("Current route: " + currentRoute);
  }
}

void handleDestination() {
  double currentX = currentDataJSON["x"];
  double currentY = currentDataJSON["y"];
  double destinationX = activeDeliveryLocationJSON["x"];
  double destinationY = activeDeliveryLocationJSON["y"];
  if (currentX == destinationX && currentY == destinationY) {
    Serial1.println(DESTINATION_REACHED_COMMAND);
  }
}

void handleNearestCompanyRoute() {
  if (currentRouteJSON.isNull()) {
    currentRoute = getNearestCompanyRoute();
    deserializeCurrentRoute();
    Serial.println("Current route: " + currentRoute);
  }
}

String getActiveDeliveryLocation() {
  String request = HOST + GET_ACTIVE_DELIVERY_LOCATION_ENDPOINT + currentDataJSON["deviceId"].as<String>();
  return performHttpGet(request);
}

String getCargoLidStatus() {
  String request = HOST + GET_CARGO_LID_STATUS_ENDPOINT + currentDataJSON["deviceId"].as<String>();
  return performHttpGet(request);
}

String getDeliveryRoute() {
  String currentCoordinates = String(currentDataJSON["location"]["x"].as<float>()) + "," + 
                              String(currentDataJSON["location"]["y"].as<float>());

  String destinationCoordinates = String(activeDeliveryLocationJSON["x"].as<float>()) + "," +
                                  String(activeDeliveryLocationJSON["y"].as<float>());

  String request = HOST + GET_DELIVERY_ROUTE_ENDPOINT + currentCoordinates + ";" + destinationCoordinates;
  return performHttpGet(request);
}

String getNearestCompanyRoute() {
  String request = HOST + GET_NEAREST_COMPANY_ROUTE_ENDPOINT + currentDataJSON["deviceId"].as<String>();
  return performHttpGet(request);
}

String performHttpGet(String request) {
  if (WiFi.status() != WL_CONNECTED) {
    Serial.printf("[HTTPS] Unable to connect\n");
    return "";
  }

  Serial.print("[HTTPS] begin...\n");

  if (https.begin(client, request)) {
    Serial.print("[HTTPS] GET...\n");

    int httpCode = https.GET();

    // httpCode will be negative on error
    if (httpCode > 0) {
      Serial.printf("[HTTPS] GET... code: %d\n", httpCode);

      if (httpCode == HTTP_CODE_OK || httpCode == HTTP_CODE_MOVED_PERMANENTLY) {
        String payload = https.getString();
        Serial.println(payload);
        deserializeJson(currentRouteJSON, https.getStream());
        return payload;
      }
    } else {
      Serial.printf("[HTTPS] GET... failed, error: %s\n", https.errorToString(httpCode).c_str());
    }

    https.end();
  }
  return "";
}

void sendRobotUpdate() {
  // Skip update if cargo lid data is outdated
  if (currentDataJSON["isCargoLidOpen"] != isLidOpen) {
    return;
  }
  
  if ((WiFi.status() != WL_CONNECTED)) {
    Serial.printf("[HTTPS] Unable to connect\n");
    return;
  }

  Serial.print("[HTTPS] begin...\n");

  String request = HOST + UPDATE_ROBOT_ENDPOINT;

  if (https.begin(client, request)) {

    https.addHeader("Content-Type", "application/json");
    Serial.print("[HTTP] POST...\n");
    
    int httpCode = https.POST(currentData);

    // httpCode will be negative on error
    if (httpCode > 0) {
      Serial.printf("[HTTPS] POST... code: %d\n", httpCode);

      if (httpCode == HTTP_CODE_OK || httpCode == HTTP_CODE_MOVED_PERMANENTLY) {
        String payload = https.getString();
        Serial.println(payload);
      }
    } else {
      Serial.printf("[HTTPS] POST... failed, error: %s\n", https.errorToString(httpCode).c_str());
    }

    https.end();
  }
}

bool deserializeCurrentData() {
  return deserializeJsonData(currentDataJSON, currentData);
}

bool deserializeActiveDeliveryLocation() {
  return deserializeJsonData(activeDeliveryLocationJSON, activeDeliveryLocation);
}

bool deserializeCurrentRoute() {
  return deserializeJsonData(currentRouteJSON, currentRoute);
}

bool deserializeJsonData(DynamicJsonDocument &doc, String &data) {
  Serial.println("Deserializing data...");
  DeserializationError error = deserializeJson(doc, data);
  if (error) {
    Serial.print(F("Deserialization failed: "));
    Serial.println(error.f_str());
    return false;
  }
  return true;
}